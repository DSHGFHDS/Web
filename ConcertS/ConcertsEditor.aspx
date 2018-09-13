<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ConcertsEditor.aspx.cs" Inherits="ConcertsEditor" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <asp:Label runat="server" Font-Size="30pt" Width="100%" style="text-align:center;" BorderStyle="None" Height="50px">已注冊音樂會信息</asp:Label>
    <asp:Panel runat="server" style="width:50%; margin-left: 25%" >
        <asp:Table ID="TableConcerts" BorderStyle="Solid" runat="server" width="100%">
        </asp:Table>
    </asp:Panel>
    </form>
</body>
</html>
