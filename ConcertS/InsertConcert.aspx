<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InsertConcert.aspx.cs" Inherits="InsertConcert" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <script type="text/javascript" src="/js/insertconcert.js"></script>
    <form id="form1" runat="server" style ="background-image:url('../../image/listbg.jpg');">
        <asp:Label runat="server" Font-Size="30pt" Width="100%" style="text-align:center;" BorderStyle="None" Height="50px">音樂會信息注冊</asp:Label>
        
        <asp:Panel runat="server" style="width:50%; margin-left: 25%" GroupingText="音樂會信息" >
            海报:<br/>
            <asp:Image ID="ImagePoster" runat="server" Height="196px" Width="223px" />
            <br/>
            <asp:FileUpload ID="UploadImage" runat="server" onchange="Setpath(this)" />
            <br/><br/>

            音樂會编号：<br/>
            <asp:TextBox ID="TBId" runat="server" Width="100%" onkeyup="OnlyNumber(this)" onafterpaste="OnlyNumber(this)"/><br/><br/>

            名称：<br/>
            <asp:TextBox ID="TBName" runat="server" Width="100%"/><br/><br/>

            演員或團體：<br/>
            <asp:TextBox ID="TBActor" runat="server" Width="100%"/><br/><br/>

            介绍：<br/>
            <asp:TextBox ID="TBDescription" runat="server" Height="300px" TextMode="MultiLine" Width="100%"/>
        </asp:Panel>

        <asp:Panel runat="server" style="position:relative; width:50%; margin-left: 25%" GroupingText="場次信息" >
            放映列表：<br/>
            <asp:Table ID="TableScreenings" style="width:100%" runat="server" BorderStyle="Solid"/>
            <br/>
            場次編號：<br/><asp:TextBox ID="TBAddScreeningID" runat="server" Width="30%" onkeyup="OnlyNumber(this)" onafterpaste="OnlyNumber(this)"/><br/>
            地點：<br/><asp:TextBox ID="TBAddLocation" runat="server" Width="30%" MaxLength="16"/><br/>
            時間：<br/><asp:TextBox ID="TBAddTime" runat="server" Width="30%" MaxLength="16"/><br/>
            票价（元）：<br/><asp:TextBox ID="TBAddPrice" runat="server" Width="30%" onkeyup="OnlyNumber(this)" onafterpaste="OnlyNumber(this)" MaxLength="7"/><br/>
            座位数：<br/><asp:TextBox ID="TBAddSeat" runat="server" Width="30%" MaxLength="3"  onkeyup="OnlyNumber(this)" onafterpaste="OnlyNumber(this)"/><br/>
            <input type="button" id="BtnAddScreening" value="添加场次" onclick="add_screening()" /><br/>
            <textarea id="TBPer" runat="server" hidden="hidden" />
        </asp:Panel>

        <div style="width:100%;text-align:center;padding-top:2em;">
            <asp:Button ID="BtnSubmit" runat="server" Width="50%" Height="3em" Text="提交" OnClick="BtnSubmit_Click" />
        </div>
    </form>
</body>
</html>
