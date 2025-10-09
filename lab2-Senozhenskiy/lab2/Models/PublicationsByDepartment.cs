using System;
using System.Collections.Generic;

namespace lab2.Models;

public partial class PublicationsByDepartment
{
    public string Department { get; set; } = null!;

    public string Title { get; set; } = null!;

    public string Type { get; set; } = null!;

    public DateTime Year { get; set; }
}
