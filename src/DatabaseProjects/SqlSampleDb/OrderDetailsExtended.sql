CREATE VIEW [dbo].[OrderDetailsExtended]
  AS 
  SELECT 
    od.OrderId,
    od.ProductId,
    p.ProductName,
    od.UnitPrice,
    od.Quantity,
    od.Discount,
    (CONVERT(MONEY, od.UnitPrice) * od.Quantity) * (1 - od.Discount) AS ExtendedPrice
  FROM [dbo].[Products] p INNER JOIN [dbo].[OrderDetails] od ON p.ProductID = od.ProductID
