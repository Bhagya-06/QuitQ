using System;
using System.Collections.Generic;

namespace QuitQ.API.Models;

public partial class Payment
{
    public int Id { get; set; }

    public int? OrderId { get; set; }

    public int UserId { get; set; }

    public string PaymentMethod { get; set; } = null!;

    public decimal Amount { get; set; }

    public string? PaymentStatus { get; set; }

    public string? TransactionId { get; set; }

    public string? PaymentGateway { get; set; }

    public DateTime? CreatedDate { get; set; }

    public virtual Order? Order { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual User User { get; set; } = null!;
}
