using System;
using System.Collections.Generic;

namespace QuitQ.API.Models;

public partial class Address
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public string AddressName { get; set; } = null!;

    public string Address1 { get; set; } = null!;

    public string? City { get; set; }

    public string? Pincode { get; set; }

    public bool? IsDefault { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual User User { get; set; } = null!;
}
