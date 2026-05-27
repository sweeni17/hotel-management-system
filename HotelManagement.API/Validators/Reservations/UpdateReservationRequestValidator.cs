using FluentValidation;
using HotelManagement.API.DTOs.Reservations;

namespace HotelManagement.API.Validators.Reservations;

public class UpdateReservationRequestValidator : AbstractValidator<UpdateReservationRequest>
{
    private static readonly string[] AllowedStatuses = ["Confirmed", "Completed", "Cancelled"];

    public UpdateReservationRequestValidator()
    {
        RuleFor(request => request.GuestName)
            .NotEmpty()
            .MaximumLength(255);

        RuleFor(request => request.GuestEmail)
            .NotEmpty()
            .EmailAddress()
            .MaximumLength(255);

        RuleFor(request => request.GuestPhone)
            .NotEmpty()
            .MaximumLength(20);

        RuleFor(request => request.RoomId)
            .GreaterThan(0);

        RuleFor(request => request.UserId)
            .GreaterThan(0);

        RuleFor(request => request.CheckInDate)
            .NotEmpty();

        RuleFor(request => request.CheckOutDate)
            .NotEmpty()
            .GreaterThan(request => request.CheckInDate)
            .WithMessage("Check-out date must be after check-in date.");

        RuleFor(request => request.ReservationStatus)
            .NotEmpty()
            .Must(status => AllowedStatuses.Contains(status))
            .WithMessage("Reservation status must be Confirmed, Completed, or Cancelled.");
    }
}
