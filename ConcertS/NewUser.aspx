<%@ Page Language="C#" AutoEventWireup="true" CodeFile="NewUser.aspx.cs" Inherits="NewUser" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table class="inputtable" style="border-style: solid; left: 95px; position: relative; top: 7px; height: 122px;width:300px">
            <tr>
                <td align="center" class="auto-style1">
                        用户名：</td>
                <td align="center" class="auto-style1">
                        <asp:TextBox ID="TextBoxN" runat="server" MaxLength="10" TabIndex="1"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td align="center" class="auto-style1">
                        密码：</td>
                <td align="center" class="auto-style1">
                        <asp:TextBox ID="TextBoxP" runat="server" MaxLength="20" TabIndex="2" TextMode="Password"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td align="center" class="auto-style1">
                        确认密码：</td>
                <td align="center" class="auto-style1">
                        <asp:TextBox ID="TextBoxPR" runat="server" MaxLength="20" TabIndex="2" TextMode="Password"></asp:TextBox>
                </td>
            </tr>
            </table>
    </div>
        <asp:Button ID="ButtonReg" runat="server" Text="注册" style="left: 95px; position: relative; top: 30px; height: 50px;width:300px" OnClick="ButtonReg_Click"/>
    </form>
</body>
</html>
