<%@ Page Language="C#" AutoEventWireup="true" CodeFile="BillWeb.aspx.cs" Inherits="BillWeb" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    </head>
<body>
    <form id="form1" runat="server" style ="background-image:url('../../image/listbg.jpg');">
        <link rel="stylesheet" type="text/css" href="css/main.css" />
        <asp:Label runat="server" Font-Size="30pt" Width="100%" style="text-align:center;" BorderStyle="None" Height="50px">訂單</asp:Label>
        <asp:Panel ID="MainPanel" runat="server" style="width:100%; margin-left: 0%">
        </asp:Panel>
    </form>
</body>
</html>
