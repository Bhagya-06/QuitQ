using System;
using System.Collections.Generic;

namespace QuitQ.API.Models;

public partial class ProductReview
{
    public int Id { get; set; }

    public int ProductId { get; set; }

    public int UserId { get; set; }

    public int Rating { get; set; }

    public string? Comment { get; set; }

    public string? ReviewImages { get; set; }

    public string? ReviewVideo { get; set; }

    public int? MediaCount { get; set; }

    public DateTime? CreatedDate { get; set; }

    public virtual Product Product { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
