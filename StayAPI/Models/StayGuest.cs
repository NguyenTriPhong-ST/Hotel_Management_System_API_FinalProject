using System;
using System.Collections.Generic;

namespace StayAPI.Models;

public partial class StayGuest
{
    public int StayId { get; set; }

    public int GuestId { get; set; }

    public bool IsMainGuest { get; set; }

    public DateTime AddedAt { get; set; }

    public virtual Guest Guest { get; set; } = null!;

    public virtual Stay Stay { get; set; } = null!;
}
