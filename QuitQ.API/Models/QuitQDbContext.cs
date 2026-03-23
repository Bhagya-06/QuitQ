using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace QuitQ.API.Models;

public partial class QuitQDbContext : DbContext
{
    public QuitQDbContext()
    {
    }

    public QuitQDbContext(DbContextOptions<QuitQDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Address> Addresses { get; set; }

    public virtual DbSet<Brand> Brands { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Offer> Offers { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderItem> OrderItems { get; set; }

    public virtual DbSet<Payment> Payments { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<ProductReview> ProductReviews { get; set; }

    public virtual DbSet<Seller> Sellers { get; set; }

    public virtual DbSet<ShopReview> ShopReviews { get; set; }

    public virtual DbSet<ShoppingCart> ShoppingCarts { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<VwOrderSummary> VwOrderSummaries { get; set; }

    public virtual DbSet<VwProductCatalog> VwProductCatalogs { get; set; }

    public virtual DbSet<Wishlist> Wishlists { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=QuitQ;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Address>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Addresse__3214EC0750CD6C85");

            entity.HasIndex(e => e.UserId, "IX_Addresses_UserId");

            entity.Property(e => e.Address1).HasMaxLength(200);
            entity.Property(e => e.AddressName)
                .HasMaxLength(50)
                .HasDefaultValue("Home");
            entity.Property(e => e.City).HasMaxLength(100);
            entity.Property(e => e.IsDefault).HasDefaultValue(false);
            entity.Property(e => e.Pincode).HasMaxLength(10);

            entity.HasOne(d => d.User).WithMany(p => p.Addresses)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Addresses__UserI__04E4BC85");
        });

        modelBuilder.Entity<Brand>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Brands__3214EC07FED00D8C");

            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Categori__3214EC076D3863D0");

            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<Offer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Offers__3214EC07096C7952");

            entity.Property(e => e.DiscountPercentage).HasColumnType("decimal(5, 2)");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Title).HasMaxLength(200);
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Orders__3214EC07A682A97A");

            entity.HasIndex(e => e.CreatedDate, "IX_Orders_CreatedDate").IsDescending();

            entity.HasIndex(e => e.Status, "IX_Orders_Status");

            entity.HasIndex(e => e.UserId, "IX_Orders_UserId");

            entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .HasDefaultValue("Pending");
            entity.Property(e => e.Total).HasColumnType("decimal(12, 2)");
            entity.Property(e => e.TrackingNumber).HasMaxLength(100);

            entity.HasOne(d => d.Address).WithMany(p => p.Orders)
                .HasForeignKey(d => d.AddressId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Orders__AddressI__0A9D95DB");

            entity.HasOne(d => d.Payment).WithMany(p => p.Orders)
                .HasForeignKey(d => d.PaymentId)
                .HasConstraintName("FK_Orders_Payments");

            entity.HasOne(d => d.User).WithMany(p => p.Orders)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Orders__UserId__09A971A2");
        });

        modelBuilder.Entity<OrderItem>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__OrderIte__3214EC07F4AC6AD8");

            entity.ToTable(tb =>
                {
                    tb.HasTrigger("TRG_CheckStock");
                    tb.HasTrigger("TRG_ReduceStock");
                });

            entity.HasIndex(e => e.OrderId, "IX_OrderItems_OrderId");

            entity.Property(e => e.Price).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderItems)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__OrderItem__Order__10566F31");

            entity.HasOne(d => d.Product).WithMany(p => p.OrderItems)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__OrderItem__Produ__114A936A");
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Payments__3214EC07C52C8F39");

            entity.HasIndex(e => e.OrderId, "IX_Payments_OrderId");

            entity.HasIndex(e => e.PaymentStatus, "IX_Payments_PaymentStatus");

            entity.HasIndex(e => e.UserId, "IX_Payments_UserId");

            entity.Property(e => e.Amount).HasColumnType("decimal(12, 2)");
            entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.PaymentGateway).HasMaxLength(50);
            entity.Property(e => e.PaymentMethod).HasMaxLength(50);
            entity.Property(e => e.PaymentStatus)
                .HasMaxLength(20)
                .HasDefaultValue("Pending");
            entity.Property(e => e.TransactionId).HasMaxLength(100);

