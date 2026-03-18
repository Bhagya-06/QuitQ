using System;
using System.Collections.Generic;

namespace QuitQ.API.Models;

public partial class Seller
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public string StoreName { get; set; } = null!;

    public string? City { get; set; }

    public string? Country { get; set; }

    public string? Gstin { get; set; }

    public string? IdProofDocument { get; set; }

    public string? IdProofNumber { get; set; }

    public string? BusinessLicense { get; set; }

    public string? BankAccountNumber { get; set; }

    public string? BankIfsc { get; set; }

    public string? VerificationStatus { get; set; }

    public DateTime? VerificationDate { get; set; }

    public int? AdminApprovedBy { get; set; }

    public decimal? ShopReviews { get; set; }

    public virtual User? AdminApprovedByNavigation { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();

    public virtual ICollection<ShopReview> ShopReviewsNavigation { get; set; } = new List<ShopReview>();

    public virtual User User { get; set; } = null!;
}
