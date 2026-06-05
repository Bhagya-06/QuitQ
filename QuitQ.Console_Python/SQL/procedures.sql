CREATE PROCEDURE SP_RegisterUser 
@Username NVARCHAR(50),@Name NVARCHAR(100),@Email NVARCHAR(150),@Password NVARCHAR(255),@Phone NVARCHAR(15),@Address NVARCHAR(500),@Role NVARCHAR(20) AS
BEGIN
    IF EXISTS (SELECT 1 FROM Users WHERE Email = @Email)
    BEGIN
        RAISERROR('Email already exists',16,1);
        RETURN;
    END

    INSERT INTO Users (Username, Name, Email, PasswordHash, Phone, Address, Role)
    VALUES (@Username, @Name, @Email, @Password, @Phone, @Address, @Role);

    SELECT SCOPE_IDENTITY() AS UserId;

    PRINT 'User registered successfully';
END;

CREATE PROCEDURE SP_LoginUser 
@Email NVARCHAR(150),@Password NVARCHAR(255) AS
BEGIN
    IF EXISTS (
        SELECT 1 FROM Users 
        WHERE Email = @Email AND PasswordHash = @Password AND IsActive = 1
    )
    BEGIN
        SELECT Id, Name, Role 
        FROM Users 
        WHERE Email = @Email;
    END
    ELSE
    BEGIN
        RAISERROR('Invalid email or password',16,1);
    END
END;

CREATE PROCEDURE SP_AddToCart 
@UserId INT,@ProductId INT,@Quantity INT AS
BEGIN
    IF EXISTS (
        SELECT 1 FROM ShoppingCart 
        WHERE UserId = @UserId AND ProductId = @ProductId
    )
    BEGIN
        UPDATE ShoppingCart
        SET Quantity = Quantity + @Quantity
        WHERE UserId = @UserId AND ProductId = @ProductId;
    END
    ELSE
    BEGIN
        INSERT INTO ShoppingCart(UserId, ProductId, Quantity)
        VALUES (@UserId, @ProductId, @Quantity);
    END
END;


CREATE PROCEDURE SP_RemoveFromCart 
@UserId INT,@ProductId INT AS
BEGIN
    DELETE FROM ShoppingCart
        WHERE UserId = @UserId AND ProductId = @ProductId;
END;


CREATE PROCEDURE SP_PlaceOrder 
@UserId INT,@AddressId INT AS
BEGIN
    BEGIN TRANSACTION;

    DECLARE @OrderId INT;
    INSERT INTO Orders(UserId, AddressId, Total)
    VALUES (@UserId, @AddressId, 0);

    SET @OrderId = SCOPE_IDENTITY();

    INSERT INTO OrderItems(OrderId, ProductId, Quantity, Price)
    SELECT @OrderId,P.Id,C.Quantity,P.Price FROM ShoppingCart C
    JOIN Products P ON C.ProductId = P.Id
    WHERE C.UserId = @UserId;

    UPDATE Orders
    SET Total = dbo.FN_GetOrderTotal(@OrderId)
    WHERE Id = @OrderId;

    DELETE FROM ShoppingCart WHERE UserId = @UserId;

    COMMIT;
END;


CREATE PROCEDURE SP_AddToWishlist 
@UserId INT,@ProductId INT AS
BEGIN
    IF NOT EXISTS (
        SELECT 1 FROM Wishlist 
        WHERE UserId = @UserId AND ProductId = @ProductId
    )
    BEGIN
        INSERT INTO Wishlist(UserId, ProductId)
        VALUES (@UserId, @ProductId);
    END
END;


CREATE PROCEDURE SP_RemoveFromWishlist @UserId INT,@ProductId INT AS
BEGIN
    DELETE FROM Wishlist
    WHERE UserId = @UserId AND ProductId = @ProductId;
END;