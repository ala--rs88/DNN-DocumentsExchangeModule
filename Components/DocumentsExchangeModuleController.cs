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

        internal String GetFileLastVersionLocalName(int fileId)
        {
            return DataProvider.Instance().GetFileLastVersionLocalName(fileId);
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
                                 String uploadsFolderPath,
                                 HttpResponse response)
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

            String calculatedLocalFileName = Guid.NewGuid().ToString() +
                                    Path.GetExtension(fileUploader.FileName);
            int i = 0;
            while (System.IO.File.Exists(uploadsFolderPath + calculatedLocalFileName) &&
                   i < 10)
            {
                calculatedLocalFileName = Guid.NewGuid().ToString() +
                                    Path.GetExtension(fileUploader.FileName);
                i++;
            }
            if (i >= 10)
            {
                return false;
            }
            else
            {
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
            //byte[] fileBytes;
            //using (FileStream localFile = new FileStream(path + localFileName, FileMode.Open))
            //{
            //    long fileSize = localFile.Length;
            //    fileBytes = new byte[(int)fileSize];
            //    localFile.Read(fileBytes, 0, (int)localFile.Length);
            //}
            //response.BinaryWrite(fileBytes);
            
            response.WriteFile(path + localFileName);
            response.End();
        }
    }
}