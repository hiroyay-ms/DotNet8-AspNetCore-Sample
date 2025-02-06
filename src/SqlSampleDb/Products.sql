CREATE TABLE [dbo].[Products]
(
  [ProductId]       INT IDENTITY(1, 1) NOT NULL,
  [ProductName]     NVARCHAR(40) NOT NULL,
  [SupplierId]      INT NULL,
  [CategoryId]      INT NULL,
  [QuantityPerUnit] NVARCHAR(20) NULL,
  [UnitPrice]       MONEY NULL CONSTRAINT "DF_Products_UnitPrice" DEFAULT (0),
  [UnitsInStock]    SMALLINT NULL CONSTRAINT "DF_Products_UnitsInStock" DEFAULT (0),
  [UnitsOnOrder]    SMALLINT NULL CONSTRAINT "DF_Products_UnitsOnOrder" DEFAULT (0),
  [ReorderLevel]    SMALLINT NULL CONSTRAINT "DF_Products_ReorderLevel" DEFAULT (0),
  [Discontinued]    BIT NOT NULL CONSTRAINT "DF_Products_Discontinued" DEFAULT (0),
  CONSTRAINT [PK_Products] PRIMARY KEY CLUSTERED ([ProductId]),
  CONSTRAINT [FK_Products_Categories] FOREIGN KEY ([CategoryId]) REFERENCES [dbo].[Categories] ([CategoryId]),
  CONSTRAINT [FK_Products_Suppliers] FOREIGN KEY ([SupplierId]) REFERENCES [dbo].[Suppliers] ([SupplierId]),
  CONSTRAINT [CK_Products_UnitPrice] CHECK ([UnitPrice] >= 0),
  CONSTRAINT [CK_Products_UnitsInStock] CHECK ([UnitsInStock] >= 0),
  CONSTRAINT [CK_Products_UnitsOnOrder] CHECK ([UnitsOnOrder] >= 0),
  CONSTRAINT [CK_Products_ReorderLevel] CHECK ([ReorderLevel] >= 0)
)
