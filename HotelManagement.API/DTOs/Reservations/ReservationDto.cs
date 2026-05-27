namespace HotelManagement.API.DTOs.Reservations;

public class ReservationDto
{
    public int ReservationId { get; set; }

    public string GuestName { get; set; } = string.Empty;

    public string GuestEmail { get; set; } = string.Empty;

    public string GuestPhone { get; set; } = string.Empty;

    public DateOnly? CheckInDate { get; set; }

    public DateOnly? CheckOutDate { get; set; }

    public int? RoomId { get; set; }

    public int? RoomNumber { get; set; }

    public int? UserId { get; set; }

    public string? ReservationStatus { get; set; }

    public DateTime? CreatedAt { get; set; }
}
