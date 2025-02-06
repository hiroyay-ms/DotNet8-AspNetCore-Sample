CREATE VIEW [dbo].[SalesTotalsByAmount]
  AS 
  SELECT 
    [OrderSubTotals].[SubTotal] AS SalesAmount,
    [Orders].[OrderId],
    [Customers].[CompanyName],
    [Orders].[ShippedDate] 
  FROM [dbo].[Customers] INNER JOIN 
        ([dbo].[Orders] INNER JOIN [dbo].[OrderSubTotals] ON [dbo].[Orders].[OrderId] = [dbo].[OrderSubTotals].[OrderId])
      ON [dbo].[Customers].[CustomerId] = [dbo].[Orders].[CustomerId]
