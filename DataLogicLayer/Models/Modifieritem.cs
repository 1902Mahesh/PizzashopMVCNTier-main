using System;
using System.Collections.Generic;

namespace DataLogicLayer.Models;

public partial class Modifieritem
{
    public long Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public long ModifierGroupId { get; set; }

    public decimal? Rate { get; set; }

    public long Unitid { get; set; }

    public int Quantity { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public long CreatedBy { get; set; }

    public long? UpdatedBy { get; set; }

    public bool Isdeleted { get; set; }

    public virtual User CreatedByNavigation { get; set; } = null!;

    public virtual Modifiergroup ModifierGroup { get; set; } = null!;

    public virtual ICollection<Modifiergroupitemmap> Modifiergroupitemmaps { get; set; } = new List<Modifiergroupitemmap>();

    public virtual ICollection<Orderitemsmodifier> Orderitemsmodifiers { get; set; } = new List<Orderitemsmodifier>();

    public virtual Unit Unit { get; set; } = null!;

    public virtual User? UpdatedByNavigation { get; set; }
}
