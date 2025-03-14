using System;
using System.Collections.Generic;

namespace DataLogicLayer.Models;

public partial class Invoice
{
    public long Id { get; set; }

    public string InvoiceNo { get; set; } = null!;

    public long Orderid { get; set; }

    public long Customerid { get; set; }

    public decimal SubTotal { get; set; }

    public decimal Cgst { get; set; }

    public decimal Sgst { get; set; }

    public decimal Gst { get; set; }

    public decimal Other { get; set; }

    public decimal TotalAmount { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public long CreatedBy { get; set; }

    public long? UpdatedBy { get; set; }

    public virtual User CreatedByNavigation { get; set; } = null!;

    public virtual Customer Customer { get; set; } = null!;

    public virtual Order Order { get; set; } = null!;

    public virtual User? UpdatedByNavigation { get; set; }
}
