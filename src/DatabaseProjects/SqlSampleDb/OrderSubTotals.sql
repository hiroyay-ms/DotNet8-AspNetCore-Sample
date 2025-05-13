CREATE VIEW [dbo].[OrderSubTotals]
  AS 
  SELECT 
    OrderId,
    SUM(CONVERT(MONEY,(UnitPrice * Quantity) * (1 - Discount))) AS SubTotal 
  FROM [dbo].[OrderDetails]
  GROUP BY OrderId
