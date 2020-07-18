CREATE TABLE [dbo].[Rates]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY(1,1), 
    [date] DATE NOT NULL, 
    [NumCode] INT NOT NULL, 
    [CharCode] NCHAR(3) NULL, 
    [Nominal] INT NOT NULL, 
    [Name] NVARCHAR(50) NULL, 
    [Value] DECIMAL(18, 4) NOT NULL
)

GO

CREATE INDEX [IX_Rates_NumCode] ON [dbo].[Rates] ([NumCode])

GO

CREATE INDEX [IX_Rates_Date] ON [dbo].[Rates] ([date])
