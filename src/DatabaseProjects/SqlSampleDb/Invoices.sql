CREATE VIEW [dbo].[Invoices]
  AS 
  SELECT 
    [Orders].[ShipName],
    [Orders].[ShipAddress],
    [Orders].[ShipCity],
    [Orders].[ShipRegion],
    [Orders].[ShipPostalCode],
    [Orders].[ShipCountry],
    [Orders].[CustomerID],
    [Customers].[CompanyName] AS CustomerName,
    [Customers].[Address],
    [Customers].[City],
    [Customers].[Region],
    [Customers].[PostalCode],
    [Customers].[Country],
    ([Employees].[FirstName] + ' ' + [Employees].[LastName]) AS Salesperson,
    [Orders].[OrderID],
    [Orders].[OrderDate],
    [Orders].[RequiredDate],
    [Orders].[ShippedDate],
    [Shippers].[CompanyName] AS ShipperName,
    [OrderDetails].[ProductID],
    [Products].[ProductName],
    [OrderDetails].[UnitPrice],
    [OrderDetails].[Quantity],
    [OrderDetails].[Discount],
    (CONVERT(MONEY, ([OrderDetails].[UnitPrice] * [OrderDetails].[Quantity] * (1 - [OrderDetails].[Discount])/100)) * 100) AS ExtendedPrice,
    [Orders].[Freight]
  FROM [dbo].[Shippers] INNER JOIN 
        ([dbo].[Products] INNER JOIN 
          (
            ([dbo].[Employees] INNER JOIN 
              ([dbo].[Customers] INNER JOIN [dbo].[Orders] ON [dbo].[Customers].[CustomerId] = [dbo].[Orders].[CustomerId])
              ON [dbo].[Employees].[EmployeeId] = [dbo].[Orders].[EmployeeId])
          INNER JOIN [dbo].[OrderDetails] ON [dbo].[Orders].[OrderId] = [dbo].[OrderDetails].[OrderId])
        ON [dbo].[Products].[ProductId] = [dbo].[OrderDetails].[ProductId])
      ON [dbo].[Shippers].[ShipperId] = [dbo].[Orders].[ShipVia]
