using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using RoomAPI.Models;

namespace RoomAPI.Data;

public partial class HotelManagementDbContext : DbContext
{
    public HotelManagementDbContext(DbContextOptions<HotelManagementDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Room> Rooms { get; set; }

    public virtual DbSet<RoomType> RoomTypes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
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

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
