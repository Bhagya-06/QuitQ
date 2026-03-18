using System;
using System.Collections.Generic;

namespace QuitQ.API.Models;

public partial class ShopReview
{
    public int Id { get; set; }

    public int SellerId { get; set; }

    public int UserId { get; set; }

    public int Rating { get; set; }

    public string? Comment { get; set; }

    public DateTime? CreatedDate { get; set; }

    public virtual Seller Seller { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
