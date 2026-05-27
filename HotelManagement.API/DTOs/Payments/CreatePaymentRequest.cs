namespace HotelManagement.API.DTOs.Payments;

public class CreatePaymentRequest
{
    public int ReservationId { get; set; }

    public string? PaymentStatus { get; set; }
}
