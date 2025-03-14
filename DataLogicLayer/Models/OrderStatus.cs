using System;
using System.Collections.Generic;

namespace DataLogicLayer.Models;

public partial class OrderStatus
{
    public long Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
