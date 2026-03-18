using System;
using System.Collections.Generic;

namespace QuitQ.API.Models;

public partial class ShoppingCart
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int ProductId { get; set; }

    public int Quantity { get; set; }

    public DateTime? CreatedDate { get; set; }

    public virtual Product Product { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
