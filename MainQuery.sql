CREATE DATABASE QuitQ
USE QuitQ

CREATE TABLE Users (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Username NVARCHAR(50) NOT NULL UNIQUE,  
    Name NVARCHAR(100) NOT NULL,               
    Gender NVARCHAR(10),                       
    Email NVARCHAR(150) NOT NULL UNIQUE,      
    PasswordHash NVARCHAR(255) NOT NULL,       
    Phone NVARCHAR(15) NOT NULL,              
    Address NVARCHAR(500) NOT NULL,                     
    Role NVARCHAR(20) NOT NULL CHECK (Role IN ('Buyer', 'Seller', 'Admin')),
    IsActive BIT DEFAULT 1,
    CreatedDate DATETIME2 DEFAULT GETDATE()
);

CREATE TABLE Sellers (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    UserId INT NOT NULL UNIQUE FOREIGN KEY REFERENCES Users(Id),
    StoreName NVARCHAR(100) NOT NULL,
    City NVARCHAR(50),
    Country NVARCHAR(50),
    GSTIN NVARCHAR(20),
    IdProofDocument NVARCHAR(500),      
    IdProofNumber NVARCHAR(50),         
    BusinessLicense NVARCHAR(500),      
    BankAccountNumber NVARCHAR(20),   
    BankIFSC NVARCHAR(15), 
    VerificationStatus NVARCHAR(20) DEFAULT 'Pending' CHECK (VerificationStatus IN ('Pending', 'Verified', 'Rejected')),
    VerificationDate DATETIME2 NULL,
    AdminApprovedBy INT NULL FOREIGN KEY REFERENCES Users(Id),
    ShopReviews DECIMAL(3,2) DEFAULT 0
);

CREATE TABLE Categories (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL,
    Description NVARCHAR(500)
);

CREATE TABLE Brands (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL
);

CREATE TABLE Products (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(200) NOT NULL,
    Description NVARCHAR(1000),             
    Price DECIMAL(10,2) NOT NULL,            
    Stock INT NOT NULL DEFAULT 0,         
    ImageUrl NVARCHAR(500),           
    CategoryId INT NOT NULL FOREIGN KEY REFERENCES Categories(Id),
    BrandId INT NULL FOREIGN KEY REFERENCES Brands(Id),          
    SellerId INT NOT NULL FOREIGN KEY REFERENCES Sellers(Id),     
    IsActive BIT DEFAULT 1,
    CreatedDate DATETIME2 DEFAULT GETDATE()
);

CREATE TABLE ProductReviews (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    ProductId INT NOT NULL FOREIGN KEY REFERENCES Products(Id),
    UserId INT NOT NULL FOREIGN KEY REFERENCES Users(Id),
    Rating INT NOT NULL CHECK (Rating BETWEEN 1 AND 5),
    Comment NVARCHAR(1000),
    ReviewImages NVARCHAR(MAX) NULL,   
    ReviewVideo NVARCHAR(500) NULL,     
    MediaCount INT DEFAULT 0,
    CreatedDate DATETIME2 DEFAULT GETDATE()
);

CREATE TABLE ShoppingCart (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    UserId INT NOT NULL FOREIGN KEY REFERENCES Users(Id),
    ProductId INT NOT NULL FOREIGN KEY REFERENCES Products(Id),
    Quantity INT NOT NULL DEFAULT 1 CHECK (Quantity > 0),
    CreatedDate DATETIME2 DEFAULT GETDATE(),
    UNIQUE(UserId, ProductId)
);

CREATE TABLE Addresses (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    UserId INT NOT NULL FOREIGN KEY REFERENCES Users(Id),
    AddressName NVARCHAR(50) NOT NULL DEFAULT 'Home',
    Address1 NVARCHAR(200) NOT NULL,
    City NVARCHAR(100),
    Pincode NVARCHAR(10),
    IsDefault BIT DEFAULT 0
);

CREATE TABLE Orders (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    UserId INT NOT NULL FOREIGN KEY REFERENCES Users(Id),
    AddressId INT NOT NULL FOREIGN KEY REFERENCES Addresses(Id),
    PaymentId INT NULL,
    Total DECIMAL(12,2) NOT NULL,
    Status NVARCHAR(20) DEFAULT 'Pending' CHECK (Status IN ('Pending','Processing','Shipped','Delivered','Cancelled')), 
    TrackingNumber NVARCHAR(100),
    CreatedDate DATETIME2 DEFAULT GETDATE()
);

CREATE TABLE OrderItems (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    OrderId INT NOT NULL FOREIGN KEY REFERENCES Orders(Id),
    ProductId INT NOT NULL FOREIGN KEY REFERENCES Products(Id),
    Quantity INT NOT NULL CHECK (Quantity > 0),
    Price DECIMAL(10,2) NOT NULL 
);

CREATE TABLE Payments (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    OrderId INT NULL FOREIGN KEY REFERENCES Orders(Id),
    UserId INT NOT NULL FOREIGN KEY REFERENCES Users(Id),
    PaymentMethod NVARCHAR(50) NOT NULL CHECK (PaymentMethod IN ('UPI','Card','Netbanking','Cash on Delivery')),
    Amount DECIMAL(12,2) NOT NULL,
    PaymentStatus NVARCHAR(20) DEFAULT 'Pending' CHECK (PaymentStatus IN ('Pending','Success','Failed','Refunded')),
    TransactionId NVARCHAR(100),           
    PaymentGateway NVARCHAR(50),        
    CreatedDate DATETIME2 DEFAULT GETDATE()
);

CREATE TABLE ShopReviews (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    SellerId INT NOT NULL FOREIGN KEY REFERENCES Sellers(Id),
    UserId INT NOT NULL FOREIGN KEY REFERENCES Users(Id),
    Rating INT NOT NULL CHECK (Rating BETWEEN 1 AND 5),
    Comment NVARCHAR(1000),
    CreatedDate DATETIME2 DEFAULT GETDATE()
);

ALTER TABLE Orders ADD CONSTRAINT FK_Orders_Payments FOREIGN KEY (PaymentId) REFERENCES Payments(Id);

