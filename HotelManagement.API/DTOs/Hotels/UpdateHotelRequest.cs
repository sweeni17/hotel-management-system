namespace HotelManagement.API.DTOs.Hotels;

public class UpdateHotelRequest
{
    public string Name { get; set; } = string.Empty;

    public string Location { get; set; } = string.Empty;

    public string? Description { get; set; }
}
