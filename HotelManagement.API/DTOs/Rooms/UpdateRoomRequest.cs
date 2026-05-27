namespace HotelManagement.API.DTOs.Rooms;

public class UpdateRoomRequest
{
    public int RoomNumber { get; set; }

    public int RoomTypeId { get; set; }

    public int HotelId { get; set; }

    public bool IsAvailable { get; set; }

    public string RoomStatus { get; set; } = string.Empty;
}
