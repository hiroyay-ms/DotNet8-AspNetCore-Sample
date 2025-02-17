CREATE PROCEDURE [dbo].[SalesByYear]
  @Beginning_Date datetime,
  @Ending_Date datetime
AS
  SELECT 
    [Orders].[ShippedDate],
    [Orders].[OrderId],
    [OrderSubTotals].[SubTotal],
    DATENAME(yy, [Orders].[ShippedDate]) AS ShippedYear 
  FROM [dbo].[Orders] INNER JOIN [dbo].[OrderSubTotals] ON [dbo].[Orders].[OrderId] = [dbo].[OrderSubTotals].[OrderId]
  WHERE [dbo].[Orders].[ShippedDate] BETWEEN @Beginning_Date AND @Ending_Date
