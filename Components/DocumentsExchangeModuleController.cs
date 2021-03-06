using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.UI.WebControls;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Services.Search;
using IgorKarpov.DocumentsExchangeModule.Components.Entities;
using File = IgorKarpov.DocumentsExchangeModule.Components.Entities.File;

namespace IgorKarpov.Modules.DocumentsExchangeModule
{ 
    /// <summary> 
    /// The Controller class for DocumentsExchangeModule 
    /// </summary> - 
    public class DocumentsExchangeModuleController : ISearchable
    {

        #region "Public Methods"


        public List<Folder> GetFolders(int? parentFolderId)
        {
            var reader = DataProvider.Instance().
                                      GetFolders(parentFolderId);
            return CBO.FillCollection<Folder>(reader);
        }

        public List<File> GetFiles(int? parentFolderId)
        {
            return CBO.FillCollection<File>(DataProvider.Instance().
                                GetFiles(parentFolderId));
        }

        public List<FileVersion> GetVersions(int fileId)
        {
            return CBO.FillCollection<FileVersion>(DataProvider.Instance().
                                GetVersions(fileId));
        }

        /// ----------------------------------------------------------------------------- 
        /// <summary> 
        /// gets an object from the database 
        /// </summary> 
        /// <remarks> 
        /// </remarks> 
        /// <param name="ModuleId">The Id of the module</param> 
        /// <history> 
        /// </history> 
        /// ----------------------------------------------------------------------------- 
        public List<DocumentsExchangeModuleInfo> GetDocumentsExchangeModulesByUser(int ModuleId, int UserId)
        {

            return CBO.FillCollection<DocumentsExchangeModuleInfo>(DataProvider.Instance().GetDocumentsExchangeModulesByUser(ModuleId, UserId));

        }

        /// ----------------------------------------------------------------------------- 
        /// <summary> 
        /// gets an object from the database 
        /// </summary> 
        /// <remarks> 
        /// </remarks> 
        /// <param name="ModuleId">The Id of the module</param> 
        /// <param name="ItemId">The Id of the item</param> 
        /// <history> 
        /// </history> 
        /// ----------------------------------------------------------------------------- 
        public DocumentsExchangeModuleInfo GetDocumentsExchangeModule(int ModuleId, int ItemId)
        {

            return (DocumentsExchangeModuleInfo)CBO.FillObject(DataProvider.Instance().GetDocumentsExchangeModule(ModuleId, ItemId), typeof(DocumentsExchangeModuleInfo));

        }

        /// ----------------------------------------------------------------------------- 
        /// <summary> 
        /// adds an object to the database 
        /// </summary> 
        /// <remarks> 
        /// </remarks> 
        /// <param name="objDocumentsExchangeModule">The DocumentsExchangeModuleInfo object</param> 
        /// <history> 
        /// </history> 
        /// ----------------------------------------------------------------------------- 
        public void AddDocumentsExchangeModule(DocumentsExchangeModuleInfo objDocumentsExchangeModule)
        {

            if (objDocumentsExchangeModule.Content.Trim() != "")
            {
                DataProvider.Instance().AddDocumentsExchangeModule(objDocumentsExchangeModule.ModuleId, objDocumentsExchangeModule.Content, objDocumentsExchangeModule.CreatedByUser);
            }

        }

        /// ----------------------------------------------------------------------------- 
        /// <summary> 
        /// saves an object to the database 
        /// </summary> 
        /// <remarks> 
        /// </remarks> 
        /// <param name="objDocumentsExchangeModule">The DocumentsExchangeModuleInfo object</param> 
        /// <history> 
        /// </history> 
        /// ----------------------------------------------------------------------------- 
        public void UpdateDocumentsExchangeModule(DocumentsExchangeModuleInfo objDocumentsExchangeModule)
        {

            if (objDocumentsExchangeModule.Content.Trim() != "")
            {
                DataProvider.Instance().UpdateDocumentsExchangeModule(objDocumentsExchangeModule.ModuleId, objDocumentsExchangeModule.ItemId, objDocumentsExchangeModule.Content, objDocumentsExchangeModule.CreatedByUser);
            }

        }

        /// ----------------------------------------------------------------------------- 
        /// <summary> 
        /// deletes an object from the database 
        /// </summary> 
        /// <remarks> 
        /// </remarks> 
        /// <param name="ModuleId">The Id of the module</param> 
        /// <param name="ItemId">The Id of the item</param> 
        /// <history> 
        /// </history> 
        /// ----------------------------------------------------------------------------- 
        public void DeleteDocumentsExchangeModule(int ModuleId, int ItemId)
        {

            DataProvider.Instance().DeleteDocumentsExchangeModule(ModuleId, ItemId);

        }

        #endregion

        #region "Optional Interfaces"

