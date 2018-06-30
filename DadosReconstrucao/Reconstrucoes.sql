CREATE TABLE [dbo].[Reconstrucoes]
(
	[Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY, 
    [Informacoes] VARCHAR(50) NULL, 
	[Arquivo]VARCHAR(50) NOT NULL,
    [DataHora] DATETIME NOT NULL
)
