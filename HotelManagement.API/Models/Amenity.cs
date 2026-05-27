using System;
using System.Collections.Generic;

namespace HotelManagement.API.Models;

public partial class Amenity
{
    public int AmenityId { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public virtual ICollection<Hotel> Hotels { get; set; } = new List<Hotel>();

    public virtual ICollection<Room> Rooms { get; set; } = new List<Room>();
}
