using HotelManagement.API.Models;

namespace HotelManagement.API.Services.Interfaces;

public interface IPaymentService
{
    Task<IReadOnlyList<Payment>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<Payment> GetByIdAsync(int id, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Payment>> GetByReservationIdAsync(int reservationId, CancellationToken cancellationToken = default);

    Task<Payment> CreateForReservationAsync(int reservationId, string? paymentStatus = null, CancellationToken cancellationToken = default);

    Task<decimal> CalculateAmountAsync(int reservationId, CancellationToken cancellationToken = default);
}
