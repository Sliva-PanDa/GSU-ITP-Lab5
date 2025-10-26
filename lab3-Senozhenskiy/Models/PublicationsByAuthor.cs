using System;
using System.Collections.Generic;

namespace SciencePortalWebApp.Models;

public partial class PublicationsByAuthor
{
    public string Author { get; set; } = null!;

    public string Title { get; set; } = null!;

    public string Type { get; set; } = null!;

    public DateTime Year { get; set; }

    public string JournalConference { get; set; } = null!;
}
