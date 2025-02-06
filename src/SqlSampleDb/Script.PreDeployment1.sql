IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id('dbo.SalesByYear') AND sysstat & 0xf = 4)
	DROP PROCEDURE [dbo].[SalesByYear]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id('dbo.TenMostExpensiveProducts') AND sysstat & 0xf = 4)
	DROP PROCEDURE [dbo].[TenMostExpensiveProducts]
GO
