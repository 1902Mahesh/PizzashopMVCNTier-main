using System;
using System.Collections.Generic;

namespace DataLogicLayer.Models;

public partial class Favoriteitem
{
    public long Id { get; set; }

    public long Orderid { get; set; }

    public long Customerid { get; set; }

    public bool Isfavorite { get; set; }

    public virtual Customer Customer { get; set; } = null!;

    public virtual Order Order { get; set; } = null!;
}
