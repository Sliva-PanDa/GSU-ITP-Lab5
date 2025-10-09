using System;
using System.Collections.Generic;

namespace lab2.Models;

public partial class Publication
{
    public int PublicationId { get; set; }

    public string Title { get; set; } = null!;

    public string Type { get; set; } = null!;

    public DateTime Year { get; set; }

    public string? DoiLink { get; set; }

    public string? FilePath { get; set; }

    public int? JournalConfId { get; set; }

    public int? DirectionId { get; set; }

    public string? Keywords { get; set; }

    public virtual ScientificDirection? Direction { get; set; }

    public virtual JournalsConference? JournalConf { get; set; }

    public virtual ICollection<Project> Projects { get; set; } = new List<Project>();

    public virtual ICollection<Teacher> Teachers { get; set; } = new List<Teacher>();
}
