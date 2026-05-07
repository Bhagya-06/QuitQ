CREATE VIEW VW_ProductCatalog AS
SELECT P.Id,P.Name,P.Price,P.Stock,C.Name AS Category,B.Name AS Brand
FROM Products P
JOIN Categories C ON P.CategoryId = C.Id
LEFT JOIN Brands B ON P.BrandId = B.Id
WHERE P.IsActive = 1;

CREATE VIEW VW_OrderSummary AS
SELECT O.Id,U.Name AS Customer,O.Total,O.Status,O.CreatedDate
FROM Orders O
JOIN Users U ON O.UserId = U.Id;