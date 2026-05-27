using AutoMapper;
using HotelManagement.API.DTOs.Reservations;
using HotelManagement.API.Exceptions;
using HotelManagement.API.Models;
using HotelManagement.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReservationsController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IReservationService _reservationService;

    public ReservationsController(IReservationService reservationService, IMapper mapper)
    {
        _reservationService = reservationService;
        _mapper = mapper;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<ReservationDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<ReservationDto>>> GetAllAsync(CancellationToken cancellationToken)
    {
        var reservations = await _reservationService.GetAllAsync(cancellationToken);

        return Ok(_mapper.Map<IReadOnlyList<ReservationDto>>(reservations));
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(ReservationDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ReservationDto>> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        try
        {
            var reservation = await _reservationService.GetByIdAsync(id, cancellationToken);

            return Ok(_mapper.Map<ReservationDto>(reservation));
        }
        catch (ResourceNotFoundException exception)
        {
            return NotFound(new { message = exception.Message });
        }
    }

    [HttpGet("availability")]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CheckAvailabilityAsync(
        [FromQuery] int roomId,
        [FromQuery] DateOnly checkInDate,
        [FromQuery] DateOnly checkOutDate,
        CancellationToken cancellationToken)
    {
        try
        {
            var isAvailable = await _reservationService.IsRoomAvailableAsync(
                roomId,
                checkInDate,
                checkOutDate,
                null,
                cancellationToken);

            return Ok(new { roomId, checkInDate, checkOutDate, isAvailable });
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

    [HttpPost]
    [Authorize(Roles = "Admin,User")]
    [ProducesResponseType(typeof(ReservationDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ReservationDto>> CreateAsync(CreateReservationRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var reservation = _mapper.Map<Reservation>(request);
            var createdReservation = await _reservationService.CreateAsync(reservation, cancellationToken);
            var response = _mapper.Map<ReservationDto>(createdReservation);

            return CreatedAtAction(nameof(GetByIdAsync), new { id = response.ReservationId }, response);
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
    [Authorize(Roles = "Admin,User")]
    [ProducesResponseType(typeof(ReservationDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ReservationDto>> UpdateAsync(int id, UpdateReservationRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var reservation = _mapper.Map<Reservation>(request);
            var updatedReservation = await _reservationService.UpdateAsync(id, reservation, cancellationToken);

            return Ok(_mapper.Map<ReservationDto>(updatedReservation));
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
    [Authorize(Roles = "Admin,User")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteAsync(int id, CancellationToken cancellationToken)
    {
        try
        {
            await _reservationService.DeleteAsync(id, cancellationToken);

            return NoContent();
        }
        catch (ResourceNotFoundException exception)
        {
            return NotFound(new { message = exception.Message });
        }
    }
}
