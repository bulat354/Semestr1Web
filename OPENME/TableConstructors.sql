CREATE DATABASE GavnoGamesDB
GO

USE GavnoGamesDB
GO

CREATE TABLE [dbo].[Accounts] (
    [Id]       INT           IDENTITY (1, 1) NOT NULL,
    [Email]    VARCHAR (200) NOT NULL,
    [Password] VARCHAR (50)  NOT NULL,
    [Name]     VARCHAR (50)  NOT NULL,
    [Gender]   VARCHAR (20)  NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    UNIQUE NONCLUSTERED ([Email] ASC)
)
GO

CREATE TABLE [dbo].[Images] (
    [Id]       INT           IDENTITY (1, 1) NOT NULL,
    [Name]     VARCHAR (100) NOT NULL,
    [ColorHex] VARCHAR (20)  NOT NULL,
    [Alt]      VARCHAR (100) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    UNIQUE NONCLUSTERED ([Name] ASC)
)
GO

CREATE TABLE [dbo].[Games] (
    [Id]          INT            IDENTITY (1, 1) NOT NULL,
    [Title]       NVARCHAR (100) NOT NULL,
    [Description] NTEXT          NOT NULL,
    [Price]       MONEY          NOT NULL,
    [ImageId]     INT            DEFAULT ((0)) NOT NULL,
    [Date]        DATETIME       NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    FOREIGN KEY ([ImageId]) REFERENCES [dbo].[Images] ([Id]) ON DELETE SET DEFAULT ON UPDATE CASCADE
)
GO

CREATE TABLE [dbo].[Articles] (
    [Id]      INT            IDENTITY (1, 1) NOT NULL,
    [Title]   NVARCHAR (100) NOT NULL,
    [Content] NTEXT          NOT NULL,
    [Date]    DATETIME       NOT NULL,
    [ImageId] INT            DEFAULT ((0)) NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    FOREIGN KEY ([ImageId]) REFERENCES [dbo].[Images] ([Id]) ON DELETE SET DEFAULT ON UPDATE CASCADE
)
GO

CREATE TABLE [dbo].[Comments] (
    [Id]        INT      IDENTITY (1, 1) NOT NULL,
    [Content]   NTEXT    NOT NULL,
    [Date]      DATETIME NOT NULL,
    [AccountId] INT      NOT NULL,
    [ArticleId] INT      NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    FOREIGN KEY ([AccountId]) REFERENCES [dbo].[Accounts] ([Id]) ON DELETE CASCADE ON UPDATE CASCADE,
    FOREIGN KEY ([ArticleId]) REFERENCES [dbo].[Articles] ([Id]) ON DELETE CASCADE ON UPDATE CASCADE
)
GO