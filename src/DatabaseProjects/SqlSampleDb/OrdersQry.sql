CREATE VIEW [dbo].[OrdersQry]
  AS 
  SELECT
    o.OrderId,
    o.CustomerId,
    o.EmployeeId,
    o.OrderDate,
    o.RequiredDate,
    o.ShippedDate,
    o.ShipVia,
    o.Freight,
    o.ShipName,
    o.ShipAddress,
    o.ShipCity,
    o.ShipRegion,
    o.ShipPostalCode,
    o.ShipCountry,
    c.CompanyName,
    c.Address,
    c.City,
    c.Region,
    c.PostalCode,
    c.Country
  FROM [dbo].[Customers] c INNER JOIN [dbo].[Orders] o ON c.CustomerID = o.CustomerID
