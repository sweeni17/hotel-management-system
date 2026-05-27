using HotelManagement.API.Exceptions;
using HotelManagement.API.Models;
using HotelManagement.API.Repositories.Interfaces;
using HotelManagement.API.Services.Interfaces;

namespace HotelManagement.API.Services.Implementations;

public class RoomService : IRoomService
{
    private readonly IHotelRepository _hotelRepository;
    private readonly IRoomRepository _roomRepository;

    public RoomService(IRoomRepository roomRepository, IHotelRepository hotelRepository)
    {
        _roomRepository = roomRepository;
        _hotelRepository = hotelRepository;
    }

    public async Task<IReadOnlyList<Room>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _roomRepository.GetAllAsync(cancellationToken);
    }

    public async Task<Room> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _roomRepository.GetByIdAsync(id, cancellationToken)
            ?? throw new ResourceNotFoundException($"Room with id {id} was not found.");
    }

    public async Task<IReadOnlyList<Room>> GetByHotelIdAsync(int hotelId, CancellationToken cancellationToken = default)
    {
        await EnsureHotelExistsAsync(hotelId, cancellationToken);

        return await _roomRepository.GetByHotelIdAsync(hotelId, cancellationToken);
    }

    public async Task<IReadOnlyList<Room>> GetAvailableRoomsByHotelIdAsync(int hotelId, CancellationToken cancellationToken = default)
    {
        await EnsureHotelExistsAsync(hotelId, cancellationToken);

        return await _roomRepository.GetAvailableRoomsByHotelIdAsync(hotelId, cancellationToken);
    }

    public async Task<Room> CreateAsync(Room room, CancellationToken cancellationToken = default)
    {
        if (!room.HotelId.HasValue)
        {
            throw new BusinessRuleException("Room must belong to a hotel.");
        }

        await EnsureHotelExistsAsync(room.HotelId.Value, cancellationToken);

        return await _roomRepository.AddAsync(room, cancellationToken);
    }

    public async Task<Room> UpdateAsync(int id, Room room, CancellationToken cancellationToken = default)
    {
        if (room.HotelId.HasValue)
        {
            await EnsureHotelExistsAsync(room.HotelId.Value, cancellationToken);
        }

        room.RoomId = id;

        return await _roomRepository.UpdateAsync(room, cancellationToken)
            ?? throw new ResourceNotFoundException($"Room with id {id} was not found.");
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var deleted = await _roomRepository.DeleteAsync(id, cancellationToken);

        if (!deleted)
        {
            throw new ResourceNotFoundException($"Room with id {id} was not found.");
        }
    }

    private async Task EnsureHotelExistsAsync(int hotelId, CancellationToken cancellationToken)
    {
        var hotelExists = await _hotelRepository.ExistsAsync(hotelId, cancellationToken);

        if (!hotelExists)
        {
            throw new ResourceNotFoundException($"Hotel with id {hotelId} was not found.");
        }
    }
}
