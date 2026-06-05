INSERT INTO Users (Username, Name, Email, PasswordHash, Phone, Address, Role) VALUES 
('user1', 'Arun Kumar', 'arun@test.com', '123456', '9000000001', 'Chennai', 'Buyer'),
('user2', 'Priya Sharma', 'priya@test.com', '123456', '9000000002', 'Bangalore', 'Buyer'),
('seller1', 'Raj Stores', 'raj@test.com', '123456', '9000000003', 'Mumbai', 'Seller'),
('seller2', 'Tech World', 'tech@test.com', '123456', '9000000004', 'Delhi', 'Seller'),
('admin1', 'Admin User', 'admin@test.com', '123456', '9000000005', 'Chennai', 'Admin');

INSERT INTO Sellers (UserId, StoreName, City, Country) VALUES
(3, 'Raj Electronics', 'Mumbai', 'India'),
(4, 'Tech World Store', 'Delhi', 'India');

INSERT INTO Categories (Name) VALUES
('Electronics'),
('Fashion'),
('Home & Living'),
('Books'),
('Gaming'), ('Sports');

INSERT INTO Brands (Name) VALUES
('Samsung'),
('Nike'),
('LG'),
('Firebolt'),
('Boat');

INSERT INTO Products (Name, Price, Stock, CategoryId, BrandId, SellerId, ImageUrl) VALUES
('Samsung Galaxy M14', 15000, 10, 1, 1, 1, '/products/samsung-galaxy.webp'),
('Nike Running Shoes', 5000, 20, 2, 2, 2, '/products/nike-shoes.webp'),
('LG Refrigerator', 30000, 5, 3, 3, 1, '/products/lg-refrigerator.webp'),
('Harry Potter Book', 800, 50, 4, 4, 2, '/products/harry-potter.webp'),
('Boat Headphones', 2000, 25, 5, 5, 1, '/products/boat-headphones.webp');

INSERT INTO Addresses (UserId, Address1, City, Pincode) VALUES
(1, 'No 10, Anna Nagar', 'Chennai', '600040'),
(2, 'MG Road', 'Bangalore', '560001');

INSERT INTO ShoppingCart (UserId, ProductId, Quantity) VALUES
(1, 1, 1),
(1, 2, 2);
