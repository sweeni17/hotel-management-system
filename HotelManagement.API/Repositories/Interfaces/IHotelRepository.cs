using HotelManagement.API.Models;

namespace HotelManagement.API.Repositories.Interfaces;

public interface IHotelRepository : IRepository<Hotel>
{
    Task<IReadOnlyList<Hotel>> SearchByLocationAsync(string location, CancellationToken cancellationToken = default);
}
