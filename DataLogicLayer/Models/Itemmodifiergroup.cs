using System;
using System.Collections.Generic;

namespace DataLogicLayer.Models;

public partial class Itemmodifiergroup
{
    public long Id { get; set; }

    public long Itemid { get; set; }

    public long ModifierGroupId { get; set; }

    public int? MaxAllowed { get; set; }

    public int? MinAllowed { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public long CreatedBy { get; set; }

    public long? UpdatedBy { get; set; }

    public bool Isdeleted { get; set; }

    public virtual User CreatedByNavigation { get; set; } = null!;

    public virtual Item Item { get; set; } = null!;

    public virtual Modifiergroup ModifierGroup { get; set; } = null!;

    public virtual User? UpdatedByNavigation { get; set; }
}
