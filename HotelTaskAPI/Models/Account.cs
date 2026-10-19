using System;
using System.Collections.Generic;

namespace HotelTaskAPI.Models;

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

    public virtual ICollection<HotelTask> HotelTaskAssignedToAccounts { get; set; } = new List<HotelTask>();

    public virtual ICollection<HotelTask> HotelTaskCompletedByAccounts { get; set; } = new List<HotelTask>();

    public virtual ICollection<HotelTask> HotelTaskCreatedByAccounts { get; set; } = new List<HotelTask>();

    public virtual ICollection<RoomBlock> RoomBlocks { get; set; } = new List<RoomBlock>();
}
