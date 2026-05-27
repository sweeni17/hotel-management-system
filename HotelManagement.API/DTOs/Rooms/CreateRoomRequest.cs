namespace HotelManagement.API.DTOs.Rooms;

public class CreateRoomRequest
{
    public int RoomNumber { get; set; }

    public int RoomTypeId { get; set; }

    public int HotelId { get; set; }

    public bool IsAvailable { get; set; } = true;

    public string RoomStatus { get; set; } = "Available";
}
