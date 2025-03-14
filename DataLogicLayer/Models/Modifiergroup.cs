using System;
using System.Collections.Generic;

namespace DataLogicLayer.Models;

public partial class Modifiergroup
{
    public long Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public long CreatedBy { get; set; }

    public long? UpdatedBy { get; set; }

    public bool Isdeleted { get; set; }

    public virtual User CreatedByNavigation { get; set; } = null!;

    public virtual ICollection<Itemmodifiergroup> Itemmodifiergroups { get; set; } = new List<Itemmodifiergroup>();

    public virtual ICollection<Modifiergroupitemmap> Modifiergroupitemmaps { get; set; } = new List<Modifiergroupitemmap>();

    public virtual ICollection<Modifieritem> Modifieritems { get; set; } = new List<Modifieritem>();

    public virtual User? UpdatedByNavigation { get; set; }
}
