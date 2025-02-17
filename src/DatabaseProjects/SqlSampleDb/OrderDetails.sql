CREATE TABLE [dbo].[OrderDetails]
(
  [OrderId] INT NOT NULL,
  [ProductId] INT NOT NULL,
  [UnitPrice] MONEY NOT NULL CONSTRAINT "DF_OrderDetails_UnitPrice" DEFAULT (0),
  [Quantity] SMALLINT NOT NULL CONSTRAINT "DF_OrderDetails_Quantity" DEFAULT (1),
  [Discount] REAL NOT NULL CONSTRAINT "DF_OrderDetails_Discount" DEFAULT (0),
  CONSTRAINT [PK_Order_Details] PRIMARY KEY CLUSTERED ([OrderId], [ProductId]),
  CONSTRAINT [FK_Order_Details_Orders] FOREIGN KEY ([OrderId]) REFERENCES [dbo].[Orders] ([OrderId]),
  CONSTRAINT [FK_Order_Details_Products] FOREIGN KEY ([ProductId]) REFERENCES [dbo].[Products] ([ProductId]),
  CONSTRAINT [CK_Order_Details_UnitPrice] CHECK ([UnitPrice] >= 0),
  CONSTRAINT [CK_Order_Details_Quantity] CHECK ([Quantity] > 0),
  CONSTRAINT [CK_Order_Details_Discount] CHECK ([Discount] >= 0 and [Discount] <= 1)
)
