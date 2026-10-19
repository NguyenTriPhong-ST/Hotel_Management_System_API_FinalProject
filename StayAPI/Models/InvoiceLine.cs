using System;
using System.Collections.Generic;

namespace StayAPI.Models;

public partial class InvoiceLine
{
    public int Id { get; set; }

    public int InvoiceId { get; set; }

    public byte LineType { get; set; }

    public int? ServiceUsageId { get; set; }

    public string Description { get; set; } = null!;

    public string Unit { get; set; } = null!;

    public decimal Quantity { get; set; }

    public decimal UnitPrice { get; set; }

    public decimal? Amount { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public byte[] RowVersion { get; set; } = null!;

    public virtual Invoice Invoice { get; set; } = null!;

    public virtual ServiceUsage? ServiceUsage { get; set; }
}