        /// ----------------------------------------------------------------------------- 
        /// <summary> 
        /// GetSearchItems implements the ISearchable Interface 
        /// </summary> 
        /// <remarks> 
        /// </remarks> 
        /// <param name="ModInfo">The ModuleInfo for the module to be Indexed</param> 
        /// <history> 
        /// </history> 
        /// ----------------------------------------------------------------------------- 
        public DotNetNuke.Services.Search.SearchItemInfoCollection GetSearchItems(ModuleInfo ModInfo)
        {

            SearchItemInfoCollection SearchItemCollection = new SearchItemInfoCollection();

            List<DocumentsExchangeModuleInfo> colDocumentsExchangeModules = GetDocumentsExchangeModulesByUser(ModInfo.ModuleID, ModInfo.CreatedByUserID);
            foreach (DocumentsExchangeModuleInfo objDocumentsExchangeModule in colDocumentsExchangeModules)
            {
                SearchItemInfo searchItem = new SearchItemInfo(ModInfo.ModuleTitle, objDocumentsExchangeModule.Content, objDocumentsExchangeModule.CreatedByUser, objDocumentsExchangeModule.CreatedDate, ModInfo.ModuleID, objDocumentsExchangeModule.ItemId.ToString(), objDocumentsExchangeModule.Content, "ItemId=" + objDocumentsExchangeModule.ItemId.ToString());
                SearchItemCollection.Add(searchItem);
            }

            return SearchItemCollection;

        }


        #endregion


        internal String GetFileContentType(int fileId)
        {
            return DataProvider.Instance().GetFileContentType(fileId);
        }

        internal string GetOriginalFileName(int fileId)
        {
            return DataProvider.Instance().GetOriginalFileName(fileId);
        }

        internal String GetFileLastVersionLocalName(int fileId)
        {
            return DataProvider.Instance().GetFileLastVersionLocalName(fileId);
        }

        internal String GetVersionLocalName(int versionId)
        {
            return DataProvider.Instance().GetVersionLocalName(versionId);
        }

        private bool IsOriginalFileNameLocallyAvailable(int? parentFolderId, String targetName)
        {
            return DataProvider.Instance().
                IsOriginalFileNameLocallyAvailable(parentFolderId, targetName);
        }

        private bool IsFolderNameLocallyAvailable(int? parentFolderId, String targetName)
        {
            return DataProvider.Instance().
                IsFolderNameLocallyAvailable(parentFolderId, targetName);
        }

        internal void AddFolder(int? parentFolderId, String targetName, int creatorUserId)
        {
            String calculatedName = targetName;
            int i = 0;
            while (!IsFolderNameLocallyAvailable(parentFolderId, calculatedName) &&
                   i < 10)
            {
                calculatedName = String.Format("({0}) {1}",
                                               i + 2,
                                               targetName);
                i++;
            }
            if (i < 10)
            {
                DataProvider.Instance().AddFolder(parentFolderId, calculatedName, creatorUserId);
            }
        }

        private String CalculateLocallyUniqueFileName(int? parentFolderId, String targetFileName)
        {
            String calculatedName = targetFileName;
            int i = 0;
            while (!IsOriginalFileNameLocallyAvailable(parentFolderId, calculatedName) &&
                   i < 10)
            {
                calculatedName = String.Format("({0}) {1}",
                                               i + 2,
                                               targetFileName);
                i++;
            }
            return i < 10 ? calculatedName : null;
        }

        internal bool UploadNewFile(int? parentFolderId,
                                 int creatorUserId,
                                 FileUpload fileUploader,
                                 String uploadsFolderPath)
        {
            if (!Directory.Exists(uploadsFolderPath))
            {
                return false;
            }

            String calculatedOriginalFileName = CalculateLocallyUniqueFileName(parentFolderId,
                                            Path.GetFileName(fileUploader.FileName));
            if (String.IsNullOrWhiteSpace(calculatedOriginalFileName))
            {
                return false;
            }

            String calculatedLocalFileName = CalculateLocalFileName(uploadsFolderPath,
                                                                    Path.GetExtension(fileUploader.FileName));
            if (String.IsNullOrWhiteSpace(calculatedLocalFileName))
            {
                return false;
            }
            fileUploader.SaveAs(uploadsFolderPath + calculatedLocalFileName);
            if (!System.IO.File.Exists(uploadsFolderPath + calculatedLocalFileName))
            {
                return false;
            }
            DataProvider.Instance().AddNewFile(parentFolderId,
                                               calculatedOriginalFileName,
                                               fileUploader.PostedFile.ContentType,
                                               creatorUserId,
                                               calculatedLocalFileName);
            return true;
        }

