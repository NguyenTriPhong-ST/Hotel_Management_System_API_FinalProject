using System;
using System.Collections.Generic;
using AuditLogAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace AuditLogAPI.Data;

public partial class HotelManagementDbContext : DbContext
{
    public HotelManagementDbContext(DbContextOptions<HotelManagementDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Account> Accounts { get; set; }

    public virtual DbSet<AuditLog> AuditLogs { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasIndex(e => e.NormalizedUsername, "IX_Accounts_NormalizedUsername").IsUnique();

            entity.HasIndex(e => new { e.Role, e.IsActive }, "IX_Accounts_Role_IsActive");

            entity.Property(e => e.CreatedAt).HasPrecision(3);
            entity.Property(e => e.Email).HasMaxLength(254);
            entity.Property(e => e.FullName).HasMaxLength(150);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.NormalizedUsername)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.PasswordHash).HasMaxLength(500);
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.RowVersion)
                .IsRowVersion()
                .IsConcurrencyToken();
            entity.Property(e => e.UpdatedAt).HasPrecision(3);
            entity.Property(e => e.Username)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<AuditLog>(entity =>
        {
            entity.HasIndex(e => new { e.ActorAccountId, e.OccurredAt }, "IX_AuditLogs_ActorAccountId_OccurredAt");

            entity.HasIndex(e => new { e.EntityType, e.EntityId, e.OccurredAt }, "IX_AuditLogs_EntityType_EntityId_OccurredAt");

            entity.HasIndex(e => e.OccurredAt, "IX_AuditLogs_OccurredAt");

            entity.Property(e => e.Action)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ActorUsernameSnapshot).HasMaxLength(50);
            entity.Property(e => e.CorrelationId)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.EntityCodeSnapshot).HasMaxLength(50);
            entity.Property(e => e.EntityId).HasMaxLength(64);
            entity.Property(e => e.EntityType)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.OccurredAt).HasPrecision(3);
            entity.Property(e => e.Summary).HasMaxLength(1000);

            entity.HasOne(d => d.ActorAccount).WithMany(p => p.AuditLogs).HasForeignKey(d => d.ActorAccountId);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
