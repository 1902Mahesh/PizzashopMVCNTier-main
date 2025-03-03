using System;
using System.Collections.Generic;

namespace DataLogicLayer.Models;

public partial class Order
{
    public long Id { get; set; }

    public long Customerid { get; set; }

    public long Tableid { get; set; }

    public DateTime Orderdate { get; set; }

    public long Orderstatus { get; set; }

    public string? Instruction { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public long CreatedBy { get; set; }

    public long? UpdatedBy { get; set; }

    public bool Isdeleted { get; set; }

    public virtual ICollection<Booktable> Booktables { get; set; } = new List<Booktable>();

    public virtual User CreatedByNavigation { get; set; } = null!;

    public virtual Customer Customer { get; set; } = null!;

    public virtual ICollection<Favoriteitem> Favoriteitems { get; set; } = new List<Favoriteitem>();

    public virtual ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();

    public virtual ICollection<Kot> Kots { get; set; } = new List<Kot>();

    public virtual ICollection<Orderdetail> Orderdetails { get; set; } = new List<Orderdetail>();

    public virtual OrderStatus OrderstatusNavigation { get; set; } = null!;

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

    public virtual Table Table { get; set; } = null!;

    public virtual User? UpdatedByNavigation { get; set; }
}
