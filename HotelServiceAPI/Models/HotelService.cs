using System;
using System.Collections.Generic;

namespace HotelServiceAPI.Models;

public partial class HotelService
{
    public int Id { get; set; }

    public string ServiceCode { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public string Unit { get; set; } = null!;

    public decimal CurrentPrice { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public byte[] RowVersion { get; set; } = null!;
}
