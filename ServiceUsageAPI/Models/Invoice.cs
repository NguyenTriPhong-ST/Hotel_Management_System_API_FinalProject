using System;
using System.Collections.Generic;

namespace ServiceUsageAPI.Models;

public partial class Invoice
{
    public int Id { get; set; }

    public string InvoiceCode { get; set; } = null!;

    public int StayId { get; set; }

    public byte Status { get; set; }

    public DateOnly BillingFromDate { get; set; }

    public DateOnly BillingToDate { get; set; }

    public string GuestNameSnapshot { get; set; } = null!;

    public string? GuestDocumentSnapshot { get; set; }

    public string RoomCodeSnapshot { get; set; } = null!;

    public decimal TotalAmount { get; set; }

    public int CreatedByAccountId { get; set; }

    public DateTime? FinalizedAt { get; set; }

    public int? FinalizedByAccountId { get; set; }

    public string? Notes { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public byte[] RowVersion { get; set; } = null!;

    public virtual Account CreatedByAccount { get; set; } = null!;

    public virtual Account? FinalizedByAccount { get; set; }

    public virtual InvoiceLine? InvoiceLine { get; set; }

    public virtual Stay Stay { get; set; } = null!;
}
