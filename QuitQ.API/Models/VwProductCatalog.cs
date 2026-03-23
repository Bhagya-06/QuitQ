using System;
using System.Collections.Generic;

namespace QuitQ.API.Models;

public partial class VwProductCatalog
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public decimal Price { get; set; }

    public int Stock { get; set; }

    public string Category { get; set; } = null!;

    public string? Brand { get; set; }
}
