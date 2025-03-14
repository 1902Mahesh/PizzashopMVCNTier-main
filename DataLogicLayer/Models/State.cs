using System;
using System.Collections.Generic;

namespace DataLogicLayer.Models;

public partial class State
{
    public long StateId { get; set; }

    public string Name { get; set; } = null!;

    public long Countryid { get; set; }

    public virtual ICollection<City> Cities { get; set; } = new List<City>();

    public virtual Country Country { get; set; } = null!;

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
