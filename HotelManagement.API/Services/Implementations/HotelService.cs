using HotelManagement.API.Exceptions;
using HotelManagement.API.Models;
using HotelManagement.API.Repositories.Interfaces;
using HotelManagement.API.Services.Interfaces;

namespace HotelManagement.API.Services.Implementations;

public class HotelService : IHotelService
{
    private readonly IHotelRepository _hotelRepository;

    public HotelService(IHotelRepository hotelRepository)
    {
        _hotelRepository = hotelRepository;
    }

    public async Task<IReadOnlyList<Hotel>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _hotelRepository.GetAllAsync(cancellationToken);
    }

    public async Task<Hotel> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _hotelRepository.GetByIdAsync(id, cancellationToken)
            ?? throw new ResourceNotFoundException($"Hotel with id {id} was not found.");
    }

    public async Task<Hotel> CreateAsync(Hotel hotel, CancellationToken cancellationToken = default)
    {
        return await _hotelRepository.AddAsync(hotel, cancellationToken);
    }

    public async Task<Hotel> UpdateAsync(int id, Hotel hotel, CancellationToken cancellationToken = default)
    {
        hotel.HotelId = id;

        return await _hotelRepository.UpdateAsync(hotel, cancellationToken)
            ?? throw new ResourceNotFoundException($"Hotel with id {id} was not found.");
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var deleted = await _hotelRepository.DeleteAsync(id, cancellationToken);

        if (!deleted)
        {
            throw new ResourceNotFoundException($"Hotel with id {id} was not found.");
        }
    }
}
