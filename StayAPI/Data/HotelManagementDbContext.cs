using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using StayAPI.Models;

namespace StayAPI.Data;

public partial class HotelManagementDbContext : DbContext
{
    public HotelManagementDbContext(DbContextOptions<HotelManagementDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Account> Accounts { get; set; }

    public virtual DbSet<Guest> Guests { get; set; }

    public virtual DbSet<Invoice> Invoices { get; set; }

    public virtual DbSet<InvoiceLine> InvoiceLines { get; set; }

    public virtual DbSet<Payment> Payments { get; set; }

    public virtual DbSet<Reservation> Reservations { get; set; }

    public virtual DbSet<Room> Rooms { get; set; }

    public virtual DbSet<ServiceUsage> ServiceUsages { get; set; }

    public virtual DbSet<Stay> Stays { get; set; }

    public virtual DbSet<StayGuest> StayGuests { get; set; }

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

        modelBuilder.Entity<InvoiceLine>(entity =>
        {
            entity.HasIndex(e => e.InvoiceId, "UX_InvoiceLines_RoomCharge")
                .IsUnique()
                .HasFilter("([LineType]=(1))");

            entity.HasIndex(e => e.ServiceUsageId, "UX_InvoiceLines_ServiceUsage")
                .IsUnique()
                .HasFilter("([ServiceUsageId] IS NOT NULL)");

            entity.Property(e => e.Amount)
                .HasComputedColumnSql("(round([Quantity]*[UnitPrice],(2)))", true)
                .HasColumnType("decimal(29, 4)");
            entity.Property(e => e.CreatedAt).HasPrecision(3);
            entity.Property(e => e.Description).HasMaxLength(250);
            entity.Property(e => e.Quantity).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.RowVersion)
                .IsRowVersion()
                .IsConcurrencyToken();
            entity.Property(e => e.Unit).HasMaxLength(30);
            entity.Property(e => e.UnitPrice).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.UpdatedAt).HasPrecision(3);

            entity.HasOne(d => d.Invoice).WithOne(p => p.InvoiceLine)
                .HasForeignKey<InvoiceLine>(d => d.InvoiceId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.ServiceUsage).WithOne(p => p.InvoiceLine).HasForeignKey<InvoiceLine>(d => d.ServiceUsageId);
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

        modelBuilder.Entity<ServiceUsage>(entity =>
        {
            entity.HasIndex(e => new { e.HotelServiceId, e.UsedAt }, "IX_ServiceUsages_HotelServiceId_UsedAt");

            entity.HasIndex(e => e.RecordedByAccountId, "IX_ServiceUsages_RecordedByAccountId");

            entity.HasIndex(e => new { e.StayId, e.UsedAt }, "IX_ServiceUsages_StayId_UsedAt");

            entity.Property(e => e.CreatedAt).HasPrecision(3);
            entity.Property(e => e.DeletedAt).HasPrecision(3);
            entity.Property(e => e.Notes).HasMaxLength(1000);
            entity.Property(e => e.Quantity).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.RowVersion)
                .IsRowVersion()
                .IsConcurrencyToken();
            entity.Property(e => e.ServiceNameSnapshot).HasMaxLength(150);
            entity.Property(e => e.UnitPrice).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.UnitSnapshot).HasMaxLength(30);
            entity.Property(e => e.UpdatedAt).HasPrecision(3);
            entity.Property(e => e.UsedAt).HasPrecision(3);

            entity.HasOne(d => d.RecordedByAccount).WithMany(p => p.ServiceUsages)
                .HasForeignKey(d => d.RecordedByAccountId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.Stay).WithMany(p => p.ServiceUsages)
                .HasForeignKey(d => d.StayId)
                .OnDelete(DeleteBehavior.ClientSetNull);
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

        modelBuilder.Entity<StayGuest>(entity =>
        {
            entity.HasKey(e => new { e.StayId, e.GuestId });

            entity.HasIndex(e => new { e.GuestId, e.StayId }, "IX_StayGuests_GuestId_StayId");

            entity.HasIndex(e => e.StayId, "UX_StayGuests_MainGuest")
                .IsUnique()
                .HasFilter("([IsMainGuest]=(1))");

            entity.Property(e => e.AddedAt).HasPrecision(3);

            entity.HasOne(d => d.Guest).WithMany(p => p.StayGuests)
                .HasForeignKey(d => d.GuestId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.Stay).WithOne(p => p.StayGuest)
                .HasForeignKey<StayGuest>(d => d.StayId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
