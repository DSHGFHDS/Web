using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;

public partial class DetailWeb : System.Web.UI.Page
{
    private long ConcertIndex;
    private List<ScreeningsInfo> ScStock = new List<ScreeningsInfo>();

    protected void Page_Load(object sender, EventArgs e)
    {
        if(!MethodLoad())
            Response.Write("<script language='javascript'>alert('加載失敗！'); location.href='MainWeb.aspx';</script>");
    }

    private bool MethodLoad()
    {
        string buffer = Request.QueryString["id"];
        if (buffer == null || buffer.Length <= 0)
            return false;

        if (Session["id"] == null || Session["id"].ToString().Length <= 0 || Session["isLogin"] == null || Session["isLogin"].ToString() != "1")
        {
            Response.Write("<script language='javascript'>alert('沒有登陸無法購買哦！'); location.href='Login.aspx';</script>");
            return false;
        }

        ConcertIndex = long.Parse(buffer);

        LoadSqlSceeningsInfo();
        
        if (!Page.IsPostBack)
        {
            string strSeleted = Request.QueryString["index"];
            if (strSeleted != null && strSeleted.Length > 0)
                pflist.SelectedIndex = int.Parse(strSeleted);
        }
        
        LoadSqlSeatsInfo();
        DisplaySeats();
        
        return true;
    }

    private void DisplaySeats()
    {
        List<TableRow> Rows = new List<TableRow>();
        int Line = 0;
        ScStock[pflist.SelectedIndex].Seats.ForEach
        (
            delegate(SeatInfo SInfo)
            {
                Line++;
                if (Line == 1)
                    Rows.Add(new TableRow());
                else 
                if (Line == 10)
                    Line = 0;

                SInfo.CBID = new CheckBox();
                if (!SInfo.IsAvaliable)
                {
                    SInfo.CBID.BackColor = System.Drawing.Color.Gray;
                    SInfo.CBID.Enabled = false;
                }

                TableCell Cell = new TableCell();
                Cell.Controls.Add(SInfo.CBID);
                Rows[Rows.Count-1].Cells.Add(Cell);
            }
        );

        SeatsTable.Rows.AddRange(Rows.ToArray());
    }

    private void LoadSqlSceeningsInfo()
    {
        SqlConnection SqlServer = InfoSqlConnect.createConnection();
        SqlServer.Open();
        SqlCommand cmd = new SqlCommand("select * from screenings where id=" + ConcertIndex, SqlServer);
        SqlDataReader reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            ScreeningsInfo Info = new ScreeningsInfo();
            Info.ScreeningID = long.Parse(reader["screeningid"].ToString());
            Info.Time = reader["time"].ToString();
            Info.Location = reader["location"].ToString();
            Info.Price = float.Parse(reader["price"].ToString());
            Info.Seats = new List<SeatInfo>();
            ScStock.Add(Info);
            pflist.Items.Add(Info.Location);
            TimeInfo.Text = "演出時間：" + Info.Time;
            PriceInfo.Text = "座位單價：" + Info.Price + "元";

        }

