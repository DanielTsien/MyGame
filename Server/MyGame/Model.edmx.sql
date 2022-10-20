
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 10/20/2022 11:03:04
-- Generated from EDMX file: D:\MyGame\Server\MyGame\Model.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [MyGame];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_TUserTPlayer]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[TPlayers] DROP CONSTRAINT [FK_TUserTPlayer];
GO
IF OBJECT_ID(N'[dbo].[FK_TPlayerTCharacter]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[TCharacters] DROP CONSTRAINT [FK_TPlayerTCharacter];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[TCharacters]', 'U') IS NOT NULL
    DROP TABLE [dbo].[TCharacters];
GO
IF OBJECT_ID(N'[dbo].[TPlayers]', 'U') IS NOT NULL
    DROP TABLE [dbo].[TPlayers];
GO
IF OBJECT_ID(N'[dbo].[TUsers]', 'U') IS NOT NULL
    DROP TABLE [dbo].[TUsers];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'TCharacters'
CREATE TABLE [dbo].[TCharacters] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [Class] int  NOT NULL,
    [Level] int  NOT NULL,
    [ConfigId] int  NOT NULL,
    [MapId] int  NOT NULL,
    [PosX] int  NOT NULL,
    [PosY] int  NOT NULL,
    [PosZ] int  NOT NULL,
    [TPlayer_Id] int  NOT NULL
);
GO

-- Creating table 'TPlayers'
CREATE TABLE [dbo].[TPlayers] (
    [Id] int  NOT NULL,
    [TUser_Id] int  NOT NULL
);
GO

-- Creating table 'TUsers'
CREATE TABLE [dbo].[TUsers] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Username] nvarchar(max)  NOT NULL,
    [Password] nvarchar(max)  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'TCharacters'
ALTER TABLE [dbo].[TCharacters]
ADD CONSTRAINT [PK_TCharacters]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'TPlayers'
ALTER TABLE [dbo].[TPlayers]
ADD CONSTRAINT [PK_TPlayers]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'TUsers'
ALTER TABLE [dbo].[TUsers]
ADD CONSTRAINT [PK_TUsers]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [TUser_Id] in table 'TPlayers'
ALTER TABLE [dbo].[TPlayers]
ADD CONSTRAINT [FK_TUserTPlayer]
    FOREIGN KEY ([TUser_Id])
    REFERENCES [dbo].[TUsers]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_TUserTPlayer'
CREATE INDEX [IX_FK_TUserTPlayer]
ON [dbo].[TPlayers]
    ([TUser_Id]);
GO

-- Creating foreign key on [TPlayer_Id] in table 'TCharacters'
ALTER TABLE [dbo].[TCharacters]
ADD CONSTRAINT [FK_TPlayerTCharacter]
    FOREIGN KEY ([TPlayer_Id])
    REFERENCES [dbo].[TPlayers]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_TPlayerTCharacter'
CREATE INDEX [IX_FK_TPlayerTCharacter]
ON [dbo].[TCharacters]
    ([TPlayer_Id]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------