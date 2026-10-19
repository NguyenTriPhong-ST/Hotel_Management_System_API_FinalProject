using System;
using System.Collections.Generic;

namespace AuditLogAPI.Models;

public partial class AuditLog
{
    public long Id { get; set; }

    public int? ActorAccountId { get; set; }

    public string? ActorUsernameSnapshot { get; set; }

    public string Action { get; set; } = null!;

    public string EntityType { get; set; } = null!;

    public string EntityId { get; set; } = null!;

    public string? EntityCodeSnapshot { get; set; }

    public DateTime OccurredAt { get; set; }

    public string Summary { get; set; } = null!;

    public string? OldValuesJson { get; set; }

    public string? NewValuesJson { get; set; }

    public string? CorrelationId { get; set; }

    public virtual Account? ActorAccount { get; set; }
}
