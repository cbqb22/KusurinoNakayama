<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Contributer.aspx.cs" Inherits="PharmacyToolMobile.Pages.掲示板.Contributer" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>PharmacyToolMobile 掲示板</title>
    <style type="text/css">
        
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table border="0">
            <tr align="left">
                <td colspan="2">
                    <asp:Label ID="lbContributerHeaderForNo" runat="server" Text="" style="font-size:18px;color:Blue;font-weight: bold;"></asp:Label>
                    <asp:Label ID="lbContributerHeaderForTitle" runat="server" Text="" style="font-size:18px;color:Purple;font-weight: bold;"></asp:Label>
                    <asp:Label ID="lbContributerHeaderForFooter" runat="server" Text="" style="font-size:15px;color:Black;font-weight: bold;"></asp:Label>
                </td>
            </tr>
            <tr align="left">
                <td>
                    <br />
                </td>
            </tr>
            <tr align="left">
                <th>
                    お名前
                </th>
                <td>
                    <asp:TextBox ID="tbName" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr align="left">
                <th>
                    Ｅメール
                </th>
                <td>
                    <asp:TextBox ID="tbEMail" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr align="left">
                <th>
                    タイトル
                </th>
                <td>
                    <asp:TextBox ID="tbTitle" runat="server"></asp:TextBox><asp:Button ID="btnContribute"
                        runat="server" Text="返信する" OnClick="btnContribute_Click" /><asp:Button ID="btnClear"
                            runat="server" Text="クリア" />
                </td>
            </tr>
            <tr>
                <th>
                    <br />
                </th>
            </tr>
            <tr align="left">
                <th>
                    コメント
                </th>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:TextBox ID="tbComment" runat="server" Width="300" Height="150" TextMode="MultiLine"></asp:TextBox>
                </td>
            </tr>
            <tr align="left">
                <th>
                    URL
                </th>
                <td>
                    <asp:TextBox ID="tbURL" runat="server"></asp:TextBox>
                </td>
            </tr>
            <%--スマートフォン・タブレット版はいったん見合わせることに--%>
            <%--            <tr align="left">
                <th>
                    添付File
                </th>
                <td>
                    <div>
                        <div>
                            <asp:Panel ID="Panel1" runat="server">
                                <asp:FileUpload ID="FileUpload1" runat="server" onClick="FileUpload1_onchange" />
                                <asp:Button
                                    ID="btnAdd" runat="server" Text="追加" OnClick="btnAdd_Click" UseSubmitBehavior="true"/>
                            </asp:Panel>
                        </div>
                    </div>
                </td>
                <td>
                </td>
            </tr>
            --%>
            <tr align="left">
                <th>
                    暗証キー
                </th>
                <td>
                    <asp:TextBox ID="tbPassword" runat="server" TextMode="Password" Text="1234"></asp:TextBox>
                </td>
            </tr>
            <tr align="left">
                <th>
                    文字色
                </th>
                <td>
                    <asp:RadioButton ID="rbBrown" runat="server" GroupName="Color" Checked="true" /><asp:Image
                        ID="Image1" runat="server" ImageUrl="~/Images/square/brown.png" Height="12px"
                        Width="12px" />
                    <asp:RadioButton ID="rbRed" runat="server" GroupName="Color" /><asp:Image ID="Image2"
                        runat="server" ImageUrl="~/Images/square/red.png" Height="12px" Width="12px" />
                    <asp:RadioButton ID="rbGreen" runat="server" GroupName="Color" /><asp:Image ID="Image3"
                        runat="server" ImageUrl="~/Images/square/green.png" Height="12px" Width="12px" />
                    <asp:RadioButton ID="rbBlue" runat="server" GroupName="Color" /><asp:Image ID="Image4"
                        runat="server" ImageUrl="~/Images/square/blue.png" Height="12px" Width="12px" />
                    <asp:RadioButton ID="rbPurple" runat="server" GroupName="Color" /><asp:Image ID="Image5"
                        runat="server" ImageUrl="~/Images/square/purple.png" Height="12px" Width="12px" />
                    <asp:RadioButton ID="rbPink" runat="server" GroupName="Color" /><asp:Image ID="Image6"
                        runat="server" ImageUrl="~/Images/square/pink.png" Height="12px" Width="12px" />
                    <asp:RadioButton ID="rbOrange" runat="server" GroupName="Color" /><asp:Image ID="Image7"
                        runat="server" ImageUrl="~/Images/square/orange.png" Height="12px" Width="12px" />
                    <asp:RadioButton ID="rbBlack" runat="server" GroupName="Color" /><asp:Image ID="Image8"
                        runat="server" ImageUrl="~/Images/square/brown.png" Height="12px" Width="12px" />
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
