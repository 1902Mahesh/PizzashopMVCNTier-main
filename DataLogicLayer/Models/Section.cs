using System;
using System.Collections.Generic;

namespace DataLogicLayer.Models;

public partial class Section
{
    public long SectionId { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public long CreatedBy { get; set; }

    public long? UpdatedBy { get; set; }

    public bool Isdeleted { get; set; }

    public virtual User CreatedByNavigation { get; set; } = null!;

    public virtual ICollection<Table> Tables { get; set; } = new List<Table>();

    public virtual User? UpdatedByNavigation { get; set; }

    public virtual ICollection<Waitinglist> Waitinglists { get; set; } = new List<Waitinglist>();
}
