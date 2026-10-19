using System;
using System.Collections.Generic;

namespace ReservationAPI.Models;

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

    public virtual ICollection<Reservation> ReservationCancelledByAccounts { get; set; } = new List<Reservation>();

    public virtual ICollection<Reservation> ReservationCreatedByAccounts { get; set; } = new List<Reservation>();

    public virtual ICollection<RoomBlock> RoomBlocks { get; set; } = new List<RoomBlock>();

    public virtual ICollection<Stay> StayCheckedInByAccounts { get; set; } = new List<Stay>();

    public virtual ICollection<Stay> StayCheckedOutByAccounts { get; set; } = new List<Stay>();
}
