using System;
using System.Collections.Generic;

namespace DataLogicLayer.Models;

public partial class User
{
    public long Id { get; set; }

    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Firstname { get; set; } = null!;

    public string Lastname { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string? Imgurl { get; set; }

    public bool Status { get; set; }

    public long Roleid { get; set; }

    public long Countryid { get; set; }

    public long Stateid { get; set; }

    public long Cityid { get; set; }

    public string Address { get; set; } = null!;

    public int Zipcode { get; set; }

    public string Phone { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public long CreatedBy { get; set; }

    public long? UpdatedBy { get; set; }

    public bool Isdeleted { get; set; }

    public virtual ICollection<Booktable> BooktableCreatedByNavigations { get; set; } = new List<Booktable>();

    public virtual ICollection<Booktable> BooktableUpdatedByNavigations { get; set; } = new List<Booktable>();

    public virtual ICollection<Category> CategoryCreatedByNavigations { get; set; } = new List<Category>();

    public virtual ICollection<Category> CategoryUpdatedByNavigations { get; set; } = new List<Category>();

    public virtual Country Country { get; set; } = null!;

    public virtual User CreatedByNavigation { get; set; } = null!;

    public virtual ICollection<Customer> CustomerCreatedByNavigations { get; set; } = new List<Customer>();

    public virtual ICollection<Customer> CustomerUpdatedByNavigations { get; set; } = new List<Customer>();

    public virtual ICollection<Customerreview> CustomerreviewCreatedByNavigations { get; set; } = new List<Customerreview>();

    public virtual ICollection<Customerreview> CustomerreviewUpdatedByNavigations { get; set; } = new List<Customerreview>();

    public virtual ICollection<User> InverseCreatedByNavigation { get; set; } = new List<User>();

    public virtual ICollection<User> InverseUpdatedByNavigation { get; set; } = new List<User>();

    public virtual ICollection<Invoice> InvoiceCreatedByNavigations { get; set; } = new List<Invoice>();

    public virtual ICollection<Invoice> InvoiceUpdatedByNavigations { get; set; } = new List<Invoice>();

    public virtual ICollection<Item> ItemCreatedByNavigations { get; set; } = new List<Item>();

    public virtual ICollection<Item> ItemUpdatedByNavigations { get; set; } = new List<Item>();

    public virtual ICollection<Itemmodifiergroup> ItemmodifiergroupCreatedByNavigations { get; set; } = new List<Itemmodifiergroup>();

    public virtual ICollection<Itemmodifiergroup> ItemmodifiergroupUpdatedByNavigations { get; set; } = new List<Itemmodifiergroup>();

    public virtual ICollection<Kot> KotCreatedByNavigations { get; set; } = new List<Kot>();

    public virtual ICollection<Kot> KotUpdatedByNavigations { get; set; } = new List<Kot>();

    public virtual ICollection<Modifiergroup> ModifiergroupCreatedByNavigations { get; set; } = new List<Modifiergroup>();

    public virtual ICollection<Modifiergroup> ModifiergroupUpdatedByNavigations { get; set; } = new List<Modifiergroup>();

    public virtual ICollection<Modifiergroupitemmap> ModifiergroupitemmapCreatedByNavigations { get; set; } = new List<Modifiergroupitemmap>();

    public virtual ICollection<Modifiergroupitemmap> ModifiergroupitemmapUpdatedByNavigations { get; set; } = new List<Modifiergroupitemmap>();

    public virtual ICollection<Modifieritem> ModifieritemCreatedByNavigations { get; set; } = new List<Modifieritem>();

    public virtual ICollection<Modifieritem> ModifieritemUpdatedByNavigations { get; set; } = new List<Modifieritem>();

    public virtual ICollection<Order> OrderCreatedByNavigations { get; set; } = new List<Order>();

    public virtual ICollection<Order> OrderUpdatedByNavigations { get; set; } = new List<Order>();

    public virtual Role Role { get; set; } = null!;

    public virtual ICollection<Rolesandpermission> RolesandpermissionCreatedByNavigations { get; set; } = new List<Rolesandpermission>();

    public virtual ICollection<Rolesandpermission> RolesandpermissionUpdatedByNavigations { get; set; } = new List<Rolesandpermission>();

    public virtual ICollection<Section> SectionCreatedByNavigations { get; set; } = new List<Section>();

    public virtual ICollection<Section> SectionUpdatedByNavigations { get; set; } = new List<Section>();

    public virtual State State { get; set; } = null!;

    public virtual ICollection<Table> TableCreatedByNavigations { get; set; } = new List<Table>();

    public virtual ICollection<Table> TableUpdatedByNavigations { get; set; } = new List<Table>();

    public virtual ICollection<Taxis> TaxisCreatedByNavigations { get; set; } = new List<Taxis>();

    public virtual ICollection<Taxis> TaxisUpdatedByNavigations { get; set; } = new List<Taxis>();

    public virtual User? UpdatedByNavigation { get; set; }

    public virtual ICollection<Waitinglist> WaitinglistCreatedByNavigations { get; set; } = new List<Waitinglist>();

    public virtual ICollection<Waitinglist> WaitinglistUpdatedByNavigations { get; set; } = new List<Waitinglist>();
}
