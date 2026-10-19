using System;
using System.Collections.Generic;

namespace AccountAPI.Models;

public partial class AuthSession
{
    public Guid Id { get; set; }

    public int AccountId { get; set; }

    public DateTime IssuedAt { get; set; }

    public DateTime ExpiresAt { get; set; }

    public DateTime? RevokedAt { get; set; }

    public virtual Account Account { get; set; } = null!;
}
