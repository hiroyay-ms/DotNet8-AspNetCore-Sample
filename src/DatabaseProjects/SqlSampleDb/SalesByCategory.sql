CREATE VIEW [dbo].[SalesByCategory]
  AS 
  SELECT 
    [Categories].[CategoryId],
    [Categories].[CategoryName],
    [Products].[ProductName],
    SUM([OrderDetailsExtended].[ExtendedPrice]) AS [ProductSales]
  FROM [dbo].[Categories] INNER JOIN 
        ([dbo].[Products] INNER JOIN 
          ([dbo].[Orders] INNER JOIN [dbo].[OrderDetailsExtended] ON [dbo].[Orders].[OrderId] = [dbo].[OrderDetailsExtended].[OrderId])
        ON [dbo].[Products].[ProductId] = [dbo].[OrderDetailsExtended].[ProductId])
      ON [dbo].[Categories].[CategoryId] = [dbo].[Products].[CategoryId]
  GROUP BY [dbo].[Categories].[CategoryId], [dbo].[Categories].[CategoryName], [dbo].[Products].[ProductName]
