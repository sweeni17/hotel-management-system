using HotelManagement.API.Data;
using HotelManagement.API.Models;
using HotelManagement.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HotelManagement.API.Repositories.Implementations;

public class HotelRepository : IHotelRepository
{
    private readonly HotelDbContext _dbContext;

    public HotelRepository(HotelDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyList<Hotel>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.Hotels
            .AsNoTracking()
            .Where(hotel => hotel.IsDeleted != true)
            .OrderBy(hotel => hotel.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<Hotel?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Hotels
            .Include(hotel => hotel.Rooms.Where(room => room.IsDeleted != true))
            .Include(hotel => hotel.Amenities)
            .FirstOrDefaultAsync(
                hotel => hotel.HotelId == id && hotel.IsDeleted != true,
                cancellationToken);
    }

    public async Task<Hotel> AddAsync(Hotel entity, CancellationToken cancellationToken = default)
    {
        entity.CreatedAt ??= DateTime.UtcNow;
        entity.IsDeleted ??= false;

        await _dbContext.Hotels.AddAsync(entity, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return entity;
    }

    public async Task<Hotel?> UpdateAsync(Hotel entity, CancellationToken cancellationToken = default)
    {
        var existingHotel = await _dbContext.Hotels
            .FirstOrDefaultAsync(
                hotel => hotel.HotelId == entity.HotelId && hotel.IsDeleted != true,
                cancellationToken);

        if (existingHotel is null)
        {
            return null;
        }

        existingHotel.Name = entity.Name;
        existingHotel.Location = entity.Location;
        existingHotel.Description = entity.Description;
        existingHotel.UpdatedAt = DateTime.UtcNow;

        await _dbContext.SaveChangesAsync(cancellationToken);

        return existingHotel;
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var hotel = await _dbContext.Hotels
            .FirstOrDefaultAsync(
                hotel => hotel.HotelId == id && hotel.IsDeleted != true,
                cancellationToken);

        if (hotel is null)
        {
            return false;
        }

        hotel.IsDeleted = true;
        hotel.UpdatedAt = DateTime.UtcNow;

        await _dbContext.SaveChangesAsync(cancellationToken);

        return true;
    }

    public async Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Hotels
            .AnyAsync(
                hotel => hotel.HotelId == id && hotel.IsDeleted != true,
                cancellationToken);
    }

    public async Task<IReadOnlyList<Hotel>> SearchByLocationAsync(string location, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Hotels
            .AsNoTracking()
            .Where(hotel =>
                hotel.IsDeleted != true &&
                hotel.Location != null &&
                hotel.Location.Contains(location))
            .OrderBy(hotel => hotel.Name)
            .ToListAsync(cancellationToken);
    }
}
