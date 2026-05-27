using FluentValidation;
using HotelManagement.API.DTOs.Payments;

namespace HotelManagement.API.Validators.Payments;

public class CreatePaymentRequestValidator : AbstractValidator<CreatePaymentRequest>
{
    private static readonly string[] AllowedStatuses = ["Pending", "Paid", "Failed", "Refunded"];

    public CreatePaymentRequestValidator()
    {
        RuleFor(request => request.ReservationId)
            .GreaterThan(0);

        RuleFor(request => request.PaymentStatus)
            .Must(status => string.IsNullOrWhiteSpace(status) || AllowedStatuses.Contains(status))
            .WithMessage("Payment status must be Pending, Paid, Failed, or Refunded.");
    }
}
