using System;
using System.Data;
using System.Data.SqlClient;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Framework.Providers;
using IgorKarpov.DocumentsExchangeModule.Components.Entities;
using Microsoft.ApplicationBlocks.Data;

namespace IgorKarpov.Modules.DocumentsExchangeModule
{

    /// ----------------------------------------------------------------------------- 
    /// <summary> 
    /// SQL Server implementation of the abstract DataProvider class 
    /// </summary> 
    /// <remarks> 
    /// </remarks> 
    /// <history> 
    /// </history> 
    /// ----------------------------------------------------------------------------- 
    public class SqlDataProvider : DataProvider
    {


        #region "Private Members"

        private const string ProviderType = "data";
        private const string ModuleQualifier = "IgorKarpov_";

        private ProviderConfiguration _providerConfiguration = ProviderConfiguration.GetProviderConfiguration(ProviderType);
        private string _connectionString;
        private string _providerPath;
        private string _objectQualifier;
        private string _databaseOwner;

        #endregion

        #region "Constructors"

        public SqlDataProvider()
        {

            // Read the configuration specific information for this provider 
            Provider objProvider = (Provider)_providerConfiguration.Providers[_providerConfiguration.DefaultProvider];

            // Read the attributes for this provider 

            //Get Connection string from web.config 
            _connectionString = Config.GetConnectionString();

            if (_connectionString == "")
            {
                // Use connection string specified in provider 
                _connectionString = objProvider.Attributes["connectionString"];
            }

            _providerPath = objProvider.Attributes["providerPath"];

            _objectQualifier = objProvider.Attributes["objectQualifier"];
            if (_objectQualifier != "" & _objectQualifier.EndsWith("_") == false)
            {
                _objectQualifier += "_";
            }

            _databaseOwner = objProvider.Attributes["databaseOwner"];
            if (_databaseOwner != "" & _databaseOwner.EndsWith(".") == false)
            {
                _databaseOwner += ".";
            }

        }

        #endregion

        #region "Properties"

        public string ConnectionString
        {
            get { return _connectionString; }
        }

        public string ProviderPath
        {
            get { return _providerPath; }
        }

        public string ObjectQualifier
        {
            get { return _objectQualifier; }
        }

        public string DatabaseOwner
        {
            get { return _databaseOwner; }
        }

        #endregion

        #region "Private Methods"

        private string GetFullyQualifiedName(string name)
        {
            return DatabaseOwner + ObjectQualifier + ModuleQualifier + name;
        }

        private object GetNull(object Field)
        {
            return DotNetNuke.Common.Utilities.Null.GetNull(Field, DBNull.Value);
        }

        #endregion

        #region "Public Methods"

        public override IDataReader GetFolders(int? parentFolderId)
        {
            return (IDataReader)SqlHelper.ExecuteReader(ConnectionString, GetFullyQualifiedName("DocumentsExchangeModule_GetFolders"), parentFolderId);
        }

        public override IDataReader GetFiles(int? parentFolderId)
        {
            return (IDataReader)SqlHelper.ExecuteReader(ConnectionString, GetFullyQualifiedName("DocumentsExchangeModule_GetFiles"), parentFolderId);
        }

        public override IDataReader GetVersions(int fileId)
        {
            return (IDataReader)SqlHelper.ExecuteReader(ConnectionString, GetFullyQualifiedName("DocumentsExchangeModule_GetVersions"), fileId);
        }

        public override Schedule GetSchedule(int userId)
        {
            IDataReader entityReader = SqlHelper.ExecuteReader(ConnectionString,
                                                               GetFullyQualifiedName(
                                                                   "DocumentsExchangeModule_GetSchedule"),
                                                               userId);
            Object[] columnsValues = new Object[10];
            entityReader.Read();
            entityReader.GetValues(columnsValues);
            return new Schedule
                {
                    Id = (int)columnsValues[0],
                    UserId = (int)columnsValues[1],
                    LastModificationDate = (DateTime)columnsValues[2],
                    Mon = (String)columnsValues[3],
                    Tue = (String)columnsValues[4],
                    Wed = (String)columnsValues[5],
                    Thu = (String)columnsValues[6],
                    Fri = (String)columnsValues[7],
                    Sat = (String)columnsValues[8],
                    Sun = (String)columnsValues[9]
                };
        }

