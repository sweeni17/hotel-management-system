using System;
using System.Collections.Generic;

namespace HotelManagement.API.Models;

public partial class Hotel
{
    public int HotelId { get; set; }

    public string? Name { get; set; }

    public string? Location { get; set; }

    public string? Description { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public bool? IsDeleted { get; set; }

    public virtual ICollection<Room> Rooms { get; set; } = new List<Room>();

    public virtual ICollection<Amenity> Amenities { get; set; } = new List<Amenity>();
}
