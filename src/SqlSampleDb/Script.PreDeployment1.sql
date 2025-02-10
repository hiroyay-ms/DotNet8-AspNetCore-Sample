IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id('EmployeeSalesByCountry'))
	DROP PROCEDURE [dbo].[EmployeeSalesByCountry]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id('SalesByYear'))
	DROP PROCEDURE [dbo].[SalesByYear]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id('TenMostExpensiveProducts'))
	DROP PROCEDURE [dbo].[TenMostExpensiveProducts]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id('SalesByCategory'))
	DROP VIEW [dbo].[SalesByCategory]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id('SalesTotalsByAmount'))
	DROP VIEW [dbo].[SalesTotalsByAmount]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id('ProductsByCategory'))
	DROP VIEW [dbo].[ProductsByCategory]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id('OrderSubTotals'))
	DROP VIEW [dbo].[OrderSubTotals]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id('OrdersQry'))
	DROP VIEW [dbo].[OrdersQry]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id('OrderDetailsExtended'))
	DROP VIEW [dbo].[OrderDetailsExtended]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id('Invoices'))
	DROP VIEW [dbo].[Invoices]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id('CustomerAndSuppliersByCity'))
	DROP VIEW [dbo].[CustomerAndSuppliersByCity]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id('OrderDetails'))
	DROP TABLE [dbo].[OrderDetails]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id('Orders'))
	DROP TABLE [dbo].[Orders]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id('Products'))
	DROP TABLE [dbo].[Products]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id('Categories'))
	DROP TABLE [dbo].[Categories]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id('Customers'))
	DROP TABLE [dbo].[Customers]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id('Shippers'))
	DROP TABLE [dbo].[Shippers]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id('Suppliers'))
	DROP TABLE [dbo].[Suppliers]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id('Employees'))
	DROP TABLE [dbo].[Employees]
GO
