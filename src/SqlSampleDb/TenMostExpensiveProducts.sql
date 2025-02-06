CREATE PROCEDURE [dbo].[TenMostExpensiveProducts]
AS
  SET ROWCOUNT 10
  SELECT
    ProductName,
    UnitPrice
  FROM [dbo].[Products]
  ORDER BY UnitPrice DESC
