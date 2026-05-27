using HotelManagement.API.Data;
using HotelManagement.API.Models;
using HotelManagement.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HotelManagement.API.Repositories.Implementations;

public class RoomRepository : IRoomRepository
{
    private readonly HotelDbContext _dbContext;

    public RoomRepository(HotelDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyList<Room>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.Rooms
            .AsNoTracking()
            .Include(room => room.Hotel)
            .Include(room => room.RoomType)
            .Where(room => room.IsDeleted != true)
            .OrderBy(room => room.HotelId)
            .ThenBy(room => room.RoomNumber)
            .ToListAsync(cancellationToken);
    }

    public async Task<Room?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Rooms
            .Include(room => room.Hotel)
            .Include(room => room.RoomType)
            .Include(room => room.Amenities)
            .FirstOrDefaultAsync(
                room => room.RoomId == id && room.IsDeleted != true,
                cancellationToken);
    }

    public async Task<Room> AddAsync(Room entity, CancellationToken cancellationToken = default)
    {
        entity.CreatedAt ??= DateTime.UtcNow;
        entity.IsDeleted ??= false;

        await _dbContext.Rooms.AddAsync(entity, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return entity;
    }

    public async Task<Room?> UpdateAsync(Room entity, CancellationToken cancellationToken = default)
    {
        var existingRoom = await _dbContext.Rooms
            .FirstOrDefaultAsync(
                room => room.RoomId == entity.RoomId && room.IsDeleted != true,
                cancellationToken);

        if (existingRoom is null)
        {
            return null;
        }

        existingRoom.RoomNumber = entity.RoomNumber;
        existingRoom.RoomTypeId = entity.RoomTypeId;
        existingRoom.IsAvailable = entity.IsAvailable;
        existingRoom.HotelId = entity.HotelId;
        existingRoom.RoomStatus = entity.RoomStatus;
        existingRoom.UpdatedAt = DateTime.UtcNow;

        await _dbContext.SaveChangesAsync(cancellationToken);

        return existingRoom;
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var room = await _dbContext.Rooms
            .FirstOrDefaultAsync(
                room => room.RoomId == id && room.IsDeleted != true,
                cancellationToken);

        if (room is null)
        {
            return false;
        }

        room.IsDeleted = true;
        room.UpdatedAt = DateTime.UtcNow;

        await _dbContext.SaveChangesAsync(cancellationToken);

        return true;
    }

    public async Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Rooms
            .AnyAsync(
                room => room.RoomId == id && room.IsDeleted != true,
                cancellationToken);
    }

    public async Task<IReadOnlyList<Room>> GetByHotelIdAsync(int hotelId, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Rooms
            .AsNoTracking()
            .Include(room => room.RoomType)
            .Where(room => room.HotelId == hotelId && room.IsDeleted != true)
            .OrderBy(room => room.RoomNumber)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Room>> GetAvailableRoomsByHotelIdAsync(int hotelId, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Rooms
            .AsNoTracking()
            .Include(room => room.RoomType)
            .Where(room =>
                room.HotelId == hotelId &&
                room.IsDeleted != true &&
                room.IsAvailable == true &&
                room.RoomStatus == "Available")
            .OrderBy(room => room.RoomNumber)
            .ToListAsync(cancellationToken);
    }
}
