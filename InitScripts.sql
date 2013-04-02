USE DNNDemoDb
GO

if not exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[IgorKarpov_DocumentsExchangeModule_Folders]') and OBJECTPROPERTY(id, N'IsTable') = 1)
	BEGIN
		CREATE TABLE [dbo].[IgorKarpov_DocumentsExchangeModule_Folders]
		(
			[Id] int PRIMARY KEY IDENTITY(1, 1) NOT NULL,
			[ParentFolderId] int FOREIGN KEY REFERENCES [dbo].[IgorKarpov_DocumentsExchangeModule_Folders](Id),
			[Name] nvarchar(100) NOT NULL,
			[CreatorUserId] int NOT NULL FOREIGN KEY REFERENCES [dbo].[Users](UserID),
			[CreationDate] datetime NOT NULL
		)
	END
GO

if not exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[IgorKarpov_DocumentsExchangeModule_Files]') and OBJECTPROPERTY(id, N'IsTable') = 1)
	BEGIN
		CREATE TABLE [dbo].[IgorKarpov_DocumentsExchangeModule_Files]
		(
			[Id] int PRIMARY KEY IDENTITY(1, 1) NOT NULL,
			[ParentFolderId] int FOREIGN KEY REFERENCES [dbo].[IgorKarpov_DocumentsExchangeModule_Folders](Id),
			[OriginalName] nvarchar(100) NOT NULL,
			[ConcentType] nvarchar(100) NOT NULL
		)
	END
GO

if not exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[IgorKarpov_DocumentsExchangeModule_FileVersions]') and OBJECTPROPERTY(id, N'IsTable') = 1)
	BEGIN
		CREATE TABLE [dbo].[IgorKarpov_DocumentsExchangeModule_FileVersions]
		(
			[Id] int PRIMARY KEY IDENTITY(1, 1) NOT NULL,
			[FileId] int NOT NULL FOREIGN KEY REFERENCES [dbo].[IgorKarpov_DocumentsExchangeModule_Files](Id),
			[LocalName] nvarchar(50) NOT NULL,
			[CreatorUserId] int NOT NULL FOREIGN KEY REFERENCES [dbo].[Users](UserID),
			[CreationDate] datetime NOT NULL
		)
	END
GO