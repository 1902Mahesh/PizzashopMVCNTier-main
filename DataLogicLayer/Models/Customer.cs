using System;
using System.Collections.Generic;

namespace DataLogicLayer.Models;

public partial class Customer
{
    public long CustomerId { get; set; }

    public string Name { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public int Persons { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public long CreatedBy { get; set; }

    public long? UpdatedBy { get; set; }

    public bool Isdeleted { get; set; }

    public virtual ICollection<Booktable> Booktables { get; set; } = new List<Booktable>();

    public virtual User CreatedByNavigation { get; set; } = null!;

    public virtual ICollection<Customerreview> Customerreviews { get; set; } = new List<Customerreview>();

    public virtual ICollection<Favoriteitem> Favoriteitems { get; set; } = new List<Favoriteitem>();

    public virtual ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

    public virtual User? UpdatedByNavigation { get; set; }

    public virtual ICollection<Waitinglist> Waitinglists { get; set; } = new List<Waitinglist>();
}
