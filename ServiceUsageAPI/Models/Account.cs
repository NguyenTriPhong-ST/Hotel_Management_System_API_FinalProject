using System;
using System.Collections.Generic;

namespace ServiceUsageAPI.Models;

public partial class Account
{
    public int Id { get; set; }

    public string Username { get; set; } = null!;

    public string NormalizedUsername { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public string FullName { get; set; } = null!;

    public string? PhoneNumber { get; set; }

    public string? Email { get; set; }

    public byte Role { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public byte[] RowVersion { get; set; } = null!;

    public virtual ICollection<Invoice> InvoiceCreatedByAccounts { get; set; } = new List<Invoice>();

    public virtual ICollection<Invoice> InvoiceFinalizedByAccounts { get; set; } = new List<Invoice>();

    public virtual ICollection<ServiceUsage> ServiceUsages { get; set; } = new List<ServiceUsage>();

    public virtual ICollection<Stay> StayCheckedInByAccounts { get; set; } = new List<Stay>();

    public virtual ICollection<Stay> StayCheckedOutByAccounts { get; set; } = new List<Stay>();
}
