IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id('OrderDetails'))
BEGIN
	ALTER TABLE [dbo].[OrderDetails] NOCHECK CONSTRAINT ALL

	DELETE FROM [dbo].[OrderDetails]

	ALTER TABLE [dbo].[OrderDetails] CHECK CONSTRAINT ALL
END


IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id('Orders'))
BEGIN
	ALTER TABLE [dbo].[Orders] NOCHECK CONSTRAINT ALL

	DELETE FROM [dbo].[Orders]

	ALTER TABLE [dbo].[Orders] CHECK CONSTRAINT ALL
END
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id('Customers'))
	DELETE FROM [dbo].[Customers]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id('Employees'))
BEGIN
	ALTER TABLE [dbo].[Employees] NOCHECK CONSTRAINT ALL

	DELETE FROM [dbo].[Employees]

	ALTER TABLE [dbo].[Employees] CHECK CONSTRAINT ALL
END
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id('Products'))
BEGIN
	ALTER TABLE [dbo].[Products] NOCHECK CONSTRAINT ALL

	DELETE FROM [dbo].[Products]

	ALTER TABLE [dbo].[Products] CHECK CONSTRAINT ALL
END
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id('Categories'))
	DELETE FROM [dbo].[Categories]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id('Suppliers'))
	DELETE FROM [dbo].[Suppliers]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id('Shippers'))
	DELETE FROM [dbo].[Shippers]
GO
