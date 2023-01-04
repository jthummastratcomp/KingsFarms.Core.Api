IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
GO

CREATE TABLE [Customers] (
    [Id] int NOT NULL IDENTITY,
    [Key] nvarchar(20) NOT NULL,
    [Name] nvarchar(max) NULL,
    [Address] nvarchar(max) NULL,
    [City] nvarchar(max) NULL,
    [Zip] nvarchar(max) NULL,
    [StoreName] nvarchar(max) NULL,
    [StorePhone] nvarchar(max) NULL,
    [OtherPhone] nvarchar(max) NULL,
    [ContactName] nvarchar(max) NULL,
    [ContactPhone] nvarchar(max) NULL,
    CONSTRAINT [PK_Customers] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [Invoices] (
    [Id] int NOT NULL IDENTITY,
    [InvoiceNumber] nvarchar(50) NOT NULL,
    [Description] nvarchar(max) NULL,
    [InvoiceDate] datetime2 NOT NULL,
    [Quantity] decimal(14,2) NOT NULL,
    [Rate] decimal(14,2) NOT NULL,
    [Amount] decimal(14,2) NOT NULL,
    [ShippingCharge] decimal(14,2) NULL,
    [DiscountAmount] decimal(14,2) NULL,
    [DueDate] datetime2 NULL,
    [Memo] nvarchar(max) NULL,
    [CustomerId] int NOT NULL,
    CONSTRAINT [PK_Invoices] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Invoices_Customers_CustomerId] FOREIGN KEY ([CustomerId]) REFERENCES [Customers] ([Id]) ON DELETE CASCADE
);
GO

CREATE INDEX [IX_Invoices_CustomerId] ON [Invoices] ([CustomerId]);
GO

CREATE UNIQUE INDEX [IX_Invoices_InvoiceNumber] ON [Invoices] ([InvoiceNumber]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20230103002925_InitialCreate', N'7.0.1');
GO

COMMIT;
GO

