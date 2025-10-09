using System;
using System.Collections.Generic;

namespace lab2.Models;

public partial class ScientificDirection
{
    public int DirectionId { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public virtual ICollection<Publication> Publications { get; set; } = new List<Publication>();
}
