using System;
using System.Collections.Generic;

namespace HotelTaskAPI.Models;

public partial class HotelTask
{
    public int Id { get; set; }

    public string TaskCode { get; set; } = null!;

    public int RoomId { get; set; }

    public byte TaskType { get; set; }

    public byte? HousekeepingKind { get; set; }

    public string Title { get; set; } = null!;

    public string Description { get; set; } = null!;

    public byte Priority { get; set; }

    public byte Status { get; set; }

    public int? AssignedToAccountId { get; set; }

    public int CreatedByAccountId { get; set; }

    public DateTime? DueAt { get; set; }

    public DateTime? StartedAt { get; set; }

    public DateTime? CompletedAt { get; set; }

    public int? CompletedByAccountId { get; set; }

    public string? CompletionNotes { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public byte[] RowVersion { get; set; } = null!;

    public virtual Account? AssignedToAccount { get; set; }

    public virtual Account? CompletedByAccount { get; set; }

    public virtual Account CreatedByAccount { get; set; } = null!;

    public virtual Room Room { get; set; } = null!;

    public virtual ICollection<RoomBlock> RoomBlocks { get; set; } = new List<RoomBlock>();
}
