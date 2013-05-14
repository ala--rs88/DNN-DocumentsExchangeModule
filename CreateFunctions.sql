USE DNNDemoDb
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[IgorKarpov_DocumentsExchangeModule_GetFileLastVersionDate]') and OBJECTPROPERTY(id, N'IsFunction') = 1)
    DROP FUNCTION [dbo].[IgorKarpov_DocumentsExchangeModule_GetFileLastVersionDate];
GO
CREATE FUNCTION [dbo].[IgorKarpov_DocumentsExchangeModule_GetFileLastVersionDate] (@fileId int)
RETURNS DateTime
AS
BEGIN
declare @creationDate DateTime
SELECT TOP (1) @creationDate = [CreationDate] 
	FROM [dbo].[IgorKarpov_DocumentsExchangeModule_FileVersions]
	WHERE [FileId] = @fileId
	ORDER BY [CreationDate] DESC
RETURN @creationDate
END
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[IgorKarpov_DocumentsExchangeModule_GetFileLastVersionId]') and OBJECTPROPERTY(id, N'IsFunction') = 1)
    DROP FUNCTION [dbo].[IgorKarpov_DocumentsExchangeModule_GetFileLastVersionId];
GO
CREATE FUNCTION [dbo].[IgorKarpov_DocumentsExchangeModule_GetFileLastVersionId] (@fileId int)
RETURNS int
AS
BEGIN
declare @lastVersionId int
SELECT TOP (1) @lastVersionId = [Id] 
	FROM [dbo].[IgorKarpov_DocumentsExchangeModule_FileVersions]
	WHERE [FileId] = @fileId
	ORDER BY [CreationDate] DESC
RETURN @lastVersionId
END
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[IgorKarpov_DocumentsExchangeModule_GetFileLastVersionLocalName]') and OBJECTPROPERTY(id, N'IsFunction') = 1)
    DROP FUNCTION [dbo].[IgorKarpov_DocumentsExchangeModule_GetFileLastVersionLocalName];
GO
CREATE FUNCTION [dbo].[IgorKarpov_DocumentsExchangeModule_GetFileLastVersionLocalName] (@fileId int)
RETURNS nvarchar(50)
AS
BEGIN
declare @localName nvarchar(50)
SELECT TOP (1) @localName = [LocalName] 
	FROM [dbo].[IgorKarpov_DocumentsExchangeModule_FileVersions]
	WHERE [FileId] = @fileId
	ORDER BY [CreationDate] DESC
RETURN @localName
END
GO

--////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[IgorKarpov_DocumentsExchangeModule_GetFileContentType]') and OBJECTPROPERTY(id, N'IsFunction') = 1)
    DROP FUNCTION [dbo].[IgorKarpov_DocumentsExchangeModule_GetFileContentType];
GO
CREATE FUNCTION [dbo].[IgorKarpov_DocumentsExchangeModule_GetFileContentType] (@fileId int)
RETURNS nvarchar(100)
AS
BEGIN
declare @contentType nvarchar(100)
SELECT TOP (1) @contentType = [ContentType] 
	FROM [dbo].[IgorKarpov_DocumentsExchangeModule_Files]
	WHERE [Id] = @fileId
	ORDER BY [CreationDate] DESC
RETURN @contentType
END
GO

--///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[IgorKarpov_DocumentsExchangeModule_GetOriginalFileName]') and OBJECTPROPERTY(id, N'IsFunction') = 1)
    DROP FUNCTION [dbo].[IgorKarpov_DocumentsExchangeModule_GetOriginalFileName];
GO
CREATE FUNCTION [dbo].[IgorKarpov_DocumentsExchangeModule_GetOriginalFileName] (@fileId int)
RETURNS nvarchar(max)
AS
BEGIN
declare @originalFileName nvarchar(max)
SELECT TOP (1) @originalFileName = [OriginalName] 
	FROM [dbo].[IgorKarpov_DocumentsExchangeModule_Files]
	WHERE [Id] = @fileId
	ORDER BY [CreationDate] DESC
RETURN @originalFileName
END
GO

--///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[IgorKarpov_DocumentsExchangeModule_GetVersionLocalName]') and OBJECTPROPERTY(id, N'IsFunction') = 1)
    DROP FUNCTION [dbo].[IgorKarpov_DocumentsExchangeModule_GetVersionLocalName];
GO
CREATE FUNCTION [dbo].[IgorKarpov_DocumentsExchangeModule_GetVersionLocalName] (@versionId int)
RETURNS nvarchar(50)
AS
BEGIN
declare @localName nvarchar(50)
SELECT @localName = [LocalName] 
	FROM [dbo].[IgorKarpov_DocumentsExchangeModule_FileVersions]
	WHERE [Id] = @versionId
RETURN @localName
END
GO

--/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[IgorKarpov_DocumentsExchangeModule_IsFileNameLocallyAvailable]') and OBJECTPROPERTY(id, N'IsFunction') = 1)
    DROP FUNCTION [dbo].[IgorKarpov_DocumentsExchangeModule_IsOriginalFileNameLocallyAvailable];
GO
CREATE FUNCTION [dbo].[IgorKarpov_DocumentsExchangeModule_IsOriginalFileNameLocallyAvailable]
	(@parentFolderId int, @targetFileName nvarchar(100))
RETURNS bit
AS
BEGIN
declare @matchesCount int
declare @isAvailable bit
SELECT @matchesCount = COUNT(*) 
	FROM [dbo].[IgorKarpov_DocumentsExchangeModule_Files]
	WHERE (@parentFolderId IS NOT NULL and [ParentFolderId] = @parentFolderId
			or @parentFolderId IS NULL and [ParentFolderId] IS NULL)
	  and [OriginalName] = @targetFileName
IF @matchesCount > 0
	set @isAvailable = 0
ELSE
	set @isAvailable = 1
RETURN @isAvailable
END
GO


if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[IgorKarpov_DocumentsExchangeModule_IsFolderNameLocallyAvailable]') and OBJECTPROPERTY(id, N'IsFunction') = 1)
    DROP FUNCTION [dbo].[IgorKarpov_DocumentsExchangeModule_IsFolderNameLocallyAvailable];
GO
CREATE FUNCTION [dbo].[IgorKarpov_DocumentsExchangeModule_IsFolderNameLocallyAvailable]
	(@parentFolderId int, @targetFolderName nvarchar(100))
RETURNS bit
AS
BEGIN
declare @matchesCount int
declare @isAvailable bit
SELECT @matchesCount = COUNT(*) 
	FROM [dbo].[IgorKarpov_DocumentsExchangeModule_Folders]
	WHERE (@parentFolderId IS NOT NULL and [ParentFolderId] = @parentFolderId
			or @parentFolderId IS NULL and [ParentFolderId] IS NULL)
	  and [Name] = @targetFolderName
IF @matchesCount > 0
	set @isAvailable = 0
ELSE
	set @isAvailable = 1
RETURN @isAvailable
END
GO
