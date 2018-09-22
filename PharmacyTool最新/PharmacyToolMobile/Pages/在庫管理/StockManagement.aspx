<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="StockManagement.aspx.cs"
    Inherits="PharmacyToolMobile.Pages.在庫管理.StockManagement" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>PharmacyToolMobile 在庫管理</title>
    <style type="text/css">
        #options, #buttons, #input
        {
            text-align: right;
        }
        
        body
        {
            background-color: #ffccff;
        }
    </style>
    <script type="text/javascript">
        function funcKeyDown() {
            var o = event.srcElement;
            var v = o.value;
            var k = event.keyCode;
            if (v == "" && k == 13) { //入力無し＆改行
                event.cancelBubble = true;
                event.returnValue = false;
                return false;
            }
            else if (k == 13) { //入力有り＆改行
                event.cancelBubble = true;
                event.returnValue = false;
                //キーダウンイベントの中でフォームSUBMITのボタン
                //を押した時の挙動が不明なのでとりあえず、時間差
                //で。問題無いなら、setTimeout を外す。
//                setTimeout(function () {
//                    document.getElementById("btn現在庫").click();
//                }, 100);

                var data = "<%=Session["data"]%>";

                window.alert(data);
                
                return false;
            }
            else { //改行以外の入力はそのまま受付
                return true;
            }
        }
