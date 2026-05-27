namespace HotelManagement.API.DTOs.Rooms;

public class RoomDto
{
    public int RoomId { get; set; }

    public int? RoomNumber { get; set; }

    public int? RoomTypeId { get; set; }

    public string? RoomTypeName { get; set; }

    public decimal? PricePerNight { get; set; }

    public bool? IsAvailable { get; set; }

    public int? HotelId { get; set; }

    public string? HotelName { get; set; }

    public string? RoomStatus { get; set; }
}
