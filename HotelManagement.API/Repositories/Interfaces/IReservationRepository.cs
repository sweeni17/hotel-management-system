using HotelManagement.API.Models;

namespace HotelManagement.API.Repositories.Interfaces;

public interface IReservationRepository : IRepository<Reservation>
{
    Task<bool> HasOverlappingReservationAsync(
        int roomId,
        DateOnly checkInDate,
        DateOnly checkOutDate,
        int? excludedReservationId = null,
        CancellationToken cancellationToken = default);

    Task<Reservation?> GetWithRoomTypeAsync(int reservationId, CancellationToken cancellationToken = default);
}
