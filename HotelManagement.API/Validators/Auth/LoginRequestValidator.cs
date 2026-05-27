using FluentValidation;
using HotelManagement.API.DTOs.Auth;

namespace HotelManagement.API.Validators.Auth;

public class LoginRequestValidator : AbstractValidator<LoginRequest>
{
    public LoginRequestValidator()
    {
        RuleFor(request => request.Email)
            .NotEmpty()
            .EmailAddress()
            .MaximumLength(255);

        RuleFor(request => request.Password)
            .NotEmpty();
    }
}
