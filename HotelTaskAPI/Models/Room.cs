using System;
using System.Collections.Generic;

namespace HotelTaskAPI.Models;

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

    public virtual ICollection<HotelTask> HotelTasks { get; set; } = new List<HotelTask>();

    public virtual ICollection<RoomBlock> RoomBlocks { get; set; } = new List<RoomBlock>();
}
