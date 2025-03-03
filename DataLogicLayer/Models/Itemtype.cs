using System;
using System.Collections.Generic;

namespace DataLogicLayer.Models;

public partial class Itemtype
{
    public long Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Imgurl { get; set; }

    public virtual ICollection<Item> Items { get; set; } = new List<Item>();
}
