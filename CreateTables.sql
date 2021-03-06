USE DNNDemoDb
GO

if not exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[IgorKarpov_DocumentsExchangeModule_Folders]') and OBJECTPROPERTY(id, N'IsTable') = 1)
	BEGIN
		CREATE TABLE [dbo].[IgorKarpov_DocumentsExchangeModule_Folders]
		(
			[Id] int PRIMARY KEY IDENTITY(1, 1) NOT NULL,
			[ParentFolderId] int,
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
			[ParentFolderId] int,
			[OriginalName] nvarchar(MAX) NOT NULL,
			[ContentType] nvarchar(100) NOT NULL,
			[CreatorUserId] int NOT NULL FOREIGN KEY REFERENCES [dbo].[Users](UserID),
			[CreationDate] datetime NOT NULL
		)
	END
GO

if not exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[IgorKarpov_DocumentsExchangeModule_FileVersions]') and OBJECTPROPERTY(id, N'IsTable') = 1)
	BEGIN
		CREATE TABLE [dbo].[IgorKarpov_DocumentsExchangeModule_FileVersions]
		(
			[Id] int PRIMARY KEY IDENTITY(1, 1) NOT NULL,
			[FileId] int NOT NULL,
			[LocalName] nvarchar(50) NOT NULL,
			[Comment] nvarchar(MAX),
			[CreatorUserId] int NOT NULL FOREIGN KEY REFERENCES [dbo].[Users](UserID),
			[CreationDate] datetime NOT NULL
		)
	END
GO

if not exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[IgorKarpov_DocumentsExchangeModule_Schedules]') and OBJECTPROPERTY(id, N'IsTable') = 1)
	BEGIN
		CREATE TABLE [dbo].[IgorKarpov_DocumentsExchangeModule_Schedules]
		(
			[Id] int PRIMARY KEY IDENTITY(1, 1) NOT NULL,
			[UserId] int NOT NULL,
			[LastModificationDate] datetime NOT NULL,
			[Mon] nvarchar(MAX),
			[Tue] nvarchar(MAX),
			[Wed] nvarchar(MAX),
			[Thu] nvarchar(MAX),
			[Fri] nvarchar(MAX),
			[Sat] nvarchar(MAX),
			[Sun] nvarchar(MAX)
		)
	END
GO




INSERT INTO [dbo].[IgorKarpov_DocumentsExchangeModule_Folders] ([Name], [CreatorUserId], [CreationDate])
SELECT 'FullFolder1', 1, GETDATE()
UNION ALL
SELECT 'EmptyFolder2', 1, GETDATE()
UNION ALL
SELECT 'EmptyFolder3', 1, GETDATE()
	

INSERT INTO [dbo].[IgorKarpov_DocumentsExchangeModule_Files] ([OriginalName], [ParentFolderId], [ContentType], [CreatorUserId], [CreationDate])
SELECT 'RootFile1', NULL, 'n/a', 1, GETDATE()
UNION ALL
SELECT 'RootFile2', NULL, 'n/a', 1, GETDATE()
UNION ALL
SELECT 'RootFile3', NULL, 'n/a', 1, GETDATE()	
UNION ALL
SELECT 'Subfile3', 1, 'n/a', 1, GETDATE()	