</script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:Button ID="btn掲示板" runat="server" Text="掲  示  板" OnClick="btn掲示板_Click" BackColor="Pink"
            Width="150" Height="30" ForeColor="Black" /><asp:Button ID="btn在庫管理" runat="server"
                Text="在 庫 管 理" OnClick="btn在庫管理_Click" BackColor="Pink" Width="150" Height="30"
                ForeColor="Red" />
    </div>
    <div />
    <div>
        <asp:Panel ID="Panel1" runat="server" Visible="true">
            <div id="options">
                使用期限日：
                <asp:DropDownList ID="ddl使用期限日" runat="server" Width="195">
                    <asp:ListItem Selected="True">全期間</asp:ListItem>
                    <asp:ListItem>期限内</asp:ListItem>
                    <asp:ListItem>期限切</asp:ListItem>
                    <asp:ListItem>期限１ヶ月以内</asp:ListItem>
                    <asp:ListItem>期限１ヶ月以上</asp:ListItem>
                    <asp:ListItem>期限２ヶ月以内</asp:ListItem>
                    <asp:ListItem>期限２ヶ月以上</asp:ListItem>
                    <asp:ListItem>期限３ヶ月以内</asp:ListItem>
                    <asp:ListItem>期限３ヶ月以上</asp:ListItem>
                    <asp:ListItem>期限４ヶ月以内</asp:ListItem>
                    <asp:ListItem>期限４ヶ月以上</asp:ListItem>
                    <asp:ListItem>期限５ヶ月以内</asp:ListItem>
                    <asp:ListItem>期限５ヶ月以上</asp:ListItem>
                    <asp:ListItem>期限６ヶ月以内</asp:ListItem>
                    <asp:ListItem>期限６ヶ月以上</asp:ListItem>
                    <asp:ListItem>期限７ヶ月以内</asp:ListItem>
                    <asp:ListItem>期限７ヶ月以上</asp:ListItem>
                    <asp:ListItem>期限８ヶ月以内</asp:ListItem>
                    <asp:ListItem>期限８ヶ月以上</asp:ListItem>
                    <asp:ListItem>期限９ヶ月以内</asp:ListItem>
                    <asp:ListItem>期限９ヶ月以上</asp:ListItem>
                    <asp:ListItem>期限１０ヶ月以内</asp:ListItem>
                    <asp:ListItem>期限１０ヶ月以上</asp:ListItem>
                    <asp:ListItem>期限１１ヶ月以内</asp:ListItem>
                    <asp:ListItem>期限１１ヶ月以上</asp:ListItem>
                    <asp:ListItem>期限１２ヶ月以内</asp:ListItem>
                    <asp:ListItem>期限１２ヶ月以上</asp:ListItem>
                </asp:DropDownList>
                <div>
                    使用量検索：
                    <asp:DropDownList ID="ddl使用量検索" runat="server" Width="195">
                        <asp:ListItem Selected="True">全期間</asp:ListItem>
                        <asp:ListItem>過去１ヶ月以内</asp:ListItem>
                        <asp:ListItem>過去２ヶ月以内</asp:ListItem>
                        <asp:ListItem>過去３ヶ月以内</asp:ListItem>
                        <asp:ListItem>過去４ヶ月以内</asp:ListItem>
                        <asp:ListItem>過去５ヶ月以内</asp:ListItem>
                        <asp:ListItem>過去６ヶ月以内</asp:ListItem>
                        <asp:ListItem>過去７ヶ月以内</asp:ListItem>
                        <asp:ListItem>過去８ヶ月以内</asp:ListItem>
                        <asp:ListItem>過去９ヶ月以内</asp:ListItem>
                        <asp:ListItem>過去１０ヶ月以内</asp:ListItem>
                        <asp:ListItem>過去１１ヶ月以内</asp:ListItem>
                        <asp:ListItem>過去１２ヶ月以内</asp:ListItem>
                    </asp:DropDownList>
                </div>
                <div>
                    使用期限色：
                    <asp:DropDownList ID="ddl使用期限色" runat="server" Width="195">
                        <asp:ListItem>１ヶ月以内</asp:ListItem>
                        <asp:ListItem>２ヶ月以内</asp:ListItem>
                        <asp:ListItem Selected="True">３ヶ月以内</asp:ListItem>
                        <asp:ListItem>４ヶ月以内</asp:ListItem>
                        <asp:ListItem>５ヶ月以内</asp:ListItem>
                        <asp:ListItem>６ヶ月以内</asp:ListItem>
                        <asp:ListItem>７ヶ月以内</asp:ListItem>
                        <asp:ListItem>８ヶ月以内</asp:ListItem>
                        <asp:ListItem>９ヶ月以内</asp:ListItem>
                        <asp:ListItem>１０ヶ月以内</asp:ListItem>
                        <asp:ListItem>１１ヶ月以内</asp:ListItem>
                        <asp:ListItem>１２ヶ月以内</asp:ListItem>
                    </asp:DropDownList>
                </div>
                <div>
                    後発品検索：
                    <asp:DropDownList ID="ddl後発品検索" runat="server" Width="195">
                        <asp:ListItem Selected="True">他規格・剤形は表示しない</asp:ListItem>
                        <asp:ListItem>他規格・剤形も表示する</asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>
        </asp:Panel>
    </div>
    <div id="input" onkeydown="funcKeyDown('<%= btn現在庫.ClientID %>')">
        <asp:TextBox ID="tb検索キーワード" runat="server" Text="" Width="283" />
    </div>
    <div id="buttons">
        <table border="0" width="100%" cellpadding="0px">
            <tr>
                <td align="left">
                    <asp:Label ID="lbSearchResult" runat="server" Text="" ForeColor="Red"></asp:Label>
                </td>
                <td align="right">
                    <asp:Button ID="btn現在庫" runat="server" Text="現在庫" Width="72" OnClick="btn現在庫_Click"
                        BackColor="LightBlue" /><asp:Button ID="btn使用量" runat="server" Text="使用量" Width="72"
                            OnClick="btn使用量_Click" BackColor="LightBlue" /><asp:Button ID="btn不動品" runat="server"
                                Text="不動品" Width="72" OnClick="btn不動品_Click" BackColor="LightBlue" /><asp:Button
                                    ID="btn後発品" runat="server" Text="後発品" Width="72" OnClick="btn後発品_Click" BackColor="LightBlue" />
                </td>
            </tr>
        </table>
    </div>
    <div>
        <asp:DataGrid ID="dg現在庫" runat="server" OnItemDataBound="dg現在庫_ItemBound" AutoGenerateColumns="false"
            Visible="true" EnableViewState="False">
            <HeaderStyle BackColor="Azure" ForeColor="Black" />
            <ItemStyle BackColor="White" />
            <AlternatingItemStyle BackColor="LightGray" />
            <Columns>
                <asp:BoundColumn DataField="店名" HeaderText="店名">
                    <ItemStyle Width="13%" />
                </asp:BoundColumn>
                <asp:BoundColumn DataField="個別医薬品コード" HeaderText="個別医薬品コード">
                    <ItemStyle Width="17%" />
                </asp:BoundColumn>
                <%--<asp:BoundColumn DataField="医薬品名" HeaderText="医薬品名"--%>
                <asp:BoundColumn DataField="医薬品名と名称２連結" HeaderText="医薬品名">
                    <ItemStyle Width="18%" />
                </asp:BoundColumn>
                <asp:BoundColumn DataField="在庫数" HeaderText="在庫数">
                    <ItemStyle Width="11%" />
                </asp:BoundColumn>
                <asp:BoundColumn DataField="使用期限" HeaderText="使用期限日">
                    <ItemStyle Width="10%" />
                </asp:BoundColumn>
                <asp:BoundColumn DataField="薬価" HeaderText="薬価">
                    <ItemStyle Width="8%" />
                </asp:BoundColumn>
                <asp:BoundColumn DataField="製造会社" HeaderText="メーカー">
                    <ItemStyle Width="13%" />
                </asp:BoundColumn>
                <asp:BoundColumn DataField="最終更新日時" HeaderText="最終更新日時">
                    <ItemStyle Width="10%" />
                </asp:BoundColumn>
            </Columns>
        </asp:DataGrid>
        <asp:DataGrid ID="dg使用量" runat="server" OnItemDataBound="dg使用量_ItemBound" AutoGenerateColumns="false"
            Visible="false" EnableViewState="False">
            <HeaderStyle BackColor="Azure" ForeColor="Black" />
            <ItemStyle BackColor="White" />
            <AlternatingItemStyle BackColor="LightGray" />
            <Columns>
                <asp:BoundColumn DataField="店名" HeaderText="店名">
                    <ItemStyle Width="16%" />
                </asp:BoundColumn>
                <asp:BoundColumn DataField="使用年月" HeaderText="使用年月">
                    <ItemStyle Width="16%" />
                </asp:BoundColumn>
                <%--<asp:BoundColumn DataField="医薬品名" HeaderText="医薬品名">--%>
                <asp:BoundColumn DataField="医薬品名と名称２連結" HeaderText="医薬品名">
                    <ItemStyle Width="26%" />
                </asp:BoundColumn>
                <asp:BoundColumn DataField="使用量" HeaderText="使用量">
                    <ItemStyle Width="13%" />
                </asp:BoundColumn>
                <asp:BoundColumn DataField="薬価" HeaderText="薬価">
                    <ItemStyle Width="13%" />
                </asp:BoundColumn>
                <asp:BoundColumn DataField="最終更新日時" HeaderText="最終更新日時">
                    <ItemStyle Width="16%" />
                </asp:BoundColumn>
            </Columns>
        </asp:DataGrid>
        <asp:DataGrid ID="dg不動品" runat="server" OnItemDataBound="dg不動品_ItemBound" AutoGenerateColumns="false"
            Visible="false" EnableViewState="False">
            <HeaderStyle BackColor="Azure" ForeColor="Black" />
            <ItemStyle BackColor="White" />
            <AlternatingItemStyle BackColor="LightGray" />
            <Columns>
                <asp:BoundColumn DataField="店名" HeaderText="店名">
                    <ItemStyle Width="16%" />
                </asp:BoundColumn>
                <asp:BoundColumn DataField="個別医薬品コード" HeaderText="個別医薬品コード">
                    <ItemStyle Width="16%" />
                </asp:BoundColumn>
