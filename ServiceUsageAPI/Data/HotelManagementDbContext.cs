using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using ServiceUsageAPI.Models;

namespace ServiceUsageAPI.Data;

public partial class HotelManagementDbContext : DbContext
{
    public HotelManagementDbContext(DbContextOptions<HotelManagementDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Account> Accounts { get; set; }

    public virtual DbSet<HotelService> HotelServices { get; set; }

    public virtual DbSet<Invoice> Invoices { get; set; }

    public virtual DbSet<InvoiceLine> InvoiceLines { get; set; }

    public virtual DbSet<ServiceUsage> ServiceUsages { get; set; }

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

        modelBuilder.Entity<HotelService>(entity =>
        {
            entity.HasIndex(e => new { e.IsActive, e.CurrentPrice }, "IX_HotelServices_IsActive_CurrentPrice");

            entity.HasIndex(e => e.ServiceCode, "IX_HotelServices_ServiceCode").IsUnique();

            entity.Property(e => e.CreatedAt).HasPrecision(3);
            entity.Property(e => e.CurrentPrice).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Description).HasMaxLength(1000);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Name).HasMaxLength(150);
            entity.Property(e => e.RowVersion)
                .IsRowVersion()
                .IsConcurrencyToken();
            entity.Property(e => e.ServiceCode)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.Unit).HasMaxLength(30);
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

            entity.HasOne(d => d.HotelService).WithMany(p => p.ServiceUsages)
                .HasForeignKey(d => d.HotelServiceId)
                .OnDelete(DeleteBehavior.ClientSetNull);

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
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
