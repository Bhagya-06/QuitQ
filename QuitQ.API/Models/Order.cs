using System;
using System.Collections.Generic;

namespace QuitQ.API.Models;

public partial class Order
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int AddressId { get; set; }

    public decimal Total { get; set; }

    public string? Status { get; set; }

    public string? TrackingNumber { get; set; }

    public DateTime? CreatedDate { get; set; }

    public int? PaymentId { get; set; }

    public virtual Address Address { get; set; } = null!;

    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    public virtual Payment? Payment { get; set; }

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

    public virtual User User { get; set; } = null!;
}
