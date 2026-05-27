using FluentValidation;
using HotelManagement.API.DTOs.Hotels;

namespace HotelManagement.API.Validators.Hotels;

public class CreateHotelRequestValidator : AbstractValidator<CreateHotelRequest>
{
    public CreateHotelRequestValidator()
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
