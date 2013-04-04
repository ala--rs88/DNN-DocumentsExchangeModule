using System;
using System.Data;
using System.Data.SqlClient;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Framework.Providers;
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

        public override String GetFileContentType(int fileId)
        {
            String command = String.Format("select {0} ({1})",
                            GetFullyQualifiedName("DocumentsExchangeModule_GetFileContentType"),
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