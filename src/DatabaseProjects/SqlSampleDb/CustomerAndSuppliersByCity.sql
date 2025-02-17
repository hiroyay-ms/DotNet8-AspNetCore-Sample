CREATE VIEW [dbo].[CustomerAndSuppliersByCity]
  AS 
  SELECT City, CompanyName, ContactName, 'Customers' AS Relationship
  FROM [dbo].[Customers] 
  UNION SELECT City, CompanyName, ContactName, 'Suppliers' 
  FROM [dbo].[Suppliers]
