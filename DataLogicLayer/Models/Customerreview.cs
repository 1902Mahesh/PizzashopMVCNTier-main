using System;
using System.Collections.Generic;

namespace DataLogicLayer.Models;

public partial class Customerreview
{
    public long Id { get; set; }

    public long Customerid { get; set; }

    public int Foodrating { get; set; }

    public int Servicerating { get; set; }

    public int Ambiencerating { get; set; }

    public int AverageRating { get; set; }

    public string? Review { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public long CreatedBy { get; set; }

    public long? UpdatedBy { get; set; }

    public bool Isdeleted { get; set; }

    public virtual User CreatedByNavigation { get; set; } = null!;

    public virtual Customer Customer { get; set; } = null!;

    public virtual User? UpdatedByNavigation { get; set; }
}
