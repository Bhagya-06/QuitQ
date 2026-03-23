using System;
using System.Collections.Generic;

namespace QuitQ.API.Models;

public partial class VwOrderSummary
{
    public int Id { get; set; }

    public string Customer { get; set; } = null!;

    public decimal Total { get; set; }

    public string? Status { get; set; }

    public DateTime? CreatedDate { get; set; }
}
