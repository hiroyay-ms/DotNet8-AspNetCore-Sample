CREATE TABLE [dbo].[Orders]
(
  [OrderId]         INT IDENTITY(1, 1) NOT NULL,
  [CustomerId]      NCHAR(5) NULL,
  [EmployeeId]      INT NULL,
  [OrderDate]       DATETIME NULL,
  [RequiredDate]    DATETIME NULL,
  [ShippedDate]     DATETIME NULL,
  [ShipVia]         INT NULL,
  [Freight]         MONEY NULL CONSTRAINT "DF_Orders_Freight" DEFAULT (0),
  [ShipName]        NVARCHAR(40) NULL,
  [ShipAddress]     NVARCHAR(60) NULL,
  [ShipCity]        NVARCHAR(15) NULL,
  [ShipRegion]      NVARCHAR(15) NULL,
  [ShipPostalCode]  NVARCHAR(10) NULL,
  [ShipCountry] NVARCHAR(15) NULL,
  CONSTRAINT [PK_Orders] PRIMARY KEY CLUSTERED ([OrderId]),
  CONSTRAINT [FK_Orders_Customers] FOREIGN KEY ([CustomerId]) REFERENCES [dbo].[Customers] ([CustomerId]),
  CONSTRAINT [FK_Orders_Employees] FOREIGN KEY ([EmployeeId]) REFERENCES [dbo].[Employees] ([EmployeeId]),
  CONSTRAINT [FK_Orders_Shippers] FOREIGN KEY ([ShipVia]) REFERENCES [dbo].[Shippers] ([ShipperId])
)
