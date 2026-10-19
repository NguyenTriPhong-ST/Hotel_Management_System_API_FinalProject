using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using RoomTypeAPI.Models;

namespace RoomTypeAPI.Data;

public partial class HotelManagementDbContext : DbContext
{
    public HotelManagementDbContext(DbContextOptions<HotelManagementDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<RoomType> RoomTypes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
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
