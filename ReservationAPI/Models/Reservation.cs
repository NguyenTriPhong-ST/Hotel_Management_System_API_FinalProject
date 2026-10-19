using System;
using System.Collections.Generic;

namespace ReservationAPI.Models;

public partial class Reservation
{
    public int Id { get; set; }

    public string ReservationCode { get; set; } = null!;

    public int GuestId { get; set; }

    public int RoomId { get; set; }

    public int RoomTypeId { get; set; }

    public DateOnly CheckInDate { get; set; }

    public DateOnly CheckOutDate { get; set; }

    public int NumberOfGuests { get; set; }

    public int CapacitySnapshot { get; set; }

    public decimal NightlyRate { get; set; }

    public byte Status { get; set; }

    public string? Notes { get; set; }

    public int CreatedByAccountId { get; set; }

    public DateTime? CancelledAt { get; set; }

    public int? CancelledByAccountId { get; set; }

    public string? CancellationReason { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public byte[] RowVersion { get; set; } = null!;

    public virtual Account? CancelledByAccount { get; set; }

    public virtual Account CreatedByAccount { get; set; } = null!;

    public virtual Guest Guest { get; set; } = null!;

    public virtual Room Room { get; set; } = null!;

    public virtual RoomType RoomType { get; set; } = null!;

    public virtual Stay? Stay { get; set; }
}