<%--                <asp:BoundColumn DataField="医薬品名" HeaderText="医薬品名">
--%>                <asp:BoundColumn DataField="医薬品名と名称２連結" HeaderText="医薬品名">
                    <ItemStyle Width="26%" />
                </asp:BoundColumn>
                <asp:BoundColumn DataField="在庫数" HeaderText="在庫数">
                    <ItemStyle Width="13%" />
                </asp:BoundColumn>
                <asp:BoundColumn DataField="使用期限" HeaderText="使用期限日">
                    <ItemStyle Width="13%" />
                </asp:BoundColumn>
                <asp:BoundColumn DataField="薬価" HeaderText="薬価" />
                <asp:BoundColumn DataField="最終更新日時" HeaderText="最終更新日時">
                    <ItemStyle Width="16%" />
                </asp:BoundColumn>
            </Columns>
        </asp:DataGrid>
        <asp:DataGrid ID="dg後発品" runat="server" OnItemDataBound="dg後発品_ItemBound" AutoGenerateColumns="false"
            Visible="false" EnableViewState="False">
            <HeaderStyle BackColor="Azure" ForeColor="Black" />
            <ItemStyle BackColor="White" />
            <AlternatingItemStyle BackColor="LightGray" />
            <Columns>
                <asp:BoundColumn DataField="店名" HeaderText="店名">
                    <ItemStyle Width="11%" />
                </asp:BoundColumn>
                <asp:BoundColumn DataField="後発区分" HeaderText="先発品">
                    <ItemStyle Width="8%" HorizontalAlign="Center" />
                </asp:BoundColumn>
                <asp:BoundColumn DataField="個別医薬品コード" HeaderText="個別医薬品コード">
                    <ItemStyle Width="13%" />
                </asp:BoundColumn>
                <%--<asp:BoundColumn DataField="医薬品名" HeaderText="医薬品名">--%>
                <asp:BoundColumn DataField="医薬品名と名称２連結" HeaderText="医薬品名">
                    <ItemStyle Width="16%" />
                </asp:BoundColumn>

                <asp:BoundColumn DataField="在庫数" HeaderText="在庫数">
                    <ItemStyle Width="10%" />
                </asp:BoundColumn>
                <asp:BoundColumn DataField="使用期限" HeaderText="使用期限日">
                    <ItemStyle Width="10%" />
                </asp:BoundColumn>
                <asp:BoundColumn DataField="薬価" HeaderText="薬価">
                    <ItemStyle Width="8%" />
                </asp:BoundColumn>
                <asp:BoundColumn DataField="製造会社" HeaderText="メーカー">
                    <ItemStyle Width="13%" />
                </asp:BoundColumn>
                <asp:BoundColumn DataField="最終更新日時" HeaderText="最終更新日時">
                    <ItemStyle Width="11%" />
                </asp:BoundColumn>
            </Columns>
        </asp:DataGrid>
    </div>
    </form>
</body>
</html>
