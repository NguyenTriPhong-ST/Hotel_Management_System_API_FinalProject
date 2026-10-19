using System;
using System.Collections.Generic;

namespace StayAPI.Models;

public partial class Payment
{
    public int Id { get; set; }

    public string ReceiptCode { get; set; } = null!;

    public int InvoiceId { get; set; }

    public Guid IdempotencyKey { get; set; }

    public decimal Amount { get; set; }

    public byte Method { get; set; }

    public DateTime PaidAt { get; set; }

    public string? ReferenceNumber { get; set; }

    public int RecordedByAccountId { get; set; }

    public DateTime CreatedAt { get; set; }

    public string? Notes { get; set; }

    public virtual Invoice Invoice { get; set; } = null!;

    public virtual Account RecordedByAccount { get; set; } = null!;
}
