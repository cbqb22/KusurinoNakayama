<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BBS.aspx.cs" Inherits="PharmacyToolMobile.Pages.掲示板.BBS" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>PharmacyToolMobile 掲示板</title>
    <style type="text/css">
        body
        {
            background-color: #ffccff;
        }
        
        #BBSHeader
        {
            font-size: 23px;
            color: #5F9EA0;
            text-align: center;
        }
        
        #NewContributer
        {
        }
        
        .tbContribute
        {
            border: 1px solid #000;
        }
        td
        {
            border-width: 0;
        }
        
        
        
        .MainNo
        {
            font-size: 18px;
            color: Blue;
            font-weight: bold;
        }
        
        .MainTitle
        {
            font-size: 18px;
            color: Purple;
            font-weight: bold;
        }
        
        
        .MainPersonNameHeader
        {
            font-size: 15px;
            color: Black;
        }
        
        
        .MainPersonName
        {
            font-size: 16px;
            color: Purple;
            font-weight: bold;
        }
        
        .MainDateHeader
        {
            font-size: 15px;
            color: Black;
        }
        
        
        .MainDate
        {
            font-size: 15px;
            color: Black;
            font-weight: bold;
        }
        
        
        .SubNo
        {
            font-size: 16px;
            color: Blue;
        }
        
        .SubTitle
        {
            font-size: 16px;
            color: Orange;
        }
        
        
        .SubPersonNameHeader
        {
            font-size: 15px;
            color: Black;
        }
        
        
        .SubPersonName
        {
            font-size: 16px;
            color: Purple;
            font-weight: bold;
        }
        
        .SubDateHeader
        {
            font-size: 15px;
            color: Black;
        }
        
        
        .SubDate
        {
            font-size: 15px;
            color: Black;
        }
        
        .pnLinkButton
        {
            text-align: center;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:Button ID="btn掲示板" runat="server" Text="掲  示  板" OnClick="btn掲示板_Click" BackColor="Pink"
            Width="150" Height="30" ForeColor="Red" /><asp:Button ID="btn在庫管理" runat="server"
                Text="在 庫 管 理" OnClick="btn在庫管理_Click" BackColor="Pink" Width="150" Height="30"
                ForeColor="Black" />
    </div>
    <div id="BBSHeader">
        調 剤 部 門 連 絡 掲 示 板</div>
    <div>
        <table width="100%" cellpadding="0">
            <tr>
                <td valign="bottom">
                    <asp:DropDownList ID="ddlスレッド選択" runat="server" Width="200" Height="30" AutoPostBack="true"
                        OnSelectedIndexChanged="ddlスレッド選択_SelectedChanged">
                    </asp:DropDownList>
                </td>
                <%--        <asp:Button ID="btnThreadMove" runat="server" Text="スレッド移動" BackColor="LightBlue"
            OnClick="btnスレッド移動_Click" />
        <asp:Button ID="btn新規投稿" runat="server" Text="新規投稿" BackColor="LightBlue" OnClick="btn新規投稿_Click" />
                --%>
                <td align="right">
                    <asp:Label runat="server" ID="lbAccessCount" Style="font-size: 30px; color: #0066ff;
                        font-weight: bold;" />
                </td>
            </tr>
        </table>
    </div>
    <div>
        <asp:Panel ID="Panel1" runat="server">
            <asp:Panel ID="pn" runat="server">
            </asp:Panel>
        </asp:Panel>
        <asp:Panel ID="Panel4" runat="server">
        </asp:Panel>
    </div>
    <div>
        <asp:Panel ID="Panel3" runat="server">
            <asp:Panel ID="pnLinkButton" runat="server" CssClass="pnLinkButton">
            </asp:Panel>
        </asp:Panel>
    </div>
    <div id="NewContributer" align="center">
        <table border="0">
            <tr>
                <th>
                    <br />
                </th>
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
                        runat="server" Text="投稿する" OnClick="btnContribute_Click" /><asp:Button ID="btnClear"
                            runat="server" Text="クリア" />
                </td>
            </tr>
            <tr align="left">
                <th>
                    コメント
                </th>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:TextBox ID="tbComment" runat="server" Width="400" Height="150" TextMode="MultiLine"></asp:TextBox>
                </td>
            </tr>
            <tr align="left">
                <th>
                    URL
                </th>
                <td>
                    <asp:TextBox ID="tbURL" runat="server" Width="332"></asp:TextBox>
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
                            <asp:Panel ID="Panel2" runat="server">
                                <%--                                <asp:FileUpload ID="FileUpload1" runat="server" onClick="FileUpload1_onchange" />
                                <asp:Button ID="btnAdd" runat="server" Text="追加" OnClick="btnAdd_Click" UseSubmitBehavior="true" />
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
                    <asp:TextBox ID="tbPassword" runat="server" TextMode="Password" Width="50" Text=""
                        MaxLength="8"></asp:TextBox>
                    (英数字8文字以内)
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
            <tr align="left">
                <td colspan="2" align="center">
                    <span style="color:#00ccff; font-size:12px;">-以下のフォームから自分の投稿記事を修正・削除することができます-</span>
                </td>
            </tr>
            <tr align="left">
                <td colspan="2">
                    <span>処理</span>
                    <asp:DropDownList ID="ddl処理" runat="server" Width="50">
                        <asp:ListItem Selected="True">修正</asp:ListItem>
                        <asp:ListItem>削除</asp:ListItem>
                    </asp:DropDownList>
                    <span>記事No:</span>
                    <asp:TextBox ID="tb記事No" runat="server" Width="50px"></asp:TextBox>
                    <span>暗証キー:</span>
                    <asp:TextBox ID="tb処理暗証キー" runat="server" Width="50px" TextMode="Password"></asp:TextBox>
                    <asp:Button ID="btn処理送信" runat="server" Text="送信" OnClick="btn処理送信_Click" />
                </td>
            </tr>
            <tr align="left">
                <td colspan="2">
                    <span style="color: Red">-キーワード検索</span>
                    <asp:TextBox ID="tbSearch" runat="server" Width="230px"></asp:TextBox><asp:Button
                        ID="btnSearch" runat="server" Text="検索" OnClick="btnSearch_Click" />
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
