using HotelManagement.API.Exceptions;
using HotelManagement.API.Models;
using HotelManagement.API.Repositories.Interfaces;
using HotelManagement.API.Services.Interfaces;

namespace HotelManagement.API.Services.Implementations;

public class ReservationService : IReservationService
{
    private readonly IReservationRepository _reservationRepository;
    private readonly IRoomRepository _roomRepository;

    public ReservationService(IReservationRepository reservationRepository, IRoomRepository roomRepository)
    {
        _reservationRepository = reservationRepository;
        _roomRepository = roomRepository;
    }

    public async Task<IReadOnlyList<Reservation>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _reservationRepository.GetAllAsync(cancellationToken);
    }

    public async Task<Reservation> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _reservationRepository.GetByIdAsync(id, cancellationToken)
            ?? throw new ResourceNotFoundException($"Reservation with id {id} was not found.");
    }

    public async Task<Reservation> CreateAsync(Reservation reservation, CancellationToken cancellationToken = default)
    {
        await ValidateReservationAsync(reservation, null, cancellationToken);

        reservation.ReservationStatus ??= "Confirmed";

        return await _reservationRepository.AddAsync(reservation, cancellationToken);
    }

    public async Task<Reservation> UpdateAsync(int id, Reservation reservation, CancellationToken cancellationToken = default)
    {
        await ValidateReservationAsync(reservation, id, cancellationToken);

        reservation.ReservationId = id;

        return await _reservationRepository.UpdateAsync(reservation, cancellationToken)
            ?? throw new ResourceNotFoundException($"Reservation with id {id} was not found.");
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var deleted = await _reservationRepository.DeleteAsync(id, cancellationToken);

        if (!deleted)
        {
            throw new ResourceNotFoundException($"Reservation with id {id} was not found.");
        }
    }

    public async Task<bool> IsRoomAvailableAsync(
        int roomId,
        DateOnly checkInDate,
        DateOnly checkOutDate,
        int? excludedReservationId = null,
        CancellationToken cancellationToken = default)
    {
        ValidateDateRange(checkInDate, checkOutDate);

        var room = await _roomRepository.GetByIdAsync(roomId, cancellationToken)
            ?? throw new ResourceNotFoundException($"Room with id {roomId} was not found.");

        if (room.IsDeleted == true || room.IsAvailable != true || room.RoomStatus != "Available")
        {
            return false;
        }

        var hasOverlap = await _reservationRepository.HasOverlappingReservationAsync(
            roomId,
            checkInDate,
            checkOutDate,
            excludedReservationId,
            cancellationToken);

        return !hasOverlap;
    }

    private async Task ValidateReservationAsync(
        Reservation reservation,
        int? excludedReservationId,
        CancellationToken cancellationToken)
    {
        if (!reservation.RoomId.HasValue)
        {
            throw new BusinessRuleException("Reservation must be assigned to a room.");
        }

        if (!reservation.UserId.HasValue)
        {
            throw new BusinessRuleException("Reservation must store the logged-in user id.");
        }

        if (!reservation.CheckInDate.HasValue || !reservation.CheckOutDate.HasValue)
        {
            throw new BusinessRuleException("Reservation requires check-in and check-out dates.");
        }

        ValidateDateRange(reservation.CheckInDate.Value, reservation.CheckOutDate.Value);

        var available = await IsRoomAvailableAsync(
            reservation.RoomId.Value,
            reservation.CheckInDate.Value,
            reservation.CheckOutDate.Value,
            excludedReservationId,
            cancellationToken);

        if (!available)
        {
            throw new BusinessRuleException("Room is not available for the selected dates.");
        }
    }

    private static void ValidateDateRange(DateOnly checkInDate, DateOnly checkOutDate)
    {
        if (checkOutDate <= checkInDate)
        {
            throw new BusinessRuleException("Check-out date must be after check-in date.");
        }
    }
}
