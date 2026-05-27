using HotelManagement.API.Models;

namespace HotelManagement.API.Services.Interfaces;

public interface IRoomService
{
    Task<IReadOnlyList<Room>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<Room> GetByIdAsync(int id, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Room>> GetByHotelIdAsync(int hotelId, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Room>> GetAvailableRoomsByHotelIdAsync(int hotelId, CancellationToken cancellationToken = default);

    Task<Room> CreateAsync(Room room, CancellationToken cancellationToken = default);

    Task<Room> UpdateAsync(int id, Room room, CancellationToken cancellationToken = default);

    Task DeleteAsync(int id, CancellationToken cancellationToken = default);
}
