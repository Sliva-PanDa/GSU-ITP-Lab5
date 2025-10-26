using System;
using System.Collections.Generic;

namespace SciencePortalWebApp.Models;

public partial class Project
{
    public int ProjectId { get; set; }

    public string Name { get; set; } = null!;

    public string Number { get; set; } = null!;

    public string FundingOrg { get; set; } = null!;

    public DateOnly StartDate { get; set; }

    public DateOnly? EndDate { get; set; }

    public int LeaderId { get; set; }

    public virtual Teacher Leader { get; set; } = null!;

    public virtual ICollection<Publication> Publications { get; set; } = new List<Publication>();
}
