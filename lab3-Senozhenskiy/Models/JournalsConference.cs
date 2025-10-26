using System;
using System.Collections.Generic;

namespace SciencePortalWebApp.Models;

public partial class JournalsConference
{
    public int JournalConfId { get; set; }

    public string Name { get; set; } = null!;

    public string? Rating { get; set; }

    public string? Publisher { get; set; }

    public string? IssnIsbn { get; set; }

    public virtual ICollection<Publication> Publications { get; set; } = new List<Publication>();
}
