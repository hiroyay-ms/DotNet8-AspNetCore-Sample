CREATE PROCEDURE [dbo].[EmployeeSalesByCountry]
  @Beginning_Date datetime,
  @pEnding_Date datetime
AS
  SELECT 
    [Employees].[Country],
    [Employees].[LastName],
    [Employees].[FirstName],
    [Orders].[ShippedDate],
    [OrderSubTotals].[SubTotal] AS SaleAmount
  FROM [dbo].[Employees] INNER JOIN 
        ([dbo].[Orders] INNER JOIN [OrderSubTotals] ON [dbo].[Orders].[OrderId] = [OrderSubTotals].[OrderId])
      ON [dbo].[Employees].[EmployeeId] = [dbo].[Orders].[EmployeeId]
  WHERE [dbo].[Orders].[ShippedDate] BETWEEN @Beginning_Date AND @pEnding_Date
