using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Entities.Modules.Actions;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Services.Localization;
using IgorKarpov.DocumentsExchangeModule.Components.Entities;
using IgorKarpov.DocumentsExchangeModule.Extensions;
using File = IgorKarpov.DocumentsExchangeModule.Components.Entities.File;

namespace IgorKarpov.Modules.DocumentsExchangeModule
{

    /// ----------------------------------------------------------------------------- 
    /// <summary> 
    /// The ViewDocumentsExchangeModule class displays the content 
    /// </summary> 
    /// <remarks> 
    /// </remarks> 
    /// <history> 
    /// </history> 
    /// ----------------------------------------------------------------------------- 
    partial class ViewDocumentsExchangeModule : PortalModuleBase, IActionable
    {
        private const String FOLDERS_TRACE = "foldersTrace";
        private const String UPLOADS_FOLDER_RELATIVE_PATH = "~/DesktopModules/IgorKarpov.DocumentsExchangeModule/Uploads/";

        #region "Event Handlers"

        /// ----------------------------------------------------------------------------- 
        /// <summary> 
        /// Page_Load runs when the control is loaded 
        /// </summary> 
        /// ----------------------------------------------------------------------------- 
        protected void Page_Load(object sender, System.EventArgs e)
        {
            if (!HttpContext.Current.User.Identity.IsAuthenticated)
            {
                multiView.Visible = false;
                return;
            }
            try
            {
                DocumentsExchangeModuleController moduleController = new DocumentsExchangeModuleController();

                // get the content from the DocumentsExchangeModule table 
                List<DocumentsExchangeModuleInfo> colDocumentsExchangeModules =
                        moduleController.GetDocumentsExchangeModulesByUser(ModuleId, UserInfo.UserID);

                //if (colDocumentsExchangeModules.Count == 0)
                //{
                //    // add the content to the DocumentsExchangeModule table 
                //    DocumentsExchangeModuleInfo objDocumentsExchangeModule = new DocumentsExchangeModuleInfo
                //        {
                //            ModuleId = ModuleId,
                //            Content = Localization.GetString("DefaultContent", LocalResourceFile),
                //            CreatedByUser = this.UserId
                //        };
                //    objDocumentsExchangeModules.AddDocumentsExchangeModule(objDocumentsExchangeModule);
                //    // get the content from the DocumentsExchangeModule table 
                //    colDocumentsExchangeModules = objDocumentsExchangeModules.GetDocumentsExchangeModulesByUser(ModuleId, UserInfo.UserID);
                //}

                // bind the content to the repeater 
                //lstFolders.DataSource = colDocumentsExchangeModules;
                //lstFolders.DataBind();

                if (ViewState[FOLDERS_TRACE] == null)
                {
                    UpdateNavigationControls(null);
                }

            }

            catch (Exception exc)
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }

        /// ----------------------------------------------------------------------------- 
        /// <summary>
        /// lstFolders_ItemDataBound runs when items are bound. Here the 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// ----------------------------------------------------------------------------- 
        protected void lstFolders_ItemDataBound(object sender, DataListItemEventArgs e)
        {
            ((LinkButton)e.Item.FindControl("lbtnFolderName")).Text =
                (((Folder)e.Item.DataItem).Name ?? String.Empty).Shorten();
            ((Label)e.Item.FindControl("lblCreatedBy")).Text =
                (((Folder)e.Item.DataItem).CreatorDisplayName ?? String.Empty).Shorten();
            ((Label)e.Item.FindControl("lblCreationDate")).Text =
                ((Folder)e.Item.DataItem).CreationDate.ToString();
        }

        protected void lstFiles_ItemDataBound(object sender, DataListItemEventArgs e)
        {
            ((LinkButton)e.Item.FindControl("lbtnFileName")).Text =
                (((File) e.Item.DataItem).OriginalName ?? String.Empty).Shorten();
            ((Label)e.Item.FindControl("lblCreationDate")).Text =
                ((File)e.Item.DataItem).CreationDate.ToString();
            ((Label)e.Item.FindControl("lblCreatedBy")).Text =
                (((File)e.Item.DataItem).CreatorDisplayName ?? String.Empty).Shorten();
            ((Label) e.Item.FindControl("lblLastVersionDate")).Text =
                ((File) e.Item.DataItem).LastVersionDate.ToString();
        }

        #endregion

        #region "Optional Interfaces"

        /// ----------------------------------------------------------------------------- 
        /// <summary> 
        /// Registers the module actions required for interfacing with the portal framework 
        /// </summary> 
        /// <value></value> 
        /// <returns></returns> 
        /// <remarks></remarks> 
        /// <history> 
        /// </history> 
        /// ----------------------------------------------------------------------------- 
        public ModuleActionCollection ModuleActions
        {
            get
            {
                ModuleActionCollection Actions = new ModuleActionCollection();
                Actions.Add(GetNextActionID(), Localization.GetString(ModuleActionType.AddContent, this.LocalResourceFile),
                   ModuleActionType.AddContent, "", "add.gif", EditUrl(), false, DotNetNuke.Security.SecurityAccessLevel.Edit,
                    true, false);
                return Actions;
            }
        }

        #endregion


        protected void lbtnShowUploadPage_Click(object sender, EventArgs e)
        {
            multiView.SetActiveView(uploadFilePage);
        }

        protected void lbtnShowFilesPage_Click(object sender, EventArgs e)
        {
            multiView.SetActiveView(filesPage);
        }

