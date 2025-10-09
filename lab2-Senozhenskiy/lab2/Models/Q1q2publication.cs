using System;
using System.Collections.Generic;

namespace lab2.Models;

public partial class Q1q2publication
{
    public string Title { get; set; } = null!;

    public string Type { get; set; } = null!;

    public DateTime Year { get; set; }

    public string? Rating { get; set; }

    public string JournalConference { get; set; } = null!;
}
