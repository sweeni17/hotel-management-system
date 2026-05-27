using HotelManagement.API.Models;

namespace HotelManagement.API.Services.Interfaces;

public interface IReservationService
{
    Task<IReadOnlyList<Reservation>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<Reservation> GetByIdAsync(int id, CancellationToken cancellationToken = default);

    Task<Reservation> CreateAsync(Reservation reservation, CancellationToken cancellationToken = default);

    Task<Reservation> UpdateAsync(int id, Reservation reservation, CancellationToken cancellationToken = default);

    Task DeleteAsync(int id, CancellationToken cancellationToken = default);

    Task<bool> IsRoomAvailableAsync(
        int roomId,
        DateOnly checkInDate,
        DateOnly checkOutDate,
        int? excludedReservationId = null,
        CancellationToken cancellationToken = default);
}
