using System;
using System.Collections.Generic;

namespace ServiceUsageAPI.Models;

public partial class Stay
{
    public int Id { get; set; }

    public string StayCode { get; set; } = null!;

    public int ReservationId { get; set; }

    public int RoomId { get; set; }

    public DateTime CheckedInAt { get; set; }

    public DateTime? CheckedOutAt { get; set; }

    public int CheckedInByAccountId { get; set; }

    public int? CheckedOutByAccountId { get; set; }

    public string? Notes { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public byte[] RowVersion { get; set; } = null!;

    public virtual Account CheckedInByAccount { get; set; } = null!;

    public virtual Account? CheckedOutByAccount { get; set; }

    public virtual Invoice? Invoice { get; set; }

    public virtual ICollection<ServiceUsage> ServiceUsages { get; set; } = new List<ServiceUsage>();
}