            entity.HasOne(d => d.Order).WithMany(p => p.Payments)
                .HasForeignKey(d => d.OrderId)
                .HasConstraintName("FK__Payments__OrderI__151B244E");

            entity.HasOne(d => d.User).WithMany(p => p.Payments)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Payments__UserId__160F4887");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Products__3214EC07E51A44D7");

            entity.HasIndex(e => e.BrandId, "IX_Products_BrandId");

            entity.HasIndex(e => e.CategoryId, "IX_Products_CategoryId");

            entity.HasIndex(e => e.IsActive, "IX_Products_IsActive");

            entity.HasIndex(e => e.Price, "IX_Products_Price");

            entity.HasIndex(e => new { e.Name, e.CategoryId, e.IsActive }, "IX_Products_Search");

            entity.HasIndex(e => e.SellerId, "IX_Products_SellerId");

            entity.HasIndex(e => new { e.SellerId, e.IsActive }, "IX_Products_SellerId_Active");

            entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Description).HasMaxLength(1000);
            entity.Property(e => e.ImageUrl).HasMaxLength(500);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Name).HasMaxLength(200);
            entity.Property(e => e.Price).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.Brand).WithMany(p => p.Products)
                .HasForeignKey(d => d.BrandId)
                .HasConstraintName("FK__Products__BrandI__70DDC3D8");

            entity.HasOne(d => d.Category).WithMany(p => p.Products)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Products__Catego__6FE99F9F");

            entity.HasOne(d => d.Seller).WithMany(p => p.Products)
                .HasForeignKey(d => d.SellerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Products__Seller__71D1E811");
        });

        modelBuilder.Entity<ProductReview>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ProductR__3214EC07767A501C");

            entity.HasIndex(e => e.ProductId, "IX_ProductReviews_ProductId");

            entity.HasIndex(e => e.UserId, "IX_ProductReviews_UserId");

            entity.Property(e => e.Comment).HasMaxLength(1000);
            entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.MediaCount).HasDefaultValue(0);
            entity.Property(e => e.ReviewVideo).HasMaxLength(500);

            entity.HasOne(d => d.Product).WithMany(p => p.ProductReviews)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ProductRe__Produ__76969D2E");

            entity.HasOne(d => d.User).WithMany(p => p.ProductReviews)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ProductRe__UserI__778AC167");
        });

        modelBuilder.Entity<Seller>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Sellers__3214EC07A0E117EC");

            entity.HasIndex(e => e.VerificationStatus, "IX_Sellers_VerificationStatus");

            entity.HasIndex(e => e.UserId, "UQ__Sellers__1788CC4D88B01EDD").IsUnique();

            entity.Property(e => e.BankAccountNumber).HasMaxLength(20);
            entity.Property(e => e.BankIfsc)
                .HasMaxLength(15)
                .HasColumnName("BankIFSC");
            entity.Property(e => e.BusinessLicense).HasMaxLength(500);
            entity.Property(e => e.City).HasMaxLength(50);
            entity.Property(e => e.Country).HasMaxLength(50);
            entity.Property(e => e.Gstin)
                .HasMaxLength(20)
                .HasColumnName("GSTIN");
            entity.Property(e => e.IdProofDocument).HasMaxLength(500);
            entity.Property(e => e.IdProofNumber).HasMaxLength(50);
            entity.Property(e => e.ShopReviews)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(3, 2)");
            entity.Property(e => e.StoreName).HasMaxLength(100);
            entity.Property(e => e.VerificationStatus)
                .HasMaxLength(20)
                .HasDefaultValue("Pending");

            entity.HasOne(d => d.AdminApprovedByNavigation).WithMany(p => p.SellerAdminApprovedByNavigations)
                .HasForeignKey(d => d.AdminApprovedBy)
                .HasConstraintName("FK__Sellers__AdminAp__6754599E");

            entity.HasOne(d => d.User).WithOne(p => p.SellerUser)
                .HasForeignKey<Seller>(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Sellers__UserId__6477ECF3");
        });

        modelBuilder.Entity<ShopReview>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ShopRevi__3214EC0788FEA922");

            entity.ToTable(tb => tb.HasTrigger("TRG_UpdateSellerRating"));

            entity.HasIndex(e => e.SellerId, "IX_ShopReviews_SellerId");

            entity.Property(e => e.Comment).HasMaxLength(1000);
            entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Seller).WithMany(p => p.ShopReviewsNavigation)
                .HasForeignKey(d => d.SellerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ShopRevie__Selle__1CBC4616");

            entity.HasOne(d => d.User).WithMany(p => p.ShopReviews)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ShopRevie__UserI__1DB06A4F");
        });

        modelBuilder.Entity<ShoppingCart>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Shopping__3214EC07DB7DA2A1");

            entity.ToTable("ShoppingCart");

            entity.HasIndex(e => e.UserId, "IX_ShoppingCart_UserId");

            entity.HasIndex(e => new { e.UserId, e.ProductId }, "IX_ShoppingCart_UserId_ProductId");

            entity.HasIndex(e => new { e.UserId, e.ProductId }, "UQ__Shopping__DCC80021B0F28917").IsUnique();

            entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Quantity).HasDefaultValue(1);

            entity.HasOne(d => d.Product).WithMany(p => p.ShoppingCarts)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ShoppingC__Produ__7F2BE32F");

            entity.HasOne(d => d.User).WithMany(p => p.ShoppingCarts)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ShoppingC__UserI__7E37BEF6");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Users__3214EC07F2DDECBC");

            entity.HasIndex(e => e.Email, "IX_Users_Email").IsUnique();

            entity.HasIndex(e => e.Role, "IX_Users_Role");

            entity.HasIndex(e => e.Username, "IX_Users_Username").IsUnique();

            entity.HasIndex(e => e.Username, "UQ__Users__536C85E49F43821E").IsUnique();

            entity.HasIndex(e => e.Email, "UQ__Users__A9D10534662FD42C").IsUnique();

            entity.Property(e => e.Address).HasMaxLength(500);
            entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Email).HasMaxLength(150);
            entity.Property(e => e.Gender).HasMaxLength(10);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.PasswordHash).HasMaxLength(255);
            entity.Property(e => e.Phone).HasMaxLength(15);
            entity.Property(e => e.Role).HasMaxLength(20);
            entity.Property(e => e.Username).HasMaxLength(50);
        });

        modelBuilder.Entity<VwOrderSummary>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("VW_OrderSummary");

            entity.Property(e => e.Customer).HasMaxLength(100);
            entity.Property(e => e.Status).HasMaxLength(20);
            entity.Property(e => e.Total).HasColumnType("decimal(12, 2)");
        });

        modelBuilder.Entity<VwProductCatalog>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("VW_ProductCatalog");

            entity.Property(e => e.Brand).HasMaxLength(100);
            entity.Property(e => e.Category).HasMaxLength(100);
            entity.Property(e => e.Name).HasMaxLength(200);
            entity.Property(e => e.Price).HasColumnType("decimal(10, 2)");
        });

        modelBuilder.Entity<Wishlist>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Wishlist__3214EC079FBA85F4");

            entity.ToTable("Wishlist");

            entity.HasIndex(e => new { e.UserId, e.ProductId }, "UQ__Wishlist__DCC80021F4E1840C").IsUnique();

            entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Product).WithMany(p => p.Wishlists)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK__Wishlist__Produc__2DE6D218");

            entity.HasOne(d => d.User).WithMany(p => p.Wishlists)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Wishlist__UserId__2CF2ADDF");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
