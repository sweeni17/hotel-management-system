using HotelManagement.API.Models;

namespace HotelManagement.API.Repositories.Interfaces;

public interface IPaymentRepository : IRepository<Payment>
{
    Task<IReadOnlyList<Payment>> GetByReservationIdAsync(int reservationId, CancellationToken cancellationToken = default);
}
