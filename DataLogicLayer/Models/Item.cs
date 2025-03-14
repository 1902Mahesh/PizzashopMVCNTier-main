using System;
using System.Collections.Generic;

namespace DataLogicLayer.Models;

public partial class Item
{
    public long ItemId { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public long ItemtypeId { get; set; }

    public decimal Rate { get; set; }

    public long Unitid { get; set; }

    public int Quantity { get; set; }

    public decimal AdditionalTax { get; set; }

    public bool DefaultTax { get; set; }

    public long Taxid { get; set; }

    public bool Isavailable { get; set; }

    public string? Shortcode { get; set; }

    public string? Imgurl { get; set; }

    public long Categoryid { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public long CreatedBy { get; set; }

    public long? UpdatedBy { get; set; }

    public bool Isdeleted { get; set; }

    public virtual Category Category { get; set; } = null!;

    public virtual User CreatedByNavigation { get; set; } = null!;

    public virtual ICollection<Itemmodifiergroup> Itemmodifiergroups { get; set; } = new List<Itemmodifiergroup>();

    public virtual Itemtype Itemtype { get; set; } = null!;

    public virtual ICollection<Orderdetail> Orderdetails { get; set; } = new List<Orderdetail>();

    public virtual Taxis Tax { get; set; } = null!;

    public virtual Unit Unit { get; set; } = null!;

    public virtual User? UpdatedByNavigation { get; set; }
}
