using System;
using System.Collections.Generic;

namespace DataLogicLayer.Models;

public partial class Rolesandpermission
{
    public long Id { get; set; }

    public long Roleid { get; set; }

    public long Permissionid { get; set; }

    public bool Canview { get; set; }

    public bool Canaddedit { get; set; }

    public bool Candelete { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public long CreatedBy { get; set; }

    public long? UpdatedBy { get; set; }

    public bool Isdeleted { get; set; }

    public virtual User CreatedByNavigation { get; set; } = null!;

    public virtual Permission Permission { get; set; } = null!;

    public virtual Role Role { get; set; } = null!;

    public virtual User? UpdatedByNavigation { get; set; }
}
