using System;
using System.Collections.Generic;

namespace StayAPI.Models;

public partial class Guest
{
    public int Id { get; set; }

    public string FullName { get; set; } = null!;

    public DateOnly? DateOfBirth { get; set; }

    public string? PhoneNumber { get; set; }

    public string? Email { get; set; }

    public byte? DocumentType { get; set; }

    public string? DocumentNumber { get; set; }

    public string? DocumentCountryCode { get; set; }

    public string? Address { get; set; }

    public string? Notes { get; set; }

    public DateTime? DeletedAt { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public byte[] RowVersion { get; set; } = null!;

    public virtual ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();

    public virtual ICollection<StayGuest> StayGuests { get; set; } = new List<StayGuest>();
}
