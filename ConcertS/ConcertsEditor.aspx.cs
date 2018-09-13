using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;

public partial class ConcertsEditor : System.Web.UI.Page
{
    private List<ConcertInfo> Concerts = new List<ConcertInfo>();
    private List<List<ScreeningsInfo>> ScreeningStock = new List<List<ScreeningsInfo>>();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["id"] == null || Session["id"].ToString().Length <= 0 || Session["isLogin"] == null || Session["isLogin"].ToString() != "1" || Session["isAdmin"] == null || Session["isAdmin"].ToString() != "1")
        {
            Response.Write("<script language='javascript'>alert('不是管理員無法操作哦！'); location.href='Login.aspx';</script>");
            return;
        }
        
        string strid = Request.QueryString["id"];
        string strscreeningall = Request.QueryString["screeningall"];
        string strscreeningid = Request.QueryString["screeningid"];
        LoadConcerts();

        if (strid != null && strid.Length > 0)
        {
            if(strscreeningall != null && strscreeningall.Length >0)
            {
                int stockindex = int.Parse(strscreeningall);
                if (ScreeningStock[stockindex].Count > 0)
                    for (int j = 0; j < ScreeningStock[stockindex].Count; j++)
                        RemoveScreening(ScreeningStock[stockindex][j].ScreeningID);

            }
            RemoveConcert(long.Parse(strid));
            Response.Write("<script language='javascript'>alert('編號:" + strid + "音樂會刪除成功！'); location.href='ConcertsEditor.aspx';</script>");
        }

        if (strscreeningid != null && strscreeningid.Length > 0)
        {
            RemoveScreening(long.Parse(strscreeningid));
            Response.Write("<script language='javascript'>alert('編號:" + strscreeningid + "場次刪除成功！'); location.href='ConcertsEditor.aspx';</script>");
        }

        if (Concerts != null && Concerts.Count > 0)
            ShowList();
        else Response.Write("<script language='javascript'>alert('還沒有任何音樂會信息！'); location.href='InsertConcert.aspx';</script>");
    }

    private void LoadConcerts()
    {
        SqlConnection con = InfoSqlConnect.createConnection();
        con.Open();
        SqlCommand cmd = new SqlCommand("select id,name from concerts", con);
        SqlDataReader reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            ConcertInfo Info = new ConcertInfo();
            Info.ConcertID = long.Parse(reader["id"].ToString());
            Info.Name = reader["name"].ToString();
            Concerts.Add(Info);
            LoadScreenings(Info.ConcertID);
        }
        reader.Close();
        con.Close();
    }

    private void LoadScreenings(long id)
    {
        List<ScreeningsInfo> Screenings = new List<ScreeningsInfo>();
        SqlConnection con = InfoSqlConnect.createConnection();
        con.Open();
        SqlCommand cmd = new SqlCommand("select * from screenings where id=" + id, con);
        SqlDataReader reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            ScreeningsInfo Info = new ScreeningsInfo();
            Info.ScreeningID = long.Parse(reader["screeningid"].ToString());
            Info.Location = reader["location"].ToString();
            Info.Time = reader["time"].ToString();
            Screenings.Add(Info);
        }
        ScreeningStock.Add(Screenings);
        reader.Close();
        con.Close();
    }

    private void ShowList()
    {
        for (int i = 0; i < Concerts.Count; i++)
        {
            TableRow row = new TableRow();
            TableCell[] cells = new TableCell[3] { new TableCell(), new TableCell(), new TableCell() };
            cells[0].Text = Concerts[i].ConcertID.ToString();
            cells[1].Text = Concerts[i].Name;
            cells[2].Text = "<a href='?id=" + Concerts[i].ConcertID + "&screeningall=" + i + "'>删除</a>";
            row.Cells.AddRange(cells);
            row.BackColor = System.Drawing.Color.FromName("#ffb400");
            TableConcerts.Rows.Add(row);

            if (ScreeningStock[i].Count <= 0)
            continue;

            ScreeningStock[i].ForEach
            (
                delegate(ScreeningsInfo SSInfo)
                {
                    row = new TableRow();
                    cells = new TableCell[3] { new TableCell(), new TableCell(), new TableCell() };
                    cells[0].Text = SSInfo.Location;
                    cells[1].Text = SSInfo.Time;
                    cells[2].Text = "<a href='?screeningid=" + SSInfo.ScreeningID + "'>删除</a>";
                    row.Cells.AddRange(cells);
                    row.BackColor = System.Drawing.Color.LightSeaGreen;
                    TableConcerts.Rows.Add(row);
                }
            );
            if (i == Concerts.Count - 1)
                continue;
            
            row = new TableRow();
            TableCell Blank = new TableCell();
            Blank.Text = "0";
            row.Cells.Add(Blank);
            row.BackColor = System.Drawing.Color.Black;
            TableConcerts.Rows.Add(row);
        }
    }

    private void RemoveConcert(long id)
    {
        SqlConnection con = InfoSqlConnect.createConnection();
        con.Open();

        SqlCommand cmd = new SqlCommand("delete from concerts where id=" + id, con);
        cmd.ExecuteNonQuery();
    }

    private void RemoveScreening(long screeningid)
    {
        SqlConnection con = InfoSqlConnect.createConnection();
        con.Open();

        SqlCommand cmd = new SqlCommand("delete from seats where screeningid=" + screeningid, con);
        cmd.ExecuteNonQuery();
        cmd = new SqlCommand("delete from screenings where screeningid=" + screeningid, con);
        cmd.ExecuteNonQuery();

        con.Close();
    }
}