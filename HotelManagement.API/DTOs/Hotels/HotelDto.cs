namespace HotelManagement.API.DTOs.Hotels;

public class HotelDto
{
    public int HotelId { get; set; }

    public string? Name { get; set; }

    public string? Location { get; set; }

    public string? Description { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}
