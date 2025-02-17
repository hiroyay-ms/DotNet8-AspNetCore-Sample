CREATE VIEW [dbo].[ProductsByCategory]
  AS 
  SELECT
    c.CategoryName,
    p.ProductName,
    p.QuantityPerUnit,
    p.UnitPrice,
    p.UnitsInStock,
    p.Discontinued 
  FROM [dbo].[Products] p INNER JOIN [dbo].[Categories] c ON p.CategoryID = c.CategoryID 
  WHERE p.Discontinued <> 1
