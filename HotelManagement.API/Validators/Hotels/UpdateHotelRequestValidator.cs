using FluentValidation;
using HotelManagement.API.DTOs.Hotels;

namespace HotelManagement.API.Validators.Hotels;

public class UpdateHotelRequestValidator : AbstractValidator<UpdateHotelRequest>
{
    public UpdateHotelRequestValidator()
    {
        RuleFor(request => request.Name)
            .NotEmpty()
            .MaximumLength(255);

        RuleFor(request => request.Location)
            .NotEmpty()
            .MaximumLength(255);

        RuleFor(request => request.Description)
            .MaximumLength(4000);
    }
}
