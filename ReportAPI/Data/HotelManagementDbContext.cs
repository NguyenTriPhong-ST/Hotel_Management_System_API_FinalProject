using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using ReportAPI.Models;

namespace ReportAPI.Data;

public partial class HotelManagementDbContext : DbContext
{
    public HotelManagementDbContext(DbContextOptions<HotelManagementDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Account> Accounts { get; set; }

    public virtual DbSet<Guest> Guests { get; set; }

    public virtual DbSet<HotelTask> HotelTasks { get; set; }

    public virtual DbSet<Invoice> Invoices { get; set; }

    public virtual DbSet<Payment> Payments { get; set; }

    public virtual DbSet<Reservation> Reservations { get; set; }

    public virtual DbSet<Room> Rooms { get; set; }

    public virtual DbSet<RoomBlock> RoomBlocks { get; set; }

    public virtual DbSet<RoomType> RoomTypes { get; set; }

    public virtual DbSet<Stay> Stays { get; set; }

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

        modelBuilder.Entity<Guest>(entity =>
        {
            entity.HasIndex(e => new { e.DocumentType, e.DocumentCountryCode, e.DocumentNumber }, "IX_Guests_DocumentType_DocumentCountryCode_DocumentNumber")
                .IsUnique()
                .HasFilter("([DocumentNumber] IS NOT NULL)");

            entity.HasIndex(e => e.PhoneNumber, "IX_Guests_PhoneNumber");

            entity.Property(e => e.Address).HasMaxLength(500);
            entity.Property(e => e.CreatedAt).HasPrecision(3);
            entity.Property(e => e.DeletedAt).HasPrecision(3);
            entity.Property(e => e.DocumentCountryCode)
                .HasMaxLength(3)
                .IsUnicode(false);
            entity.Property(e => e.DocumentNumber).HasMaxLength(30);
            entity.Property(e => e.Email).HasMaxLength(254);
            entity.Property(e => e.FullName).HasMaxLength(150);
            entity.Property(e => e.Notes).HasMaxLength(1000);
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.RowVersion)
                .IsRowVersion()
                .IsConcurrencyToken();
            entity.Property(e => e.UpdatedAt).HasPrecision(3);
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

        modelBuilder.Entity<Invoice>(entity =>
        {
            entity.HasIndex(e => e.CreatedByAccountId, "IX_Invoices_CreatedByAccountId");

            entity.HasIndex(e => e.FinalizedByAccountId, "IX_Invoices_FinalizedByAccountId");

            entity.HasIndex(e => e.InvoiceCode, "IX_Invoices_InvoiceCode").IsUnique();

            entity.HasIndex(e => e.StayId, "IX_Invoices_StayId").IsUnique();

            entity.Property(e => e.CreatedAt).HasPrecision(3);
            entity.Property(e => e.FinalizedAt).HasPrecision(3);
            entity.Property(e => e.GuestDocumentSnapshot).HasMaxLength(30);
            entity.Property(e => e.GuestNameSnapshot).HasMaxLength(150);
            entity.Property(e => e.InvoiceCode)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.Notes).HasMaxLength(1000);
            entity.Property(e => e.RoomCodeSnapshot)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.RowVersion)
                .IsRowVersion()
                .IsConcurrencyToken();
            entity.Property(e => e.TotalAmount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.UpdatedAt).HasPrecision(3);

            entity.HasOne(d => d.CreatedByAccount).WithMany(p => p.InvoiceCreatedByAccounts)
                .HasForeignKey(d => d.CreatedByAccountId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.FinalizedByAccount).WithMany(p => p.InvoiceFinalizedByAccounts).HasForeignKey(d => d.FinalizedByAccountId);

            entity.HasOne(d => d.Stay).WithOne(p => p.Invoice)
                .HasForeignKey<Invoice>(d => d.StayId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasIndex(e => e.IdempotencyKey, "IX_Payments_IdempotencyKey").IsUnique();

            entity.HasIndex(e => e.InvoiceId, "IX_Payments_InvoiceId");

            entity.HasIndex(e => new { e.PaidAt, e.Method }, "IX_Payments_PaidAt_Method");

            entity.HasIndex(e => e.ReceiptCode, "IX_Payments_ReceiptCode").IsUnique();

            entity.HasIndex(e => e.RecordedByAccountId, "IX_Payments_RecordedByAccountId");

            entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.CreatedAt).HasPrecision(3);
            entity.Property(e => e.Notes).HasMaxLength(500);
            entity.Property(e => e.PaidAt).HasPrecision(3);
            entity.Property(e => e.ReceiptCode)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.ReferenceNumber).HasMaxLength(100);

            entity.HasOne(d => d.Invoice).WithMany(p => p.Payments)
                .HasForeignKey(d => d.InvoiceId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.RecordedByAccount).WithMany(p => p.Payments)
                .HasForeignKey(d => d.RecordedByAccountId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<Reservation>(entity =>
        {
            entity.HasIndex(e => new { e.Id, e.RoomId }, "AK_Reservations_Id_RoomId").IsUnique();

            entity.HasIndex(e => e.CancelledByAccountId, "IX_Reservations_CancelledByAccountId");

            entity.HasIndex(e => e.CreatedByAccountId, "IX_Reservations_CreatedByAccountId");

            entity.HasIndex(e => new { e.GuestId, e.CheckInDate }, "IX_Reservations_GuestId_CheckInDate");

            entity.HasIndex(e => e.ReservationCode, "IX_Reservations_ReservationCode").IsUnique();

            entity.HasIndex(e => new { e.RoomId, e.Status, e.CheckInDate, e.CheckOutDate }, "IX_Reservations_RoomId_Status_CheckInDate_CheckOutDate");

            entity.HasIndex(e => e.RoomTypeId, "IX_Reservations_RoomTypeId");

            entity.Property(e => e.CancellationReason).HasMaxLength(500);
            entity.Property(e => e.CancelledAt).HasPrecision(3);
            entity.Property(e => e.CreatedAt).HasPrecision(3);
            entity.Property(e => e.NightlyRate).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Notes).HasMaxLength(1000);
            entity.Property(e => e.ReservationCode)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.RowVersion)
                .IsRowVersion()
                .IsConcurrencyToken();
            entity.Property(e => e.UpdatedAt).HasPrecision(3);

            entity.HasOne(d => d.CancelledByAccount).WithMany(p => p.ReservationCancelledByAccounts).HasForeignKey(d => d.CancelledByAccountId);

            entity.HasOne(d => d.CreatedByAccount).WithMany(p => p.ReservationCreatedByAccounts)
                .HasForeignKey(d => d.CreatedByAccountId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.Guest).WithMany(p => p.Reservations)
                .HasForeignKey(d => d.GuestId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.Room).WithMany(p => p.Reservations)
                .HasForeignKey(d => d.RoomId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.RoomType).WithMany(p => p.Reservations)
                .HasForeignKey(d => d.RoomTypeId)
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

            entity.HasOne(d => d.RoomType).WithMany(p => p.Rooms)
                .HasForeignKey(d => d.RoomTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull);
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

        modelBuilder.Entity<RoomType>(entity =>
        {
            entity.HasIndex(e => e.BasePricePerNight, "IX_RoomTypes_BasePricePerNight");

            entity.HasIndex(e => e.Code, "IX_RoomTypes_Code").IsUnique();

            entity.Property(e => e.BasePricePerNight).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Code)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.CreatedAt).HasPrecision(3);
            entity.Property(e => e.DeletedAt).HasPrecision(3);
            entity.Property(e => e.Description).HasMaxLength(1000);
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.RowVersion)
                .IsRowVersion()
                .IsConcurrencyToken();
            entity.Property(e => e.UpdatedAt).HasPrecision(3);
        });

        modelBuilder.Entity<Stay>(entity =>
        {
            entity.HasIndex(e => e.CheckedInAt, "IX_Stays_CheckedInAt");

            entity.HasIndex(e => e.CheckedInByAccountId, "IX_Stays_CheckedInByAccountId");

            entity.HasIndex(e => e.CheckedOutByAccountId, "IX_Stays_CheckedOutByAccountId");

            entity.HasIndex(e => e.ReservationId, "IX_Stays_ReservationId").IsUnique();

            entity.HasIndex(e => new { e.ReservationId, e.RoomId }, "IX_Stays_ReservationId_RoomId").IsUnique();

            entity.HasIndex(e => e.StayCode, "IX_Stays_StayCode").IsUnique();

            entity.HasIndex(e => e.RoomId, "UX_Stays_RoomId_Active")
                .IsUnique()
                .HasFilter("([CheckedOutAt] IS NULL)");

            entity.Property(e => e.CheckedInAt).HasPrecision(3);
            entity.Property(e => e.CheckedOutAt).HasPrecision(3);
            entity.Property(e => e.CreatedAt).HasPrecision(3);
            entity.Property(e => e.Notes).HasMaxLength(1000);
            entity.Property(e => e.RowVersion)
                .IsRowVersion()
                .IsConcurrencyToken();
            entity.Property(e => e.StayCode)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedAt).HasPrecision(3);

            entity.HasOne(d => d.CheckedInByAccount).WithMany(p => p.StayCheckedInByAccounts)
                .HasForeignKey(d => d.CheckedInByAccountId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.CheckedOutByAccount).WithMany(p => p.StayCheckedOutByAccounts).HasForeignKey(d => d.CheckedOutByAccountId);

            entity.HasOne(d => d.Room).WithOne(p => p.Stay)
                .HasForeignKey<Stay>(d => d.RoomId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.Reservation).WithOne(p => p.Stay)
                .HasPrincipalKey<Reservation>(p => new { p.Id, p.RoomId })
                .HasForeignKey<Stay>(d => new { d.ReservationId, d.RoomId })
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
