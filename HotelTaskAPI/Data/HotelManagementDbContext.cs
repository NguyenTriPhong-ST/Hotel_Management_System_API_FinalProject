using System;
using System.Collections.Generic;
using HotelTaskAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace HotelTaskAPI.Data;

public partial class HotelManagementDbContext : DbContext
{
    public HotelManagementDbContext(DbContextOptions<HotelManagementDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Account> Accounts { get; set; }

    public virtual DbSet<HotelTask> HotelTasks { get; set; }

    public virtual DbSet<Room> Rooms { get; set; }

    public virtual DbSet<RoomBlock> RoomBlocks { get; set; }

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

        modelBuilder.Entity<HotelTask>(entity =>
        {
            entity.HasIndex(e => new { e.Id, e.RoomId }, "AK_HotelTasks_Id_RoomId").IsUnique();

            entity.HasIndex(e => new { e.AssignedToAccountId, e.Status, e.CreatedAt }, "IX_HotelTasks_AssignedToAccountId_Status_CreatedAt");

            entity.HasIndex(e => e.CompletedByAccountId, "IX_HotelTasks_CompletedByAccountId");

            entity.HasIndex(e => e.CreatedByAccountId, "IX_HotelTasks_CreatedByAccountId");

            entity.HasIndex(e => new { e.RoomId, e.TaskType, e.Status }, "IX_HotelTasks_RoomId_TaskType_Status");

            entity.HasIndex(e => e.TaskCode, "IX_HotelTasks_TaskCode").IsUnique();

            entity.Property(e => e.CompletedAt).HasPrecision(3);
            entity.Property(e => e.CompletionNotes).HasMaxLength(2000);
            entity.Property(e => e.CreatedAt).HasPrecision(3);
            entity.Property(e => e.Description).HasMaxLength(2000);
            entity.Property(e => e.DueAt).HasPrecision(3);
            entity.Property(e => e.RowVersion)
                .IsRowVersion()
                .IsConcurrencyToken();
            entity.Property(e => e.StartedAt).HasPrecision(3);
            entity.Property(e => e.TaskCode)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.Title).HasMaxLength(200);
            entity.Property(e => e.UpdatedAt).HasPrecision(3);

            entity.HasOne(d => d.AssignedToAccount).WithMany(p => p.HotelTaskAssignedToAccounts).HasForeignKey(d => d.AssignedToAccountId);

            entity.HasOne(d => d.CompletedByAccount).WithMany(p => p.HotelTaskCompletedByAccounts).HasForeignKey(d => d.CompletedByAccountId);

            entity.HasOne(d => d.CreatedByAccount).WithMany(p => p.HotelTaskCreatedByAccounts)
                .HasForeignKey(d => d.CreatedByAccountId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.Room).WithMany(p => p.HotelTasks)
                .HasForeignKey(d => d.RoomId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<Room>(entity =>
        {
            entity.HasIndex(e => e.RoomCode, "IX_Rooms_RoomCode").IsUnique();

            entity.HasIndex(e => new { e.RoomTypeId, e.OperationalStatus, e.HousekeepingStatus }, "IX_Rooms_RoomTypeId_OperationalStatus_HousekeepingStatus");

            entity.Property(e => e.CreatedAt).HasPrecision(3);
            entity.Property(e => e.DeletedAt).HasPrecision(3);
            entity.Property(e => e.Description).HasMaxLength(1000);
            entity.Property(e => e.RoomCode)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.RowVersion)
                .IsRowVersion()
                .IsConcurrencyToken();
            entity.Property(e => e.UpdatedAt).HasPrecision(3);
        });

        modelBuilder.Entity<RoomBlock>(entity =>
        {
            entity.HasIndex(e => e.CreatedByAccountId, "IX_RoomBlocks_CreatedByAccountId");

            entity.HasIndex(e => new { e.HotelTaskId, e.RoomId }, "IX_RoomBlocks_HotelTaskId_RoomId");

            entity.HasIndex(e => new { e.RoomId, e.StartAt }, "IX_RoomBlocks_RoomId_StartAt");

            entity.Property(e => e.CancelledAt).HasPrecision(3);
            entity.Property(e => e.CreatedAt).HasPrecision(3);
            entity.Property(e => e.EndAt).HasPrecision(3);
            entity.Property(e => e.Reason).HasMaxLength(1000);
            entity.Property(e => e.ReleasedAt).HasPrecision(3);
            entity.Property(e => e.RowVersion)
                .IsRowVersion()
                .IsConcurrencyToken();
            entity.Property(e => e.StartAt).HasPrecision(3);
            entity.Property(e => e.UpdatedAt).HasPrecision(3);

            entity.HasOne(d => d.CreatedByAccount).WithMany(p => p.RoomBlocks)
                .HasForeignKey(d => d.CreatedByAccountId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.Room).WithMany(p => p.RoomBlocks)
                .HasForeignKey(d => d.RoomId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.HotelTask).WithMany(p => p.RoomBlocks)
                .HasPrincipalKey(p => new { p.Id, p.RoomId })
                .HasForeignKey(d => new { d.HotelTaskId, d.RoomId });
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
