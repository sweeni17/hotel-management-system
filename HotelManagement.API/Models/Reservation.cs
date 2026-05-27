using System;
using System.Collections.Generic;

namespace HotelManagement.API.Models;

public partial class Reservation
{
    public int ReservationId { get; set; }

    public string GuestName { get; set; } = null!;

    public string GuestEmail { get; set; } = null!;

    public string GuestPhone { get; set; } = null!;

    public DateOnly? CheckInDate { get; set; }

    public DateOnly? CheckOutDate { get; set; }

    public int? RoomId { get; set; }

    public string? ReservationStatus { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public bool? IsDeleted { get; set; }

    public int? UserId { get; set; }

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();

    public virtual Room? Room { get; set; }

    public virtual User? User { get; set; }
}
