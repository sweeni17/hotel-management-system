using AutoMapper;
using HotelManagement.API.DTOs.Hotels;
using HotelManagement.API.Exceptions;
using HotelManagement.API.Models;
using HotelManagement.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HotelsController : ControllerBase
{
    private readonly IHotelService _hotelService;
    private readonly IMapper _mapper;

    public HotelsController(IHotelService hotelService, IMapper mapper)
    {
        _hotelService = hotelService;
        _mapper = mapper;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<HotelDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<HotelDto>>> GetAllAsync(CancellationToken cancellationToken)
    {
        var hotels = await _hotelService.GetAllAsync(cancellationToken);

        return Ok(_mapper.Map<IReadOnlyList<HotelDto>>(hotels));
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(HotelDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<HotelDto>> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        try
        {
            var hotel = await _hotelService.GetByIdAsync(id, cancellationToken);

            return Ok(_mapper.Map<HotelDto>(hotel));
        }
        catch (ResourceNotFoundException exception)
        {
            return NotFound(new { message = exception.Message });
        }
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(HotelDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<HotelDto>> CreateAsync(CreateHotelRequest request, CancellationToken cancellationToken)
    {
        var hotel = _mapper.Map<Hotel>(request);
        var createdHotel = await _hotelService.CreateAsync(hotel, cancellationToken);
        var response = _mapper.Map<HotelDto>(createdHotel);

        return CreatedAtAction(nameof(GetByIdAsync), new { id = response.HotelId }, response);
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(HotelDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<HotelDto>> UpdateAsync(int id, UpdateHotelRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var hotel = _mapper.Map<Hotel>(request);
            var updatedHotel = await _hotelService.UpdateAsync(id, hotel, cancellationToken);

            return Ok(_mapper.Map<HotelDto>(updatedHotel));
        }
        catch (ResourceNotFoundException exception)
        {
            return NotFound(new { message = exception.Message });
        }
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteAsync(int id, CancellationToken cancellationToken)
    {
        try
        {
            await _hotelService.DeleteAsync(id, cancellationToken);

            return NoContent();
        }
        catch (ResourceNotFoundException exception)
        {
            return NotFound(new { message = exception.Message });
        }
    }
}
