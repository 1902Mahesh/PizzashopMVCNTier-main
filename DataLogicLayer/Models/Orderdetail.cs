using System;
using System.Collections.Generic;

namespace DataLogicLayer.Models;

public partial class Orderdetail
{
    public long Id { get; set; }

    public long Orderid { get; set; }

    public long Itemid { get; set; }

    public int Quantity { get; set; }

    public decimal? Price { get; set; }

    public string? Instruction { get; set; }

    public virtual Item Item { get; set; } = null!;

    public virtual Order Order { get; set; } = null!;

    public virtual ICollection<Orderitemsmodifier> Orderitemsmodifiers { get; set; } = new List<Orderitemsmodifier>();
}
