<%@ Control Language="C#" Inherits="IgorKarpov.Modules.DocumentsExchangeModule.ViewDocumentsExchangeModule"
    AutoEventWireup="true" CodeBehind="ViewDocumentsExchangeModule.ascx.cs" %>




<asp:MultiView ID="multiView" runat="server" ActiveViewIndex="0">
    <asp:View ID="filesPage" runat="server">
        <table cellpadding="4" style="width: 100%; height: 45px; background: lightgray;">
            <tr>
                <th width="30%" style="vertical-align: middle;">Content</th>
                <th width="20%" style="vertical-align: middle;">Created By</th>
                <th width="20%" style="vertical-align: middle;">Creation Date</th>
                <th width="20%" style="vertical-align: middle;">Last Version Date</th>
                <th width="10%" style="vertical-align: middle;">Actions</th>
            </tr>
        </table>

        <asp:DataList ID="lstFolders" DataKeyField="Id" runat="server" CellPadding="4"
            OnItemDataBound="lstFolders_ItemDataBound" BackColor="White" Width="100%">
            <ItemTemplate>
                <table cellpadding="4" style="border-style: solid; border-color: inherit; border-width: 1px; width: 100%; height: 25px;">
                    <tr>
                        <td valign="top" width="30%" align="left">
                            <asp:LinkButton ID="lbtnFolderName" runat="server" Text="Name" CssClass="Normal" OnClick="lbtnFolderName_Click" />
                        </td>
                        <td valign="top" width="20%" align="left">
                            <asp:Label ID="lblCreatedBy" runat="server" Text="Created By"></asp:Label>
                        </td>
                        <td valign="top" width="20%" align="center">
                            <asp:Label ID="lblCreationDate" runat="server" Text="Creation Date"></asp:Label>
                        </td>
                        <td valign="top" width="20%" align="center"></td>
                        <td valign="top" width="10%" align="right">

                            <asp:HyperLink ID="HyperLink1" NavigateUrl='<%# EditUrl("Id",DataBinder.Eval(Container.DataItem,"Id").ToString()) %>' 
                                Visible="<%# IsEditable %>" runat="server">
                                <asp:Image ID="Image1" runat="server" ImageUrl="~/images/edit.gif" AlternateText="Edit"
                                    Visible="<%#IsEditable%>" resourcekey="Edit" />
                            </asp:HyperLink>
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

                            <asp:Label ID="lblOriginalName" runat="server" Text="Original Name" CssClass="Normal" />
                        </td>
                        <td valign="top" width="20%" align="left">
                            <asp:Label ID="lblCreatedBy" runat="server" Text="Created By"></asp:Label>
                        </td>
                        <td valign="top" width="20%" align="center">
                            <asp:Label ID="lblCreationDate" runat="server" Text="Creation Date"></asp:Label>
                        </td>
                        <td valign="top" width="20%" align="center">
                            <asp:Label ID="lblLastVersionDate" runat="server" Text="Last Version Date"></asp:Label>
                        </td>
                        <td valign="top" width="10%" align="right">

                            <asp:HyperLink ID="HyperLink1" NavigateUrl='<%# EditUrl("Id",DataBinder.Eval(Container.DataItem,"Id").ToString()) %>' 
                                Visible="<%# IsEditable %>" runat="server">
                                <asp:Image ID="Image1" runat="server" ImageUrl="~/images/edit.gif" AlternateText="Edit"
                                    Visible="<%#IsEditable%>" resourcekey="Edit" />
                            </asp:HyperLink>
                            <asp:Button ID="deleteItemButton" runat="server" Text="X" Height="23px" Width="23px" />

                        </td>
                    </tr>
                </table>
            </ItemTemplate>
        </asp:DataList>



        <asp:Button ID="showUploadPageButton" runat="server" OnClick="showUploadPageButton_Click" Text="Upload file" />
    </asp:View>

    <asp:View ID="uploadFilePage" runat="server">
        <asp:FileUpload ID="fileUploader" runat="server" />
        <asp:Button ID="uploadButton" runat="server" CssClass="myButton" Text="Upload" OnClick="uploadButton_Click" />
        <asp:Panel ID="Panel1" runat="server">
            <asp:Button ID="showFilesPageButton" runat="server" OnClick="showFilesPageButton_Click" Text="Back" />
        </asp:Panel>
    </asp:View>
</asp:MultiView>

<p>
    &nbsp;
</p>


