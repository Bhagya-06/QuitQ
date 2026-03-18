USE QuitQ;

-- 1. USER LOGIN/SEARCH 
CREATE UNIQUE NONCLUSTERED INDEX IX_Users_Email ON Users(Email);
CREATE UNIQUE NONCLUSTERED INDEX IX_Users_Username ON Users(Username);
CREATE NONCLUSTERED INDEX IX_Users_Role ON Users(Role);

-- 2. PRODUCT CATALOG 
CREATE NONCLUSTERED INDEX IX_Products_CategoryId ON Products(CategoryId);
CREATE NONCLUSTERED INDEX IX_Products_BrandId ON Products(BrandId);
CREATE NONCLUSTERED INDEX IX_Products_SellerId ON Products(SellerId);
CREATE NONCLUSTERED INDEX IX_Products_IsActive ON Products(IsActive);
CREATE NONCLUSTERED INDEX IX_Products_Price ON Products(Price); 

-- 3. SELLER DASHBOARD
CREATE NONCLUSTERED INDEX IX_Products_SellerId_Active ON Products(SellerId, IsActive);

-- 4. SHOPPING CART
CREATE NONCLUSTERED INDEX IX_ShoppingCart_UserId ON ShoppingCart(UserId);
CREATE NONCLUSTERED INDEX IX_ShoppingCart_UserId_ProductId ON ShoppingCart(UserId, ProductId);

-- 5. ORDERS
CREATE NONCLUSTERED INDEX IX_Orders_UserId ON Orders(UserId);
CREATE NONCLUSTERED INDEX IX_Orders_Status ON Orders(Status);
CREATE NONCLUSTERED INDEX IX_Orders_CreatedDate ON Orders(CreatedDate DESC);

-- 6. PAYMENTS 
CREATE NONCLUSTERED INDEX IX_Payments_OrderId ON Payments(OrderId);
CREATE NONCLUSTERED INDEX IX_Payments_UserId ON Payments(UserId);
CREATE NONCLUSTERED INDEX IX_Payments_PaymentStatus ON Payments(PaymentStatus);

-- 7. REVIEWS
CREATE NONCLUSTERED INDEX IX_ProductReviews_ProductId ON ProductReviews(ProductId);
CREATE NONCLUSTERED INDEX IX_ProductReviews_UserId ON ProductReviews(UserId);
CREATE NONCLUSTERED INDEX IX_ShopReviews_SellerId ON ShopReviews(SellerId);

-- 8. ADDRESSES 
CREATE NONCLUSTERED INDEX IX_Addresses_UserId ON Addresses(UserId);

-- 9. COMPOSITE INDEXES 
CREATE NONCLUSTERED INDEX IX_Products_Search ON Products(Name, CategoryId, IsActive) INCLUDE (Price, Stock);

CREATE NONCLUSTERED INDEX IX_OrderItems_OrderId ON OrderItems(OrderId) INCLUDE (ProductId, Quantity, Price);

-- 10. SELLER VERIFICATION
CREATE NONCLUSTERED INDEX IX_Sellers_VerificationStatus ON Sellers(VerificationStatus);
