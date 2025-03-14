using System;
using System.Collections.Generic;

namespace DataLogicLayer.Models;

public partial class Table
{
    public long Id { get; set; }

    public string Name { get; set; } = null!;

    public long Sectionid { get; set; }

    public int Capacity { get; set; }

    public long? TableStatus { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public long CreatedBy { get; set; }

    public long? UpdatedBy { get; set; }

    public bool Isdeleted { get; set; }

    public bool IsOccupied { get; set; }

    public virtual User CreatedByNavigation { get; set; } = null!;

    public virtual ICollection<Orderitemsmodifier> Orderitemsmodifiers { get; set; } = new List<Orderitemsmodifier>();

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual Section Section { get; set; } = null!;

    public virtual TableStatus? TableStatusNavigation { get; set; }

    public virtual User? UpdatedByNavigation { get; set; }
}
