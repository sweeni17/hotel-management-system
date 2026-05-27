using AutoMapper;
using HotelManagement.API.DTOs.Rooms;
using HotelManagement.API.Exceptions;
using HotelManagement.API.Models;
using HotelManagement.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RoomsController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IRoomService _roomService;

    public RoomsController(IRoomService roomService, IMapper mapper)
    {
        _roomService = roomService;
        _mapper = mapper;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<RoomDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<RoomDto>>> GetAllAsync(CancellationToken cancellationToken)
    {
        var rooms = await _roomService.GetAllAsync(cancellationToken);

        return Ok(_mapper.Map<IReadOnlyList<RoomDto>>(rooms));
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(RoomDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<RoomDto>> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        try
        {
            var room = await _roomService.GetByIdAsync(id, cancellationToken);

            return Ok(_mapper.Map<RoomDto>(room));
        }
        catch (ResourceNotFoundException exception)
        {
            return NotFound(new { message = exception.Message });
        }
    }

    [HttpGet("by-hotel/{hotelId:int}")]
    [ProducesResponseType(typeof(IReadOnlyList<RoomDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IReadOnlyList<RoomDto>>> GetByHotelIdAsync(int hotelId, CancellationToken cancellationToken)
    {
        try
        {
            var rooms = await _roomService.GetByHotelIdAsync(hotelId, cancellationToken);

            return Ok(_mapper.Map<IReadOnlyList<RoomDto>>(rooms));
        }
        catch (ResourceNotFoundException exception)
        {
            return NotFound(new { message = exception.Message });
        }
    }

    [HttpGet("available/by-hotel/{hotelId:int}")]
    [ProducesResponseType(typeof(IReadOnlyList<RoomDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IReadOnlyList<RoomDto>>> GetAvailableByHotelIdAsync(int hotelId, CancellationToken cancellationToken)
    {
        try
        {
            var rooms = await _roomService.GetAvailableRoomsByHotelIdAsync(hotelId, cancellationToken);

            return Ok(_mapper.Map<IReadOnlyList<RoomDto>>(rooms));
        }
        catch (ResourceNotFoundException exception)
        {
            return NotFound(new { message = exception.Message });
        }
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(RoomDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<RoomDto>> CreateAsync(CreateRoomRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var room = _mapper.Map<Room>(request);
            var createdRoom = await _roomService.CreateAsync(room, cancellationToken);
            var response = _mapper.Map<RoomDto>(createdRoom);

            return CreatedAtAction(nameof(GetByIdAsync), new { id = response.RoomId }, response);
        }
        catch (BusinessRuleException exception)
        {
            return BadRequest(new { message = exception.Message });
        }
        catch (ResourceNotFoundException exception)
        {
            return NotFound(new { message = exception.Message });
        }
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(RoomDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<RoomDto>> UpdateAsync(int id, UpdateRoomRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var room = _mapper.Map<Room>(request);
            var updatedRoom = await _roomService.UpdateAsync(id, room, cancellationToken);

            return Ok(_mapper.Map<RoomDto>(updatedRoom));
        }
        catch (BusinessRuleException exception)
        {
            return BadRequest(new { message = exception.Message });
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
            await _roomService.DeleteAsync(id, cancellationToken);

            return NoContent();
        }
        catch (ResourceNotFoundException exception)
        {
            return NotFound(new { message = exception.Message });
        }
    }
}
