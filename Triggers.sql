CREATE TRIGGER TRG_CheckStock ON OrderItems
INSTEAD OF INSERT
AS
BEGIN
    IF EXISTS (
        SELECT 1
        FROM inserted I
        JOIN Products P ON I.ProductId = P.Id
        WHERE P.Stock < I.Quantity
    )
    BEGIN
        RAISERROR ('Insufficient Stock',16,1);
        RETURN;
    END;

    INSERT INTO OrderItems(OrderId, ProductId, Quantity, Price)
    SELECT OrderId, ProductId, Quantity, Price FROM inserted;
END;

CREATE TRIGGER TRG_ReduceStock ON OrderItems
AFTER INSERT
AS
BEGIN
    UPDATE P
    SET P.Stock = P.Stock - I.Quantity
    FROM Products P
    JOIN inserted I ON P.Id = I.ProductId;
END;

CREATE TRIGGER TRG_UpdateSellerRating ON ShopReviews
AFTER INSERT
AS
BEGIN
    UPDATE S
    SET ShopReviews = (
        SELECT AVG(Rating)
        FROM ShopReviews
        WHERE SellerId = S.Id
    )
    FROM Sellers S
    JOIN inserted I ON S.Id = I.SellerId;
END;