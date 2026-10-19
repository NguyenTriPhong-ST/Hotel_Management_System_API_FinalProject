using System;
using System.Collections.Generic;

namespace ReservationAPI.Models;

public partial class Room
{
    public int Id { get; set; }

    public string RoomCode { get; set; } = null!;

    public int RoomTypeId { get; set; }

    public short FloorNumber { get; set; }

    public string? Description { get; set; }

    public byte OperationalStatus { get; set; }

    public byte HousekeepingStatus { get; set; }

    public DateTime? DeletedAt { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public byte[] RowVersion { get; set; } = null!;

    public virtual ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();

    public virtual ICollection<RoomBlock> RoomBlocks { get; set; } = new List<RoomBlock>();

    public virtual RoomType RoomType { get; set; } = null!;

    public virtual Stay? Stay { get; set; }
}
