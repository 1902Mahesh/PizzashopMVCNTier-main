using System;
using System.Collections.Generic;

namespace DataLogicLayer.Models;

public partial class Kot
{
    public long Id { get; set; }

    public long Orderid { get; set; }

    public bool Status { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public long CreatedBy { get; set; }

    public long? UpdatedBy { get; set; }

    public bool Isdeleted { get; set; }

    public virtual User CreatedByNavigation { get; set; } = null!;

    public virtual Order Order { get; set; } = null!;

    public virtual User? UpdatedByNavigation { get; set; }
}
