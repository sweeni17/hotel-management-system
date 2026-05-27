using FluentValidation;
using HotelManagement.API.DTOs.Rooms;

namespace HotelManagement.API.Validators.Rooms;

public class CreateRoomRequestValidator : AbstractValidator<CreateRoomRequest>
{
    private static readonly string[] AllowedStatuses = ["Available", "Maintenance", "Occupied"];

    public CreateRoomRequestValidator()
    {
        RuleFor(request => request.RoomNumber)
            .GreaterThan(0);

        RuleFor(request => request.RoomTypeId)
            .GreaterThan(0);

        RuleFor(request => request.HotelId)
            .GreaterThan(0);

        RuleFor(request => request.RoomStatus)
            .NotEmpty()
            .Must(status => AllowedStatuses.Contains(status))
            .WithMessage("Room status must be Available, Maintenance, or Occupied.");
    }
}