        SqlServer.Close();
    }

    private void LoadSqlSeatsInfo()
    {
        SqlConnection SqlServer = InfoSqlConnect.createConnection();
        SqlServer.Open();

        SqlCommand cmd = new SqlCommand("select * from seats where screeningid=" + ScStock[pflist.SelectedIndex].ScreeningID, SqlServer);
        SqlDataReader reader = cmd.ExecuteReader();

        ScStock[pflist.SelectedIndex].Seats = new List<SeatInfo>();
        while (reader.Read())
        {
            SeatInfo Info = new SeatInfo();
            Info.Position = int.Parse(reader["position"].ToString());
            Info.IsAvaliable = reader["avaliable"].ToString() == "1";
            ScStock[pflist.SelectedIndex].Seats.Add(Info);
            
        }
        SqlServer.Close();
    }

    private List<int> CheckAvaliable()
    {
        List<int> SeatPicked = new List<int>();

        bool SaleOff = true;
        ScStock[pflist.SelectedIndex].Seats.ForEach
        (
            delegate(SeatInfo SInfo)
            {
                if (SInfo.CBID.Checked)
                    SeatPicked.Add(SInfo.Position);
                if (SInfo.IsAvaliable)
                    SaleOff = false;
            }
        );

        if (SaleOff)
        {
            Response.Write("<script language='javascript'>alert('該場票已售完哦！'); location.href='DetailWeb.aspx?id=" + ConcertIndex + "';</script>");
            return new List<int>();
        }

        if (SeatPicked.Count < 1)
        {
            Response.Write("<script language='javascript'>alert('還沒有選票哦！');</script>");
            return new List<int>();
        }

        

        SqlConnection SqlServer = InfoSqlConnect.createConnection();
        SqlServer.Open();

        bool arror = false;
        SeatPicked.ForEach
        (
            delegate(int index)
            {

                SqlCommand cmd = new SqlCommand("select * from seats where screeningid=" + ScStock[pflist.SelectedIndex].ScreeningID + "and position=" + index, SqlServer);
                SqlDataReader reader = cmd.ExecuteReader();
                reader.Read();
                if (reader["avaliable"].ToString() != "1")
                {
                    arror = true;
                    return;
                }
                reader.Close();
            }
        );

        SqlServer.Close();
        if(arror)
        return new List<int>();
        
        return SeatPicked;
    }

    protected void CheckButton_Click(object sender, EventArgs e)
    {
        MakeSure();
    }

    private void MakeSure()
    {
        List<int> Tackets = CheckAvaliable();
        if (Tackets.Count == 0)
            return;

        SqlConnection con = InfoSqlConnect.createConnection();
        con.Open();
        SqlCommand cmd = new SqlCommand("select name from concerts where id=" + ConcertIndex, con);
        SqlDataReader reader = cmd.ExecuteReader();
        reader.Read();
        string NameBuffer = reader["name"].ToString();
        string LocationBuffer = ScStock[pflist.SelectedIndex].Location;
        string TimeBuffer = ScStock[pflist.SelectedIndex].Time;
        float PriceBuffer = ScStock[pflist.SelectedIndex].Price;
        string SeatsBuffer = "";
        string PrintBuffer = "音樂會名稱：" + NameBuffer + "\\n";

        reader.Close();

        PrintBuffer += "演出地點：" + LocationBuffer + "\\n";
        PrintBuffer += "演出時間：" + TimeBuffer + "\\n";
        PrintBuffer += "售票單價：" + PriceBuffer + "\\n";
        PrintBuffer += "已購買座位爲：";

        Tackets.ForEach
        (
            delegate(int index)
            {
                SeatsBuffer += index + " ";
                cmd = new SqlCommand("update seats set avaliable='0' where avaliable='1' and screeningid=" + ScStock[pflist.SelectedIndex].ScreeningID + "and position=" + index, con);
                cmd.ExecuteNonQuery();
            }
        );
        con.Close();

        CreateBill(long.Parse(Session["id"].ToString()), NameBuffer, ScStock[pflist.SelectedIndex].ScreeningID, ScStock[pflist.SelectedIndex].Location, ScStock[pflist.SelectedIndex].Time, SeatsBuffer, ScStock[pflist.SelectedIndex].Price * Tackets.Count);

        PrintBuffer += SeatsBuffer + "\\n";
        PrintBuffer += "總價：" + ScStock[pflist.SelectedIndex].Price * Tackets.Count + "元";

        Response.Write("<script language='javascript'>alert('" + PrintBuffer + "'); location.href='DetailWeb.aspx?id=" + ConcertIndex + "&index=" + pflist.SelectedIndex + "'</script>");
    }

    private void CreateBill(long userid, string strName, long screeningid, string strLocation, string strTime, string strSeats, float fCost)
    {
        SqlConnection con = UserSqlConnect.createConnection();
        con.Open();
        SqlCommand cmd = new SqlCommand("insert into [bill](owner,name,screeningid,location,time,seats,cost) values(" +
        userid + ",'" +
        strName + "'," +
        screeningid + ",'" +
        strLocation + "','" +
        strTime + "','" +
        strSeats + "'," +
        fCost + ")", con);
        cmd.ExecuteNonQuery();
    }

    protected void pflist_SelectedIndexChanged(object sender, EventArgs e)
    {
        Response.Redirect("DetailWeb.aspx?id=" + ConcertIndex + "&index=" + pflist.SelectedIndex);
    }
}