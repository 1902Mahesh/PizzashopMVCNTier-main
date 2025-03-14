using System;
using System.Collections.Generic;

namespace DataLogicLayer.Models;

public partial class Role
{
    public long RoleId { get; set; }

    public string Rolename { get; set; } = null!;

    public virtual ICollection<Rolesandpermission> Rolesandpermissions { get; set; } = new List<Rolesandpermission>();

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
