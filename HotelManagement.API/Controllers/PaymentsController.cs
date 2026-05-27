using AutoMapper;
using HotelManagement.API.DTOs.Payments;
using HotelManagement.API.Exceptions;
using HotelManagement.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PaymentsController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IPaymentService _paymentService;

    public PaymentsController(IPaymentService paymentService, IMapper mapper)
    {
        _paymentService = paymentService;
        _mapper = mapper;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<PaymentDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<PaymentDto>>> GetAllAsync(CancellationToken cancellationToken)
    {
        var payments = await _paymentService.GetAllAsync(cancellationToken);

        return Ok(_mapper.Map<IReadOnlyList<PaymentDto>>(payments));
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(PaymentDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<PaymentDto>> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        try
        {
            var payment = await _paymentService.GetByIdAsync(id, cancellationToken);

            return Ok(_mapper.Map<PaymentDto>(payment));
        }
        catch (ResourceNotFoundException exception)
        {
            return NotFound(new { message = exception.Message });
        }
    }

    [HttpGet("by-reservation/{reservationId:int}")]
    [ProducesResponseType(typeof(IReadOnlyList<PaymentDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IReadOnlyList<PaymentDto>>> GetByReservationIdAsync(int reservationId, CancellationToken cancellationToken)
    {
        try
        {
            var payments = await _paymentService.GetByReservationIdAsync(reservationId, cancellationToken);

            return Ok(_mapper.Map<IReadOnlyList<PaymentDto>>(payments));
        }
        catch (ResourceNotFoundException exception)
        {
            return NotFound(new { message = exception.Message });
        }
    }

    [HttpGet("calculate/{reservationId:int}")]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CalculateAmountAsync(int reservationId, CancellationToken cancellationToken)
    {
        try
        {
            var amount = await _paymentService.CalculateAmountAsync(reservationId, cancellationToken);

            return Ok(new { reservationId, amount });
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
    [ProducesResponseType(typeof(PaymentDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<PaymentDto>> CreateAsync(CreatePaymentRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var payment = await _paymentService.CreateForReservationAsync(
                request.ReservationId,
                request.PaymentStatus,
                cancellationToken);

            var response = _mapper.Map<PaymentDto>(payment);

            return CreatedAtAction(nameof(GetByIdAsync), new { id = response.PaymentId }, response);
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
}
