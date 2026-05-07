CREATE FUNCTION FN_GetOrderTotal(@OrderId INT) RETURNS DECIMAL(12,2) AS
BEGIN
    DECLARE @Total DECIMAL(12,2);

    SELECT @Total = SUM(Quantity * Price)
    FROM OrderItems
    WHERE OrderId = @OrderId;

    RETURN ISNULL(@Total,0);
END;

