using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;

public partial class BillWeb : System.Web.UI.Page
{
    private List<BillInfo> BillStock = new List<BillInfo>();
    
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["id"] == null || Session["id"].ToString().Length <= 0 || Session["isLogin"] == null || Session["isLogin"].ToString() != "1")
        {
            Response.Write("<script language='javascript'>alert('沒有登陸無法查看哦！'); location.href='Login.aspx';</script>");
            return;
        }
        
        if (!LoadBillInfo())
        {
            Response.Write("<script language='javascript'>alert('沒有購買信息！'); location.href='MainWeb.aspx';</script>");
            return;
        }

        DisplyBill();
    }

    private bool LoadBillInfo()
    {
        SqlConnection con = UserSqlConnect.createConnection();
        con.Open();

        SqlCommand cmd = new SqlCommand("select * from bill where owner=" + Session["id"].ToString(), con);
        SqlDataReader reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            BillInfo Info = new BillInfo();
            Info.Info_C = new ConcertInfo();
            Info.Info_S = new ScreeningsInfo();
            Info.Info_S.Seats = new List<SeatInfo>();

            Info.BillID = long.Parse(reader["id"].ToString());
            Info.Cost = float.Parse(reader["cost"].ToString());
            Info.Info_C.Name = reader["name"].ToString();
            Info.Info_S.ScreeningID = long.Parse(reader["screeningid"].ToString());
            Info.Info_S.Location = reader["location"].ToString();
            Info.Info_S.Time = reader["time"].ToString();

            List<string> SeatBuffer = new List<string>(reader["seats"].ToString().Trim().Split(' '));

            SeatBuffer.ForEach
            (
                delegate(string PositionBuffer)
                {
                    SeatInfo SInfo = new SeatInfo();
                    SInfo.Position = int.Parse(PositionBuffer);
                    Info.Info_S.Seats.Add(SInfo);
                }
            );

            BillStock.Add(Info);
        }
        con.Close();

        if (BillStock.Count <= 0)
            return false;

        return true;
    }

    private void DisplyBill()
    {
        BillStock.ForEach
        (
            delegate(BillInfo BInfo)
            {
                Panel PBill = new Panel();
                PBill.CssClass = "bill";
                PBill.GroupingText = "單號：" + BInfo.BillID;
                Button DelBtn = new Button();
                DelBtn.Text = "取消訂單";
                DelBtn.ID = BInfo.BillID + "#" + BInfo.Info_S.ScreeningID + "#";
                DelBtn.Click += new System.EventHandler(DelBtn_OnClick);
                Table TableMessage = new Table();
                TableCell[] Cells = new TableCell[5] { new TableCell(), new TableCell(), new TableCell(), new TableCell(), new TableCell() };
                TableRow[] Rows = new TableRow[5] { new TableRow(), new TableRow(), new TableRow(), new TableRow(), new TableRow() };
                Cells[0].Text = "音樂會：" + BInfo.Info_C.Name;
                Cells[1].Text = "地點：" + BInfo.Info_S.Location;
                Cells[2].Text = "時間：" + BInfo.Info_S.Time;
                Cells[3].Text = "座位：";
                BInfo.Info_S.Seats.ForEach(position => { Cells[3].Text += position.Position + "號 "; DelBtn.ID += position.Position + " "; });
                Cells[4].Text = "<br/>總價：" + BInfo.Cost;
                for (int i = 0; i < 5; i ++) Rows[i].Cells.Add(Cells[i]);
                TableMessage.Rows.AddRange(Rows);
                PBill.Controls.Add(TableMessage);
                PBill.Controls.Add(DelBtn);
                MainPanel.Controls.Add(PBill);
            }
        );
    }

    private void CancelBill(string message)
    {
        long id = long.Parse(message.Split('#')[0]);

        SqlConnection con = UserSqlConnect.createConnection();
        con.Open();
        SqlCommand cmd = new SqlCommand("delete from bill where id=" + id, con);
        cmd.ExecuteNonQuery();
        con.Close();

        con = InfoSqlConnect.createConnection();
        con.Open();

        long sid = long.Parse(message.Split('#')[1]);
        List<string> SeatsBuffer = new List<string>(message.Split('#')[2].Trim().Split(' '));
        SeatsBuffer.ForEach(strPosition => { cmd = new SqlCommand("update seats set avaliable='1' where avaliable='0' and screeningid=" + sid + "and position=" + strPosition, con); cmd.ExecuteNonQuery(); });
        con.Close();
    }

    protected void DelBtn_OnClick(object sender, EventArgs e)
    {
        Button DelBtn = sender as Button;
        CancelBill(DelBtn.ID);
        Response.Write("<script language='javascript'>alert('已取消訂單！');location.href='BillWeb.aspx';</script>");
    }
}