using System;
using System.Collections.Generic;
using AccountAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace AccountAPI.Data;

public partial class HotelManagementDbContext : DbContext
{
    public HotelManagementDbContext(DbContextOptions<HotelManagementDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Account> Accounts { get; set; }

    public virtual DbSet<AuthSession> AuthSessions { get; set; }

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

        modelBuilder.Entity<AuthSession>(entity =>
        {
            entity.HasIndex(e => new { e.AccountId, e.RevokedAt, e.ExpiresAt }, "IX_AuthSessions_AccountId_RevokedAt_ExpiresAt");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.ExpiresAt).HasPrecision(3);
            entity.Property(e => e.IssuedAt).HasPrecision(3);
            entity.Property(e => e.RevokedAt).HasPrecision(3);

            entity.HasOne(d => d.Account).WithMany(p => p.AuthSessions)
                .HasForeignKey(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
