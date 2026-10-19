using System;
using System.Collections.Generic;
using GuestAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace GuestAPI.Data;

public partial class HotelManagementDbContext : DbContext
{
    public HotelManagementDbContext(DbContextOptions<HotelManagementDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Guest> Guests { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
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

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