        internal bool UploadVersion(int currentFileId, FileUpload versionUploader, String uploadsFolderPath,
                                    String versionComment, int creatorUserId)
        {
            if (!Directory.Exists(uploadsFolderPath))
            {
                return false;
            }
            String contentType = DataProvider.Instance().GetFileContentType(currentFileId);
            if (!contentType.Equals(versionUploader.PostedFile.ContentType))
            {
                return false;
            }
            String calculatedLocalFileName = CalculateLocalFileName(uploadsFolderPath,
                                                                    Path.GetExtension(versionUploader.FileName));
            if (String.IsNullOrWhiteSpace(calculatedLocalFileName))
            {
                return false;
            }
            versionUploader.SaveAs(uploadsFolderPath + calculatedLocalFileName);
            if (!System.IO.File.Exists(uploadsFolderPath + calculatedLocalFileName))
            {
                return false;
            }
            DataProvider.Instance().AddVersion(currentFileId,
                                               calculatedLocalFileName,
                                               versionComment,
                                               creatorUserId);
            return true;
        }

        private String CalculateLocalFileName(String targetPath, String fileExtension)
        {
            String calculatedLocalFileName = Guid.NewGuid().ToString() + fileExtension;
            int i = 0;
            while (System.IO.File.Exists(targetPath + calculatedLocalFileName) &&
                   i < 10)
            {
                calculatedLocalFileName = Guid.NewGuid().ToString() + fileExtension;
                i++;
            }
            return i >= 10
                ? String.Empty
                : calculatedLocalFileName;
        }

        /// <summary>
        /// Writes file to response.
        /// </summary>
        public void DownloadFile(String path, String localFileName, String originalFileName,
                                  String contentType, HttpResponse response)
        {
            if (!System.IO.File.Exists(path + localFileName))
            {
                return;
            }
            response.ClearContent();
            response.ClearHeaders();
            response.ContentType = contentType;
            response.AddHeader("content-disposition",
                               "attachment; filename=" + originalFileName);

            response.WriteFile(path + localFileName);
            response.End();
        }

        public void DeleteFolder(int folderId)
        {
            DataProvider.Instance().DeleteFolder(folderId);
        }

        public void DeleteFile(int fileId)
        {
            DataProvider.Instance().DeleteFile(fileId);
        }

        public bool DeleteVersion(int versionId)
        {
            if (DataProvider.Instance().GetRelatedVersionsCount(versionId) < 2)
            {
                return false;
            }
            DataProvider.Instance().DeleteVersion(versionId);
            return true;
        }

        public Schedule GetCurrentUserSchedule(int userId)
        {
            DataProvider.Instance().CheckScheduleAvailability(userId);
            return DataProvider.Instance().GetSchedule(userId);
        }

        public void UpdateSchedule(int userId, String userName, String uploadsFolderPath,
                                    String mon, String tue, String wed, String thu, String fri, String sat, String sun)
        {
            DataProvider.Instance().UpdateSchedule(userId, mon, tue, wed, thu, fri, sat, sun);
            UpdateScheduleDocument(userId, userName, uploadsFolderPath);
        }

        private bool UpdateScheduleDocument(int userId, String username, String uploadsFolderPath)
        {
            DataProvider.Instance().CheckSchedulesFolderAvailability(userId);

            Schedule schedule = DataProvider.Instance().GetSchedule(userId);
            String localFileName = SaveScheduleToFileSystem(uploadsFolderPath, schedule);
            if (String.IsNullOrWhiteSpace(localFileName))
            {
                return false;
            }
            String originalFileName = EvaluateScheduleDocumentName(userId, username);

            if (DataProvider.Instance().GetFilesCount(originalFileName) < 1)
            {
                String contentType = "text/plain";
                AddScheduleDocument(originalFileName, contentType, userId, localFileName);
            }
            else
            {
                int existingFileId = DataProvider.Instance().GetFileId(originalFileName);
                AddScheduleVersion(existingFileId, localFileName, userId);
            }
            return true;
        }

        private String EvaluateScheduleDocumentName(int userId, String username)
        {
            return String.Format("[{0}] - {1} - Schedule", userId, username);
        }

        private String SaveScheduleToFileSystem(String uploadsFolderPath, Schedule schedule)
        {
            if (!Directory.Exists(uploadsFolderPath))
            {
                return String.Empty;
            }

            String extension = ".txt";

            String calculatedLocalFileName = CalculateLocalFileName(uploadsFolderPath, extension);
            if (String.IsNullOrWhiteSpace(calculatedLocalFileName))
            {
                return String.Empty;
            }
            schedule.SaveAs(uploadsFolderPath + calculatedLocalFileName);
            return !System.IO.File.Exists(uploadsFolderPath + calculatedLocalFileName)
                    ? String.Empty
                    : calculatedLocalFileName;
        }

        private void AddScheduleDocument(String originalFileName, String contentType, int creatorUserId, String localFileName)
        {
            int parentFolderId = DataProvider.Instance().GetSchedulesFolderId();
            DataProvider.Instance().AddNewFile(parentFolderId,
                                               originalFileName,
                                               contentType,
                                               creatorUserId,
                                               localFileName);
        }

        private void AddScheduleVersion(int existingFileId, String localFileName, int creatorUserId)
        {
            DataProvider.Instance().AddVersion(existingFileId,
                                               localFileName,
                                               "Auto-Generated",
                                               creatorUserId);
        }
    }
}