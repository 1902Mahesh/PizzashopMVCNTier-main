using System;
using System.Collections.Generic;

namespace DataLogicLayer.Models;

public partial class Paymentmethod
{
    public long Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();
}
