using System;
using System.Collections.Generic;

namespace DataLogicLayer.Models;

public partial class Taxis
{
    public long Id { get; set; }

    public string Name { get; set; } = null!;

    public bool Taxtype { get; set; }

    public decimal Amount { get; set; }

    public bool Isenabled { get; set; }

    public bool? DefaultTax { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public long CreatedBy { get; set; }

    public long? UpdatedBy { get; set; }

    public bool Isdeleted { get; set; }

    public virtual User CreatedByNavigation { get; set; } = null!;

    public virtual ICollection<Item> Items { get; set; } = new List<Item>();

    public virtual User? UpdatedByNavigation { get; set; }
}