        protected void lbtnShowCreateFolder_Click(object sender, EventArgs e)
        {
            txtbxTargetFileName.Text = String.Empty;
            multiView.SetActiveView(createFolderPage);
        }

        

        protected void uploadButton_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(fileUploader.FileName))
            {
                return;
            }
            fileUploader.SaveAs(Server.MapPath(UPLOADS_FOLDER_RELATIVE_PATH) +
                                    "uploaded_" +
                                    Path.GetFileName(fileUploader.FileName));
            DownloadFile(Server.MapPath(UPLOADS_FOLDER_RELATIVE_PATH),
                                        "uploaded_" + Path.GetFileName(fileUploader.FileName),
                                        fileUploader.PostedFile.ContentType);
        }

        protected void lstContent_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        /// Downloads a file
        /// 
        /// Path to the directory
        /// File Name of a document/file
        /// Content Type of the downloadable file (e.g. application/pdf)
        private void DownloadFile(String strPath, String strFileName, String strContentType)
        {
            if (!System.IO.File.Exists(strPath + strFileName))
            {
                return;
            }
            Response.ContentType = strContentType;
            Response.AddHeader("content-disposition", "attachment; filename=" + strFileName);
            byte[] getContent;
            using (FileStream sourceFile = new FileStream(strPath + strFileName, FileMode.Open))
            {
                long fileSize = sourceFile.Length;
                getContent = new byte[(int) fileSize];
                sourceFile.Read(getContent, 0, (int) sourceFile.Length);
            }

            Response.BinaryWrite(getContent);
        }

        private void UpdateNavigationControls(int? parentFolderId)
        {
            if (parentFolderId.HasValue && parentFolderId.Value < 1)
            {
                parentFolderId = null;
            }

            DocumentsExchangeModuleController moduleController =
                new DocumentsExchangeModuleController();


            lstFolders.DataSource = moduleController.GetFolders(parentFolderId);
            lstFolders.DataBind();

            lstFiles.DataSource = moduleController.GetFiles(parentFolderId);
            lstFiles.DataBind();

            List<int?> foldersTrace = ViewState[FOLDERS_TRACE] as List<int?> ??
                            new List<int?>();


            if (!foldersTrace.Contains(parentFolderId))
            {
                foldersTrace.Add(parentFolderId);
            }
            else
            {
                if (foldersTrace.IndexOf(parentFolderId) < foldersTrace.Count - 1)
                {
                    foldersTrace.RemoveRange(foldersTrace.IndexOf(parentFolderId) + 1,
                        foldersTrace.Count - 1 - foldersTrace.IndexOf(parentFolderId));
                }
            }

            ViewState[FOLDERS_TRACE] = foldersTrace;

            StringBuilder foldersTraceString = new StringBuilder();
            foreach (var folderId in foldersTrace)
            {
                foldersTraceString.Append(folderId.HasValue
                                              ? String.Format(">{0}", folderId.Value)
                                              : String.Format("{0}", "ROOT"));
            }
            lblFoldersTrace.Text = foldersTraceString.ToString();

            UpdateUpButtonVisibility();
        }

        protected void btnCreateFolder_Click(object sender, EventArgs e)
        {
            String targetFileName = txtbxTargetFileName.Text;
            List<int?> foldersTrace = ViewState[FOLDERS_TRACE] as List<int?>;
            int? parentFolderId = (foldersTrace != null) ?
                        foldersTrace[foldersTrace.Count - 1] :
                        null;
            (new DocumentsExchangeModuleController()).AddFolder(parentFolderId,
                            targetFileName,
                            UserId);
            UpdateNavigationControls(parentFolderId);
            multiView.SetActiveView(filesPage);
        }

        protected void lbtnFolderName_Click(object sender, EventArgs e)
        {
            UpdateNavigationControls(int.Parse(lstFolders.
                                                DataKeys[
                                                    ((DataListItem)((LinkButton)sender).
                                                            NamingContainer).ItemIndex]
                                                .ToString()));
        }


        protected void lbtnFileName_Click(object sender, EventArgs e)
        {
            int fileId = int.Parse(lstFiles.
                                    DataKeys[
                                        ((DataListItem)((LinkButton)sender).
                                                NamingContainer).ItemIndex]
                                    .ToString());
            DocumentsExchangeModuleController moduleController =
                new DocumentsExchangeModuleController();
            String fileContentType = moduleController.GetFileContentType(fileId);
            String localFileName = moduleController.GetFileLastVersionLocalName(fileId);
            if (!String.IsNullOrWhiteSpace(fileContentType) &&
                !String.IsNullOrWhiteSpace(localFileName))
            {
                DownloadFile(Server.MapPath(UPLOADS_FOLDER_RELATIVE_PATH),
                             localFileName,
                             fileContentType);
            }
        }

        protected void lbtnUp_Click(object sender, EventArgs e)
        {
            List<int?> foldersTrace = ViewState[FOLDERS_TRACE] as List<int?>;
            if (foldersTrace != null && foldersTrace.Count >= 1)
            {
                UpdateNavigationControls(foldersTrace[foldersTrace.Count - 2]);
            }
        }

        private void UpdateUpButtonVisibility()
        {
            List<int?> foldersTrace = ViewState[FOLDERS_TRACE] as List<int?>;
            if (foldersTrace != null && foldersTrace.Count < 2)
            {
                tblUp.Visible = false;
            }
            else
            {
                tblUp.Visible = true;
            }
        }
    }

}