USE DNNDemoDb
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[IgorKarpov_DocumentsExchangeModule_GetFolders]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
	drop procedure [dbo].[IgorKarpov_DocumentsExchangeModule_GetFolders]
GO

create procedure [dbo].[IgorKarpov_DocumentsExchangeModule_GetFolders]
	@ParentFolderId int
as
select Id,
	   ParentFolderId,
	   Name,
	   CreatorUserId,
	   folders.CreationDate,
	   'CreatorDisplayName' = [Users].DisplayName
from [IgorKarpov_DocumentsExchangeModule_Folders] folders
inner join [Users] on folders.[CreatorUserId] = [Users].[UserId]
where (@ParentFolderId IS NOT NULL and [ParentFolderId] = @ParentFolderId)
   or (@ParentFolderId IS NULL and [ParentFolderId] IS NULL)
GO

--///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[IgorKarpov_DocumentsExchangeModule_GetFiles]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
	drop procedure [dbo].[IgorKarpov_DocumentsExchangeModule_GetFiles]
GO

create procedure [dbo].[IgorKarpov_DocumentsExchangeModule_GetFiles]
	@ParentFolderId int
as
select files.Id,
	   ParentFolderId,
	   OriginalName,
	   ContentType,
	   files.CreatorUserId,
	   files.CreationDate,
	   'CreatorDisplayName' = [Users].DisplayName,
	   'LastVersionDate' = [dbo].[IgorKarpov_DocumentsExchangeModule_GetFileLastVersionDate](files.Id),
	   'LastVersionId' = [dbo].[IgorKarpov_DocumentsExchangeModule_GetFileLastVersionId](files.Id)
from [IgorKarpov_DocumentsExchangeModule_Files] files
inner join [Users] on files.[CreatorUserId] = [Users].[UserId]
where (@ParentFolderId IS NOT NULL and [ParentFolderId] = @ParentFolderId)
   or (@ParentFolderId IS NULL and [ParentFolderId] IS NULL)
GO

--//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[IgorKarpov_DocumentsExchangeModule_GetVersions]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
	drop procedure [dbo].[IgorKarpov_DocumentsExchangeModule_GetVersions]
GO

create procedure [dbo].[IgorKarpov_DocumentsExchangeModule_GetVersions]
	@FileId int
as
select [Id],
	   [FileId],
	   [LocalName],
	   [Comment],
	   [CreatorUserId],
	   [CreationDate],
	   'CreatorDisplayName' = [Users].[DisplayName]
from [IgorKarpov_DocumentsExchangeModule_FileVersions] versions
inner join [Users] on versions.[CreatorUserId] = [Users].[UserId]
where versions.[FileId] = @FileId
GO

--//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[IgorKarpov_DocumentsExchangeModule_AddFolder]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
	drop procedure [dbo].[IgorKarpov_DocumentsExchangeModule_AddFolder]
GO

create procedure [dbo].[IgorKarpov_DocumentsExchangeModule_AddFolder]
	@parentFolderId int,
	@name nvarchar(MAX),
	@creatorUserId int
as
insert into [IgorKarpov_DocumentsExchangeModule_Folders] (
	ParentFolderId,
	Name,
	CreatorUserId,
	CreationDate
) 
values (
	@parentFolderId,
	@name,
	@creatorUserId,
	GETDATE()
)
GO


----------------------------------------------------------------------------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[IgorKarpov_DocumentsExchangeModule_AddNewFile]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
	drop procedure [dbo].[IgorKarpov_DocumentsExchangeModule_AddNewFile]
GO

create procedure [dbo].[IgorKarpov_DocumentsExchangeModule_AddNewFile]
	@parentFolderId int,
	@originalName nvarchar(MAX),
	@contentType nvarchar(100),
	@creatorUserId int,
	@localFileName nvarchar(50)
as
BEGIN
	set xact_abort on
	 BEGIN TRAN
		declare @insertedFileIdTable table (insertedId int)
		
		INSERT INTO [dbo].[IgorKarpov_DocumentsExchangeModule_Files] (
			ParentFolderId,
			OriginalName,
			ContentType,
			CreatorUserId,
			CreationDate)
			OUTPUT INSERTED.ID INTO @insertedFileIdTable(insertedId)
		values (
			@parentFolderId,
			@originalName,
			@contentType,
			@creatorUserId,
			GETDATE()
		)
		
		declare @insertedFileId int
		SELECT TOP(1) @insertedFileId = insertedId FROM @insertedFileIdTable
		
		INSERT INTO [dbo].[IgorKarpov_DocumentsExchangeModule_FileVersions] (
			FileId,
			LocalName,
			Comment,
			CreatorUserId,
			CreationDate
		)
		values (
			@insertedFileId,
			@localFileName,
			'Initial version',
			@creatorUserId,
			GETDATE()
		)
	 COMMIT TRAN
END
GO
--///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[IgorKarpov_DocumentsExchangeModule_AddVersion]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
	drop procedure [dbo].[IgorKarpov_DocumentsExchangeModule_AddVersion]
GO
create procedure [dbo].[IgorKarpov_DocumentsExchangeModule_AddVersion]
	@fileId int,
	@localFileName nvarchar(50),
	@comment nvarchar(max),
	@creatorUserId int
as
BEGIN
	INSERT INTO [dbo].[IgorKarpov_DocumentsExchangeModule_FileVersions] (
		[FileId],
		[LocalName],
		[Comment],
		[CreatorUserId],
		[CreationDate]
	)
	values (
		@fileId,
		@localFileName,
		@comment,
		@creatorUserId,
		GETDATE()
	)
END
GO

--//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[IgorKarpov_DocumentsExchangeModule_HideFolder]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
	drop procedure [dbo].[IgorKarpov_DocumentsExchangeModule_HideFolder]
GO
create procedure [dbo].[IgorKarpov_DocumentsExchangeModule_HideFolder]
	@folderId int
as
BEGIN
	UPDATE [dbo].[IgorKarpov_DocumentsExchangeModule_Folders] 
		SET ParentFolderId = -1
		WHERE Id = @folderId
END
GO

--//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[IgorKarpov_DocumentsExchangeModule_HideFile]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
	drop procedure [dbo].[IgorKarpov_DocumentsExchangeModule_HideFile]
GO
create procedure [dbo].[IgorKarpov_DocumentsExchangeModule_HideFile]
	@fileId int
as
BEGIN
	UPDATE [dbo].[IgorKarpov_DocumentsExchangeModule_Files] 
		SET ParentFolderId = -1
		WHERE Id = @fileId
END
GO

--//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[IgorKarpov_DocumentsExchangeModule_HideVersion]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
	drop procedure [dbo].[IgorKarpov_DocumentsExchangeModule_HideVersion]
GO
create procedure [dbo].[IgorKarpov_DocumentsExchangeModule_HideVersion]
	@versionId int
as
BEGIN
	UPDATE [dbo].[IgorKarpov_DocumentsExchangeModule_FileVersions] 
		SET FileId = -1
		WHERE Id = @versionId
END
GO