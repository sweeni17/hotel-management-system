using FluentValidation;
using HotelManagement.API.DTOs.Rooms;

namespace HotelManagement.API.Validators.Rooms;

public class UpdateRoomRequestValidator : AbstractValidator<UpdateRoomRequest>
{
    private static readonly string[] AllowedStatuses = ["Available", "Maintenance", "Occupied"];

    public UpdateRoomRequestValidator()
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