        public override void UpdateSchedule(int userId, String mon, String tue, String wed, String thu, String fri, String sat, String sun)
        {
            SqlHelper.ExecuteNonQuery(ConnectionString,
                        GetFullyQualifiedName("DocumentsExchangeModule_UpdateSchedule"),
                        userId,
                        mon,
                        tue,
                        wed,
                        thu,
                        fri,
                        sat,
                        sun);
        }

        public override String GetFileContentType(int fileId)
        {
            String command = String.Format("select {0} ({1})",
                            GetFullyQualifiedName("DocumentsExchangeModule_GetFileContentType"),
                            fileId);
            return (String)SqlHelper.ExecuteScalar(ConnectionString, CommandType.Text, command);
        }

        public override string GetOriginalFileName(int fileId)
        {
            String command = String.Format("select {0} ({1})",
                            GetFullyQualifiedName("DocumentsExchangeModule_GetOriginalFileName"),
                            fileId);
            return (String)SqlHelper.ExecuteScalar(ConnectionString, CommandType.Text, command);
        }

        public override String GetFileLastVersionLocalName(int fileId)
        {
            String command = String.Format("select {0} ({1})",
                            GetFullyQualifiedName("DocumentsExchangeModule_GetFileLastVersionLocalName"),
                            fileId);
            return (String)SqlHelper.ExecuteScalar(ConnectionString, CommandType.Text, command);
        }

        public override String GetVersionLocalName(int versionId)
        {
            String command = String.Format("select {0} ({1})",
                            GetFullyQualifiedName("DocumentsExchangeModule_GetVersionLocalName"),
                            versionId);
            return (String)SqlHelper.ExecuteScalar(ConnectionString, CommandType.Text, command);
        }

        public override int GetRelatedVersionsCount(int versionId)
        {
            String command = String.Format("select {0} ({1})",
                            GetFullyQualifiedName("DocumentsExchangeModule_GetRelatedVersionsCount"),
                            versionId);
            return (int)SqlHelper.ExecuteScalar(ConnectionString, CommandType.Text, command);
        }

        public override int GetFilesCount(String fileName)
        {
            String command = String.Format("select {0} ('{1}')",
                            GetFullyQualifiedName("DocumentsExchangeModule_GetFilesCount"),
                            fileName);
            return (int)SqlHelper.ExecuteScalar(ConnectionString, CommandType.Text, command);
        }

        public override int GetSchedulesFolderId()
        {
            String command = String.Format("select {0} ()",
                            GetFullyQualifiedName("DocumentsExchangeModule_GetSchedulesFolderId"));
            return (int)SqlHelper.ExecuteScalar(ConnectionString, CommandType.Text, command);
        }

        public override int GetFileId(string originalFileName)
        {
            String command = String.Format("select {0} ('{1}')",
                            GetFullyQualifiedName("DocumentsExchangeModule_GetFileId"),
                            originalFileName);
            return (int)SqlHelper.ExecuteScalar(ConnectionString, CommandType.Text, command);
        }

        public override bool IsOriginalFileNameLocallyAvailable(int? parentFolderId, String targetaName)
        {
            String parentFolderIdString = parentFolderId.HasValue
                                              ? parentFolderId.Value.ToString()
                                              : "NULL";
            String command = String.Format("select {0} ({1}, N'{2}')",
                            GetFullyQualifiedName("DocumentsExchangeModule_IsOriginalFileNameLocallyAvailable"),
                            parentFolderIdString,
                            targetaName);
            return (bool)SqlHelper.ExecuteScalar(ConnectionString, CommandType.Text, command);
        }

        public override bool IsFolderNameLocallyAvailable(int? parentFolderId, String targetaName)
        {
            String parentFolderIdString = parentFolderId.HasValue
                                              ? parentFolderId.Value.ToString()
                                              : "NULL";
            String command = String.Format("select {0} ({1}, N'{2}')",
                            GetFullyQualifiedName("DocumentsExchangeModule_IsFolderNameLocallyAvailable"),
                            parentFolderIdString,
                            targetaName);
            return (bool)SqlHelper.ExecuteScalar(ConnectionString, CommandType.Text, command);
        }

