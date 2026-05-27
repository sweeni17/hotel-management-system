using HotelManagement.API.Models;

namespace HotelManagement.API.Repositories.Interfaces;

public interface IRoomRepository : IRepository<Room>
{
    Task<IReadOnlyList<Room>> GetByHotelIdAsync(int hotelId, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Room>> GetAvailableRoomsByHotelIdAsync(int hotelId, CancellationToken cancellationToken = default);
}
