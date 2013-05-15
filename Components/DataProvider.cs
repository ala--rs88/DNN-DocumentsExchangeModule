
using System;
using System.Data;
using DotNetNuke;
using IgorKarpov.DocumentsExchangeModule.Components.Entities;

namespace IgorKarpov.Modules.DocumentsExchangeModule
{

    /// ----------------------------------------------------------------------------- 
    /// <summary> 
    /// An abstract class for the data access layer 
    /// </summary> 
    /// <remarks> 
    /// </remarks> 
    /// <history> 
    /// </history> 
    /// ----------------------------------------------------------------------------- 
    public abstract class DataProvider
    {

        #region "Shared/Static Methods"

        /// <summary>
        /// singleton reference to the instantiated object 
        /// </summary>
        private static DataProvider objProvider = null;

        /// <summary>
        /// constructor
        /// </summary>
        static DataProvider()
        {
            CreateProvider();
        }

        /// <summary>
        /// dynamically create provider 
        /// </summary>
        private static void CreateProvider()
        {
            objProvider = (DataProvider)DotNetNuke.Framework.Reflection.CreateObject("data", "IgorKarpov.Modules.DocumentsExchangeModule", "");
        }

        /// <summary>
        /// return the provider 
        /// </summary>
        /// <returns></returns>
        public static DataProvider Instance()
        {
            return objProvider;
        }

        #endregion

        #region "Abstract methods"

        public abstract IDataReader GetFolders(int? parentFolderId);
        public abstract IDataReader GetFiles(int? parentFolderId);
        public abstract IDataReader GetVersions(int fileId);
        public abstract Schedule GetSchedule(int userId);

        public abstract void UpdateSchedule(int userId, String mon, String tue, String wed, String thu, String fri, String sat, String sun);

        public abstract String GetFileContentType(int fileId);
        public abstract String GetOriginalFileName(int fileId);
        public abstract String GetFileLastVersionLocalName(int fileId);
        public abstract String GetVersionLocalName(int versionId);
        public abstract int GetRelatedVersionsCount(int versionId);
        public abstract int GetFilesCount(String fileName);
        public abstract int GetSchedulesFolderId();
        public abstract int GetFileId(String originalFileName);
        
        public abstract bool IsOriginalFileNameLocallyAvailable(int? parentFolderId, String targetName);
        public abstract bool IsFolderNameLocallyAvailable(int? parentFolderId, String targetName);
        public abstract void AddFolder(int? parentFolderId, String name, int creatorUserId);
        public abstract void AddNewFile(int? parentFolderId, String originalName, String contentType, int creatorUserId, String localFileName);
        public abstract void AddVersion(int currentFileId, String calculatedLocalFileName, String versionComment, int creatorUserId);
        public abstract void CheckScheduleAvailability(int userId);
        public abstract void CheckSchedulesFolderAvailability(int userId);

        public abstract void DeleteFolder(int folderId);
        public abstract void DeleteFile(int fileId);
        public abstract void DeleteVersion(int versionId);

        public abstract IDataReader GetDocumentsExchangeModules(int ModuleId);
        public abstract IDataReader GetDocumentsExchangeModulesByUser(int ModuleId, int UserId);
        public abstract IDataReader GetDocumentsExchangeModule(int ModuleId, int ItemId);
        public abstract void AddDocumentsExchangeModule(int ModuleId, string Content, int UserId);
        public abstract void UpdateDocumentsExchangeModule(int ModuleId, int ItemId, string Content, int UserId);
        public abstract void DeleteDocumentsExchangeModule(int ModuleId, int ItemId);

        #endregion

        
    }
}