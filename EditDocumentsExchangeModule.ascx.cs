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

using DotNetNuke;
using DotNetNuke.Common;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Services.Localization;
using DotNetNuke.Entities.Modules;

namespace IgorKarpov.Modules.DocumentsExchangeModule
{

    /// ----------------------------------------------------------------------------- 
    /// <summary> 
    /// The EditDocumentsExchangeModule class is used to manage content 
    /// </summary> 
    /// <remarks> 
    /// </remarks> 
    /// <history> 
    /// </history> 
    /// ----------------------------------------------------------------------------- 
    partial class EditDocumentsExchangeModule : PortalModuleBase
    {

        #region "Private Members"

        private int ItemId = Null.NullInteger;

        #endregion

        #region "Event Handlers"

        /// ----------------------------------------------------------------------------- 
        /// <summary> 
        /// Page_Load runs when the control is loaded 
        /// </summary> 
        /// <remarks> 
        /// </remarks> 
        /// <history> 
        /// </history> 
        /// ----------------------------------------------------------------------------- 
        protected void Page_Load(object sender, System.EventArgs e)
        {
            try
            {

                // Determine ItemId of DocumentsExchangeModule to Update 
                if ((Request.QueryString["ItemId"] != null))
                {
                    ItemId = Int32.Parse(Request.QueryString["ItemId"]);
                }

                // If this is the first visit to the page, bind the role data to the datalist 
                if (Page.IsPostBack == false)
                {

                    cmdDelete.Attributes.Add("onClick", "javascript:return confirm('" + Localization.GetString("DeleteItem") + "');");

                    if (!Null.IsNull(ItemId))
                    {
                        // get content 
                        DocumentsExchangeModuleController objDocumentsExchangeModules = new DocumentsExchangeModuleController();
                        DocumentsExchangeModuleInfo objDocumentsExchangeModule = objDocumentsExchangeModules.GetDocumentsExchangeModule(ModuleId, ItemId);
                        if ((objDocumentsExchangeModule != null))
                        {
                            txtContent.Text = objDocumentsExchangeModule.Content;
                            ctlAudit.CreatedByUser = objDocumentsExchangeModule.CreatedByUserName;
                            ctlAudit.CreatedDate = objDocumentsExchangeModule.CreatedDate.ToString();
                        }
                        else
                        {
                            // security violation attempt to access item not related to this Module 
                            Response.Redirect(Globals.NavigateURL(), true);
                        }
                    }
                    else
                    {
                        cmdDelete.Visible = false;
                        ctlAudit.Visible = false;
                    }
                }
            }

            catch (Exception exc)
            {
                //Module failed to load 
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }

        /// ----------------------------------------------------------------------------- 
        /// <summary> 
        /// cmdCancel_Click runs when the cancel button is clicked 
        /// </summary> 
        /// <remarks> 
        /// </remarks> 
        /// <history> 
        /// </history> 
        /// ----------------------------------------------------------------------------- 
        protected void cmdCancel_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect(Globals.NavigateURL(), true);
            }
            catch (Exception exc)
            {
                //Module failed to load 
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }

        /// ----------------------------------------------------------------------------- 
        /// <summary> 
        /// cmdUpdate_Click runs when the update button is clicked 
        /// </summary> 
        /// <remarks> 
        /// </remarks> 
        /// <history> 
        /// </history> 
        /// ----------------------------------------------------------------------------- 
        protected void cmdUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                DocumentsExchangeModuleController objDocumentsExchangeModules = new DocumentsExchangeModuleController();

                DocumentsExchangeModuleInfo objDocumentsExchangeModule = new DocumentsExchangeModuleInfo();

                objDocumentsExchangeModule.ModuleId = ModuleId;
                objDocumentsExchangeModule.ItemId = ItemId;
                objDocumentsExchangeModule.Content = txtContent.Text;
                objDocumentsExchangeModule.CreatedByUser = this.UserId;

                if (Null.IsNull(ItemId))
                {
                    // add the content within the DocumentsExchangeModule table 
                    objDocumentsExchangeModules.AddDocumentsExchangeModule(objDocumentsExchangeModule);
                }
                else
                {
                    // update the content within the DocumentsExchangeModule table 
                    objDocumentsExchangeModules.UpdateDocumentsExchangeModule(objDocumentsExchangeModule);
                }

                // Redirect back to the portal home page 
                Response.Redirect(Globals.NavigateURL(), true);
            }
            catch (Exception exc)
            {
                //Module failed to load 
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }

        /// ----------------------------------------------------------------------------- 
        /// <summary> 
        /// cmdDelete_Click runs when the delete button is clicked 
        /// </summary> 
        /// <remarks> 
        /// </remarks> 
        /// <history> 
        /// </history> 
        /// ----------------------------------------------------------------------------- 
        protected void cmdDelete_Click(object sender, EventArgs e)
        {
            try
            {
                // Only attempt to delete the item if it exists already 
                if (!Null.IsNull(ItemId))
                {

                    DocumentsExchangeModuleController objDocumentsExchangeModules = new DocumentsExchangeModuleController();
                    objDocumentsExchangeModules.DeleteDocumentsExchangeModule(ModuleId, ItemId);

                }

                // Redirect back to the portal home page 
                Response.Redirect(Globals.NavigateURL(), true);
            }
            catch (Exception exc)
            {
                //Module failed to load 
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }

        #endregion

    }
}
