using HotelManagement.API.Exceptions;
using HotelManagement.API.Models;
using HotelManagement.API.Repositories.Interfaces;
using HotelManagement.API.Services.Interfaces;

namespace HotelManagement.API.Services.Implementations;

public class PaymentService : IPaymentService
{
    private readonly IPaymentRepository _paymentRepository;
    private readonly IReservationRepository _reservationRepository;

    public PaymentService(IPaymentRepository paymentRepository, IReservationRepository reservationRepository)
    {
        _paymentRepository = paymentRepository;
        _reservationRepository = reservationRepository;
    }

    public async Task<IReadOnlyList<Payment>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _paymentRepository.GetAllAsync(cancellationToken);
    }

    public async Task<Payment> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _paymentRepository.GetByIdAsync(id, cancellationToken)
            ?? throw new ResourceNotFoundException($"Payment with id {id} was not found.");
    }

    public async Task<IReadOnlyList<Payment>> GetByReservationIdAsync(int reservationId, CancellationToken cancellationToken = default)
    {
        await EnsureReservationExistsAsync(reservationId, cancellationToken);

        return await _paymentRepository.GetByReservationIdAsync(reservationId, cancellationToken);
    }

    public async Task<Payment> CreateForReservationAsync(
        int reservationId,
        string? paymentStatus = null,
        CancellationToken cancellationToken = default)
    {
        var amount = await CalculateAmountAsync(reservationId, cancellationToken);

        var payment = new Payment
        {
            ReservationId = reservationId,
            Amount = amount,
            PaymentDate = DateOnly.FromDateTime(DateTime.UtcNow),
            PaymentStatus = string.IsNullOrWhiteSpace(paymentStatus) ? "Pending" : paymentStatus
        };

        return await _paymentRepository.AddAsync(payment, cancellationToken);
    }

    public async Task<decimal> CalculateAmountAsync(int reservationId, CancellationToken cancellationToken = default)
    {
        var reservation = await _reservationRepository.GetWithRoomTypeAsync(reservationId, cancellationToken)
            ?? throw new ResourceNotFoundException($"Reservation with id {reservationId} was not found.");

        if (!reservation.CheckInDate.HasValue || !reservation.CheckOutDate.HasValue)
        {
            throw new BusinessRuleException("Reservation requires check-in and check-out dates.");
        }

        if (reservation.CheckOutDate <= reservation.CheckInDate)
        {
            throw new BusinessRuleException("Check-out date must be after check-in date.");
        }

        var pricePerNight = reservation.Room?.RoomType?.PricePerNight
            ?? throw new BusinessRuleException("Room type price is required to calculate payment amount.");

        var nights = reservation.CheckOutDate.Value.DayNumber - reservation.CheckInDate.Value.DayNumber;

        return nights * pricePerNight;
    }

    private async Task EnsureReservationExistsAsync(int reservationId, CancellationToken cancellationToken)
    {
        var exists = await _reservationRepository.ExistsAsync(reservationId, cancellationToken);

        if (!exists)
        {
            throw new ResourceNotFoundException($"Reservation with id {reservationId} was not found.");
        }
    }
}
