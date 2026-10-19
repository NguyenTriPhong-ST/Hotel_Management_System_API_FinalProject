using System;
using System.Collections.Generic;

namespace HotelTaskAPI.Models;

public partial class RoomBlock
{
    public int Id { get; set; }

    public int RoomId { get; set; }

    public int? HotelTaskId { get; set; }

    public DateTime StartAt { get; set; }

    public DateTime? EndAt { get; set; }

    public DateTime? ReleasedAt { get; set; }

    public DateTime? CancelledAt { get; set; }

    public string Reason { get; set; } = null!;

    public int CreatedByAccountId { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public byte[] RowVersion { get; set; } = null!;

    public virtual Account CreatedByAccount { get; set; } = null!;

    public virtual HotelTask? HotelTask { get; set; }

    public virtual Room Room { get; set; } = null!;
}
