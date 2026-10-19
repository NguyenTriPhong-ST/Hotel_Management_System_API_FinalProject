using System;
using System.Collections.Generic;

namespace StayAPI.Models;

public partial class ServiceUsage
{
    public int Id { get; set; }

    public int StayId { get; set; }

    public int HotelServiceId { get; set; }

    public string ServiceNameSnapshot { get; set; } = null!;

    public string UnitSnapshot { get; set; } = null!;

    public decimal Quantity { get; set; }

    public decimal UnitPrice { get; set; }

    public DateTime UsedAt { get; set; }

    public int RecordedByAccountId { get; set; }

    public string? Notes { get; set; }

    public DateTime? DeletedAt { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public byte[] RowVersion { get; set; } = null!;

    public virtual InvoiceLine? InvoiceLine { get; set; }

    public virtual Account RecordedByAccount { get; set; } = null!;

    public virtual Stay Stay { get; set; } = null!;
}