        public override void AddFolder(int? parentFolderId, string name, int creatorUserId)
        {
            SqlHelper.ExecuteNonQuery(ConnectionString,
                        GetFullyQualifiedName("DocumentsExchangeModule_AddFolder"),
                        parentFolderId,
                        name,
                        creatorUserId);
        }

        public override void AddNewFile(int? parentFolderId, String originalName,
                                String contentType, int creatorUserId, String localFileName)
        {
            SqlHelper.ExecuteNonQuery(ConnectionString,
                        GetFullyQualifiedName("DocumentsExchangeModule_AddNewFile"),
                        parentFolderId,
                        originalName,
                        contentType,
                        creatorUserId,
                        localFileName);
        }

        public override void AddVersion(int currentFileId, string calculatedLocalFileName, string versionComment, int creatorUserId)
        {
            SqlHelper.ExecuteNonQuery(ConnectionString,
                        GetFullyQualifiedName("DocumentsExchangeModule_AddVersion"),
                        currentFileId,
                        calculatedLocalFileName,
                        versionComment,
                        creatorUserId);
        }

        public override void CheckScheduleAvailability(int userId)
        {
            SqlHelper.ExecuteNonQuery(ConnectionString,
                        GetFullyQualifiedName("DocumentsExchangeModule_CheckScheduleAvailability"),
                        userId);
        }

        public override void CheckSchedulesFolderAvailability(int userId)
        {
            SqlHelper.ExecuteNonQuery(ConnectionString,
                        GetFullyQualifiedName("DocumentsExchangeModule_CheckSchedulesFolderAvailability"),
                        userId);
        }

        public override void DeleteFolder(int folderId)
        {
            SqlHelper.ExecuteNonQuery(ConnectionString,
                        GetFullyQualifiedName("DocumentsExchangeModule_HideFolder"),
                        folderId);
        }

        public override void DeleteFile(int fileId)
        {
            SqlHelper.ExecuteNonQuery(ConnectionString,
                        GetFullyQualifiedName("DocumentsExchangeModule_HideFile"),
                        fileId);
        }

        public override void DeleteVersion(int versionId)
        {
            SqlHelper.ExecuteNonQuery(ConnectionString,
                        GetFullyQualifiedName("DocumentsExchangeModule_HideVersion"),
                        versionId);
        }

        public override IDataReader GetDocumentsExchangeModules(int ModuleId)
        {
            return (IDataReader)SqlHelper.ExecuteReader(ConnectionString, GetFullyQualifiedName("GetDocumentsExchangeModules"), ModuleId);
        }

        public override IDataReader GetDocumentsExchangeModule(int ModuleId, int ItemId)
        {
            return (IDataReader)SqlHelper.ExecuteReader(ConnectionString, GetFullyQualifiedName("GetDocumentsExchangeModule"), ModuleId, ItemId);
        }

        public override void AddDocumentsExchangeModule(int ModuleId, string Content, int UserId)
        {
            SqlHelper.ExecuteNonQuery(ConnectionString, GetFullyQualifiedName("AddDocumentsExchangeModule"), ModuleId, Content, UserId);
        }

        public override void UpdateDocumentsExchangeModule(int ModuleId, int ItemId, string Content, int UserId)
        {
            SqlHelper.ExecuteNonQuery(ConnectionString, GetFullyQualifiedName("UpdateDocumentsExchangeModule"), ModuleId, ItemId, Content, UserId);
        }

        public override void DeleteDocumentsExchangeModule(int ModuleId, int ItemId)
        {
            SqlHelper.ExecuteNonQuery(ConnectionString, GetFullyQualifiedName("DeleteDocumentsExchangeModule"), ModuleId, ItemId);
        }

        #endregion


        public override IDataReader GetDocumentsExchangeModulesByUser(int ModuleId, int UserId)
        {
            return (IDataReader)SqlHelper.ExecuteReader(ConnectionString, GetFullyQualifiedName("GetDocumentsExchangeModulesByUser"), ModuleId, UserId);
        }
    }
}