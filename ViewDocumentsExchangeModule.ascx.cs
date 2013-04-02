// 
// DotNetNuke® - http://www.dotnetnuke.com 
// Copyright (c) 2002-2013 
// by DotNetNuke Corporation 
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated 
// documentation files (the "Software"), to deal in the Software without restriction, including without limitation 
// the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and 
// to permit persons to whom the Software is furnished to do so, subject to the following conditions: 
// 
// The above copyright notice and this permission notice shall be included in all copies or substantial portions 
// of the Software. 
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
// TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
// THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
// CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
// DEALINGS IN THE SOFTWARE. 
// 

using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Entities.Modules.Actions;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Services.Localization;
using IgorKarpov.DocumentsExchangeModule.Components.Entities;
//using File = System.IO.File;
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

                UpdateNavigationListControls(null);
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
        protected void lstFolders_ItemDataBound(object sender, System.Web.UI.WebControls.DataListItemEventArgs e)
        {
            ((LinkButton)e.Item.FindControl("lbtnFolderName")).Text = ((Folder)e.Item.DataItem).Name;
            ((Label)e.Item.FindControl("lblCreatedBy")).Text = ((Folder)e.Item.DataItem).CreatorUserId.ToString();
            ((Label)e.Item.FindControl("lblCreationDate")).Text = ((Folder)e.Item.DataItem).CreationDate.ToString();
        }

        protected void lstFiles_ItemDataBound(object sender, System.Web.UI.WebControls.DataListItemEventArgs e)
        {
            ((Label)e.Item.FindControl("lblOriginalName")).Text = ((File) e.Item.DataItem).OriginalName;
            ((Label)e.Item.FindControl("lblCreationDate")).Text = ((File)e.Item.DataItem).CreationDate.ToString();
            ((Label) e.Item.FindControl("lblCreatedBy")).Text = ((File) e.Item.DataItem).CreatorUserId.ToString();
            ((Label)e.Item.FindControl("lblLastVersionDate")).Text = DateTime.Now.ToString();
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


        protected void showUploadPageButton_Click(object sender, EventArgs e)
        {
            multiView.SetActiveView(uploadFilePage);
        }

        protected void showFilesPageButton_Click(object sender, EventArgs e)
        {
            multiView.SetActiveView(filesPage);
        }

        protected void uploadButton_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(fileUploader.FileName))
            {
                return;
            }
            fileUploader.SaveAs(Server.MapPath("~/DesktopModules/IgorKarpov.DocumentsExchangeModule/Uploads/") +
                                    "uploaded_" +
                                    Path.GetFileName(fileUploader.FileName));
            DownloadFile(Server.MapPath("~/DesktopModules/IgorKarpov.DocumentsExchangeModule/Uploads/"),
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
            if (System.IO.File.Exists(strPath + strFileName))
            {
                Response.ContentType = strContentType;
                Response.AddHeader("content-disposition", "attachment; filename=" + strFileName);
                byte[] getContent;
                using (FileStream sourceFile = new FileStream(strPath + strFileName, FileMode.Open))
                {
                    long FileSize;
                    FileSize = sourceFile.Length;
                    getContent = new byte[(int) FileSize];
                    sourceFile.Read(getContent, 0, (int) sourceFile.Length);
                }

                Response.BinaryWrite(getContent);
            }
        }

        private void UpdateNavigationListControls(int? parentFolderId)
        {
            List<Folder> folders = new List<Folder>
                    {
                        new Folder
                            {
                                Id = 1,
                                Name = "Name1",
                                CreatorUserId = 1,
                                CreationDate = DateTime.Now,
                            },
                        new Folder
                            {
                                Id = 2,
                                Name = "Name2",
                                CreatorUserId = 1,
                                CreationDate = DateTime.Now,
                            }
                    };
            List<File> files = new List<File>
                    {
                        new File
                            {
                                OriginalName = "File1",
                                CreationDate = DateTime.Now
                            },
                        new File
                            {
                                OriginalName = "File2",
                                CreationDate = DateTime.Now
                            }
                    };
            lstFolders.DataSource = folders;
            lstFolders.DataBind();

            lstFiles.DataSource = files;
            lstFiles.DataBind();
        }

        protected void lbtnFolderName_Click(object sender, EventArgs e)
        {
            LinkButton clickedButton = ((LinkButton)sender);
            clickedButton.Text = lstFolders.DataKeys[((DataListItem)clickedButton.NamingContainer).ItemIndex].ToString();
        }

    }

}