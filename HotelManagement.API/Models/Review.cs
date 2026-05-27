using System;
using System.Collections.Generic;

namespace HotelManagement.API.Models;

public partial class Review
{
    public int ReviewId { get; set; }

    public int? ReservationId { get; set; }

    public int? Rating { get; set; }

    public string? Comment { get; set; }

    public DateOnly? ReviewDate { get; set; }

    public virtual Reservation? Reservation { get; set; }
}
