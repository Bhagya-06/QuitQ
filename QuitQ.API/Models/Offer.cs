using System;
using System.Collections.Generic;

namespace QuitQ.API.Models;

public partial class Offer
{
    public int Id { get; set; }

    public string? Title { get; set; }

    public decimal? DiscountPercentage { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public bool? IsActive { get; set; }
}
