using System;
using System.Collections.Generic;

namespace DataLogicLayer.Models;

public partial class Payment
{
    public long Id { get; set; }

    public long Orderid { get; set; }

    public long Customerid { get; set; }

    public long Methods { get; set; }

    public long Status { get; set; }

    public DateTime Paymentdate { get; set; }

    public decimal Amount { get; set; }

    public virtual Customer Customer { get; set; } = null!;

    public virtual Paymentmethod MethodsNavigation { get; set; } = null!;

    public virtual Order Order { get; set; } = null!;

    public virtual Paymentstatus StatusNavigation { get; set; } = null!;
}
