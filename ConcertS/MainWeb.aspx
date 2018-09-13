<%@ Page Language="C#" AutoEventWireup="true" CodeFile="MainWeb.aspx.cs" Inherits="MainWeb" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <style type="text/css">
        #Button1 {
            height: 39px;
        }
    </style>
</head>
<body>

    <link rel="stylesheet" type="text/css" href="css/main.css" />
    <form id="form1" runat="server">
        <div class="header">
                音樂會搜索：
                <asp:TextBox ID="TbSearch" Font-Size="15pt" runat="server" Width="15em"/>
                <asp:Button ID="BtnSearch" Font-Size="13pt" Text="搜尋" runat="server" Width="3em" OnClick="BtnSearch_Click"/>
                <asp:Button ID="BtnUser" runat="server" Text="尚未登陸" OnClick="BtnUser_Click" Height="25px" Width="86px" />
        </div>
        <div class="concert_more">
            <asp:Button ID="BtnMore" runat="server" Text="下一頁" Font-Size="15pt" Height="40px" OnClick="Button1_Click" Width="30em"/><br/>
            <asp:Button ID="BtnBill" Font-Size="13pt" Text="訂單查詢" runat="server" Width="8em" OnClick="BtnBill_Click"/>
            <asp:Button ID="BtnInsert" Font-Size="13pt" Text="注冊音樂會信息" runat="server" Width="8em" OnClick="BtnInsert_Click" Visible="False"/>
            <asp:Button ID="BtnEditor" Font-Size="13pt" Text="編輯音樂會信息" runat="server" Width="8em" OnClick="BtnEditor_Click" Visible="False"/>
        </div>
        </form>
</body>
</html>
