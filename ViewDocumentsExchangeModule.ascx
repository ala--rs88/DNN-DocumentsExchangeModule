<%@ Control Language="C#" Inherits="IgorKarpov.Modules.DocumentsExchangeModule.ViewDocumentsExchangeModule"
    AutoEventWireup="true" CodeBehind="ViewDocumentsExchangeModule.ascx.cs" %>

<script src="//ajax.googleapis.com/ajax/libs/jquery/1.9.1/jquery.min.js"></script>

<asp:MultiView ID="multiView" runat="server" ActiveViewIndex="0">
    <asp:View ID="filesPage" runat="server">
        <table cellpadding="4" style="width: 100%; height: 45px; background: lightgray;">
            <tr>
                <th width="30%" style="vertical-align: middle;">Title</th>
                <th width="20%" style="vertical-align: middle;">Author</th>
                <th width="20%" style="vertical-align: middle;">Creation Date</th>
                <th width="20%" style="vertical-align: middle;">Last Version Date</th>
                <th width="10%" style="vertical-align: middle;">Actions</th>
            </tr>
        </table>

        <div style="margin: 8px 10px 4px 6px;">
            <asp:Label ID="lblFoldersTrace" runat="server"></asp:Label>
        </div>

        <asp:Table ID="tblUp" BackColor="White" runat="server" CellPadding="4"
            Style="border-style: solid; border-color: inherit; border-width: 1px; width: 99%; height: 35px; margin: 8px 0 4px 4px;">
            <asp:TableRow>
                <asp:TableCell>
                    <asp:LinkButton ID="lbtnUp" Text="..." runat="server" OnClick="lbtnUp_Click" />
                </asp:TableCell>
            </asp:TableRow>
        </asp:Table>
        <asp:DataList ID="lstFolders" DataKeyField="Id" runat="server" CellPadding="4"
            OnItemDataBound="lstFolders_ItemDataBound" BackColor="White" Width="100%">
            <ItemTemplate>
                <table cellpadding="4" style="border-style: solid; border-color: inherit; border-width: 1px; width: 100%; height: 25px;">
                    <tr>
                        <td valign="top" width="30%" align="left">
                            <asp:LinkButton ID="lbtnFolderName" runat="server" Text="Name" CssClass="Normal" OnClick="lbtnFolderName_Click" />
                        </td>
                        <td valign="top" width="20%" align="center">
                            <asp:Label ID="lblCreatedBy" runat="server" Text="Created By"></asp:Label></td>
                        <td valign="top" width="20%" align="center">
                            <asp:Label ID="lblCreationDate" runat="server" Text="Creation Date"></asp:Label></td>
                        <td valign="top" width="20%" align="center"></td>
                        <td valign="top" width="10%" align="right">

                            <%--<asp:HyperLink ID="HyperLink1" NavigateUrl='<%# EditUrl("Id",DataBinder.Eval(Container.DataItem,"Id").ToString()) %>' 
                                Visible="<%# IsEditable %>" runat="server">
                                <asp:Image ID="Image1" runat="server" ImageUrl="~/images/edit.gif" AlternateText="Edit"
                                    Visible="<%#IsEditable%>" resourcekey="Edit" />
                            </asp:HyperLink>--%>
                            <asp:Button ID="deleteItemButton" runat="server" Text="X" Height="23px" Width="23px" />

                        </td>
                    </tr>
                </table>
            </ItemTemplate>
        </asp:DataList>

        <asp:DataList ID="lstFiles" DataKeyField="Id" runat="server" CellPadding="4"
            OnItemDataBound="lstFiles_ItemDataBound" BackColor="White" Width="100%">
            <ItemTemplate>
                <table cellpadding="4" style="border-style: solid; border-color: inherit; border-width: 1px; width: 100%; height: 25px;">
                    <tr>
                        <td valign="top" width="30%" align="left">
                            <asp:LinkButton ID="lbtnFileName" runat="server" Text="Original Name" CssClass="Normal" OnClick="lbtnFileName_Click" />
                        </td>
                        <td valign="top" width="20%" align="center">
                            <asp:Label ID="lblCreatedBy" runat="server" Text="Created By"></asp:Label></td>
                        <td valign="top" width="20%" align="center">
                            <asp:Label ID="lblCreationDate" runat="server" Text="Creation Date"></asp:Label></td>
                        <td valign="top" width="20%" align="center">
                            <asp:Label ID="lblLastVersionDate" runat="server" Text="Last Version Date"></asp:Label></td>
                        <td valign="top" width="10%" align="right">

                            <%--<asp:HyperLink ID="HyperLink1" NavigateUrl='<%# EditUrl("Id",DataBinder.Eval(Container.DataItem,"Id").ToString()) %>' 
                                Visible="<%# IsEditable %>" runat="server">
                                <asp:Image ID="Image1" runat="server" ImageUrl="~/images/edit.gif" AlternateText="Edit"
                                    Visible="<%#IsEditable%>" resourcekey="Edit" />
                            </asp:HyperLink>--%>
                            <asp:Button ID="btnShowVersionsPage" OnClick="btnShowVersionsPage_Click" runat="server" Text="V" Height="23px" Width="23px" />
                            <asp:Button ID="btnDeleteItem" runat="server" Text="X" Height="23px" Width="23px" />
                        </td>
                    </tr>
                </table>
            </ItemTemplate>
        </asp:DataList>

        <div>
            <asp:LinkButton ID="lbtnShowUploadPage" runat="server" OnClick="lbtnShowUploadPage_Click" Text="Upload new file" />
            <asp:LinkButton ID="lbtnShowCreateFolder" runat="server" OnClick="lbtnShowCreateFolder_Click" Text="Create folder" />
        </div>

    </asp:View>

    <asp:View ID="uploadFilePage" runat="server">
        <asp:FileUpload ID="fileUploader" runat="server" />
        <asp:Button ID="uploadButton" runat="server" CssClass="myButton" Text="Upload" OnClick="uploadButton_Click" />
        <asp:Panel ID="Panel1" runat="server">
            <div class="backButton">
                <asp:LinkButton ID="lbtnShowFilesPage" runat="server" OnClick="lbtnShowFilesPage_Click" Text="Back" />
            </div>
        </asp:Panel>
        <script>
            $(function () {
                $('div.backButton a').click(function () {
                    var $fileInput = $('input:file');
                    $fileInput.replaceWith($fileInput.val('').clone(true));
                });
            });
        </script>
    </asp:View>

    <asp:View ID="versionsPage" runat="server">
        <asp:Label ID="lblCurrentFileDescription" runat="server" Font-Bold="True" Font-Size="X-Large">Versions of file "FileTitle":</asp:Label><table cellpadding="4" style="width: 100%; height: 45px; background: lightgray;">
            <tr>
                <th width="10%" style="vertical-align: middle; text-align: left;">Absolute Id</th>
                <th width="40%" style="vertical-align: middle;">Comment</th>
                <th width="20%" style="vertical-align: middle;">Creation Date</th>
                <th width="20%" style="vertical-align: middle;">Created by</th>
                <th width="10%" style="vertical-align: middle;">Actions</th>
            </tr>
        </table>
        <asp:DataList ID="lstVersions" DataKeyField="Id" runat="server" CellPadding="4"
            OnItemDataBound="lstVersions_ItemDataBound" BackColor="White" Width="100%">
            <ItemTemplate>
                <table cellpadding="4" style="border-style: solid; border-color: inherit; border-width: 1px; width: 100%; height: 25px;">
                    <tr>
                        <td valign="top" width="10%" align="left" style="text-align: left">
                            <asp:Label ID="lblId" runat="server" Text="Version Id"></asp:Label></td>
                        <td valign="top" width="40%" align="center">
                            <asp:LinkButton ID="lbtnVersionComment" runat="server" Text="Name" CssClass="Normal" OnClick="lbtnVersionComment_Click" /></td>
                        <td valign="top" width="20%" align="center">
                            <asp:Label ID="lblCreationDate" runat="server" Text="Creation Date"></asp:Label></td>
                        <td valign="top" width="20%" align="center">
                            <asp:Label ID="lblCreatedBy" runat="server" Text="Created By"></asp:Label></td>
                        <td valign="top" width="10%" align="right">
                            <asp:Button ID="deleteItemButton" runat="server" Text="X" Height="23px" Width="23px" />
                        </td>
                    </tr>
                </table>
            </ItemTemplate>
        </asp:DataList>

        <asp:LinkButton ID="lbtnShowUploadVersionPage" runat="server" OnClick="lbtnShowUploadVersionPage_Click" Text="Upload new version" />
        <div class="backButton">
            <asp:LinkButton ID="lbtnShowFilesPageFromVersionsPage" runat="server" OnClick="lbtnShowFilesPage_Click" Text="Back" />
        </div>
    </asp:View>

    <asp:View ID="uploadVersionPage" runat="server">
        <label>Please, comment this version:</label>
        <br />
        <asp:TextBox ID="tbxVersionComment" runat="server"></asp:TextBox><br />
        <asp:FileUpload ID="versionUploader" runat="server" />
        <asp:Button ID="btnUploadVersion" runat="server" Text="Upload" OnClick="btnUploadVersion_Click" />
        <div class="backButton">
            <asp:LinkButton ID="lbtnShowVersionsPageFromUploadVersionPage" runat="server" OnClick="btnShowVersionsPage_Click" Text="Back" />
        </div>
        <script>
            $(function () {
                $('div.backButton a').click(function () {
                    var $fileInput = $('input:file');
                    $fileInput.replaceWith($fileInput.val('').clone(true));
                });
            });
        </script>
    </asp:View>

    <asp:View ID="createFolderPage" runat="server">
        <asp:TextBox ID="txtbxTargetFileName" runat="server"></asp:TextBox><asp:Button ID="btnCreateFolder" Text="Create folder" OnClick="btnCreateFolder_Click" runat="server" />
        <asp:LinkButton runat="server" OnClick="lbtnShowFilesPage_Click" Text="Back" />
    </asp:View>

</asp:MultiView>

<p>
    &nbsp;
</p>
