using System;
using System.Collections.Generic;

namespace DataLogicLayer.Models;

public partial class Unit
{
    public long UnitId { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Item> Items { get; set; } = new List<Item>();

    public virtual ICollection<Modifieritem> Modifieritems { get; set; } = new List<Modifieritem>();
}
