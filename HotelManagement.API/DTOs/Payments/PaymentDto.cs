namespace HotelManagement.API.DTOs.Payments;

public class PaymentDto
{
    public int PaymentId { get; set; }

    public int? ReservationId { get; set; }

    public decimal? Amount { get; set; }

    public DateOnly? PaymentDate { get; set; }

    public string? PaymentStatus { get; set; }
}
