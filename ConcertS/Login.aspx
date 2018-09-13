<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <style type="text/css">
        .auto-style1 {
            width: 145px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <table class="inputtable" style="border-style: solid; left: 95px; position: relative; top: 7px; height: 122px;width:300px">
            <tr>
                <td align="center" class="auto-style1">
                        用户名：</td>
                <td align="center" class="auto-style1">
                        <asp:TextBox ID="TextBoxN" runat="server" MaxLength="10" TabIndex="1" style="margin-left: 0px" Width="128px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td align="center" class="auto-style1">
                        密码：</td>
                <td align="center" class="auto-style1">
                        <asp:TextBox ID="TextBoxP" runat="server" MaxLength="20" TabIndex="2" TextMode="Password" Width="128px"></asp:TextBox>
                </td>
            </tr>
            </table>

    <table class="btntable"  border="0" style="border-style: none; left: 95px; position: relative; top: 7px;width:300px; height: 33px;">
        <tr>
                <td align="center" class="auto-style1">
                        <asp:Button ID="BtnLogin" runat="server" Text="登录" OnClick="BtnLogin_Click" TabIndex="3" Width="74px" /></td>
                <td align="center" class="auto-style1">
                <asp:Button ID="BtnReg" runat="server" Text="注册" PostBackUrl="~/NewUser.aspx" TabIndex="4" Width="74px" />
                </td>
            </tr>
    </table>
    </form>
</body>
</html>
