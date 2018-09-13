<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DetailWeb.aspx.cs" Inherits="DetailWeb" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <style type="text/css">
        #Checkbox1 {
            width: 20px;
        }
    </style>
</head>
<body>
    <script type="text/javascript" src="/js/detail.js"></script>
    <form id="form1" runat="server">
    <asp:Panel runat="server" style="width:25%; background: #c2b0b0" GroupingText="選擇購票" >
    
        選擇場次<br />
        <asp:DropDownList ID="pflist" runat="server" AutoPostBack="True" OnSelectedIndexChanged="pflist_SelectedIndexChanged">
        </asp:DropDownList><br />
        <asp:Label ID="TimeInfo" runat="server" Text=""/><br />
        <asp:Label ID="PriceInfo" runat="server" Text=""/><br />
        <asp:Panel runat="server" style="width:30%; background: #c2b0b0" GroupingText="請選擇位置" >
            <asp:Table ID="SeatsTable" runat="server" BorderStyle="Solid" BackColor="#666699"/>
        </asp:Panel>
        <asp:Button ID="CheckButton" runat="server" OnClick="CheckButton_Click" style="height: 21px" Text="確定" />
        <br />
    </asp:Panel>
    </form>
</body>
</html>
