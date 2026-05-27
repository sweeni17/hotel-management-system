using HotelManagement.API.Models;

namespace HotelManagement.API.Services.Interfaces;

public interface IHotelService
{
    Task<IReadOnlyList<Hotel>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<Hotel> GetByIdAsync(int id, CancellationToken cancellationToken = default);

    Task<Hotel> CreateAsync(Hotel hotel, CancellationToken cancellationToken = default);

    Task<Hotel> UpdateAsync(int id, Hotel hotel, CancellationToken cancellationToken = default);

    Task DeleteAsync(int id, CancellationToken cancellationToken = default);
}
