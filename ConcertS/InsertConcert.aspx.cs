using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;

public partial class InsertConcert : System.Web.UI.Page
{

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["id"] == null || Session["id"].ToString().Length <= 0 || Session["isLogin"] == null || Session["isLogin"].ToString() != "1" || Session["isAdmin"] == null || Session["isAdmin"].ToString() != "1")
            Response.Write("<script language='javascript'>alert('不是管理員無法操作哦！'); location.href='Login.aspx';</script>");
    }

    private bool checkNotEmpty()
    {
        if (TBId.Text == null || TBId.Text.Length <= 0)
        {
            Response.Write("<script language='javascript'>alert('音樂會編號不能爲空');</script>");
            return false;
        }

        if (TBName.Text == null || TBName.Text.Length <= 0)
        {
            Response.Write("<script language='javascript'>alert('名字不能留空');</script>");
            return false;
        }

        if (TBPer.Value == null || TBPer.Value.Length <= 0)
        {
            Response.Write("<script language='javascript'>alert('請添加場次信息');</script>");
            return false;
        }

        return true;
    }

    protected void BtnSubmit_Click(object sender, EventArgs e)
    {
        if (!checkNotEmpty())
            return;

        if (ConcertInsert() && InsertScreeningsAndSeats())
        {
            Response.Write("<script language='javascript'>alert('添加完成！');self.location='InsertConcert.aspx'</script>");
            return;
        }

        Response.Write("<script language='javascript'>alert('添加失敗，可能有相同的音樂編號或場次編號哦！');</script>");
    }

    private bool ConcertInsert()
    {
        SqlConnection con = InfoSqlConnect.createConnection();
        con.Open();
        ConcertInfo info = new ConcertInfo();
        info.ConcertID = long.Parse(TBId.Text);
        info.Name = TBName.Text;
        info.Description = TBDescription.Text;
        info.Actor = TBActor.Text;
        if (UploadImage.FileName == null || UploadImage.FileName.Length <= 0) info.Poster = "";
        else info.Poster = "image/poster/" + UploadImage.FileName;

        SqlCommand cmd = new SqlCommand("insert into concerts values(" +
        info.ConcertID + ",'" +
        info.Name + "','" +
        info.Description + "','" +
        info.Actor + "','" +
        info.Poster + "')", con);

        int res = -1;
        try
        {
            res = cmd.ExecuteNonQuery();
        }
        catch
        {
            return false;
        }
        if (res < 0)
            return false;
        return true;
    }

    private bool InsertScreeningsAndSeats()
    {   
        DataTable dataTableScreenings = new DataTable();
        dataTableScreenings.Columns.AddRange(new DataColumn[]{
            new DataColumn("id",typeof(long)),
            new DataColumn("screeningid",typeof(long)),
            new DataColumn("location",typeof(string)),
            new DataColumn("time",typeof(string)),
            new DataColumn("price",typeof(float))
        });

        DataTable dataTableSeats = new DataTable();
        dataTableSeats.Columns.AddRange(new DataColumn[]{
            new DataColumn("screeningid",typeof(long)),
            new DataColumn("avaliable",typeof(char)),
            new DataColumn("position",typeof(int)),
        });

        int AllSeats = 0;
        long concertid = long.Parse(TBId.Text);
        string[] lines = TBPer.Value.Split('\n');
        for (int i = 0; i < lines.Length; i++)
        {
            if (lines[i].Length <= 0)
            continue;

            string[] args = lines[i].Split('#');
            long screeningsid = long.Parse(args[0]);
            string location = args[1];
            string time = args[2];
            float price = float.Parse(args[3]);
            int seatCount = int.Parse(args[4]);
            AllSeats += seatCount;

            DataRow rowScreening = dataTableScreenings.NewRow();
            rowScreening[0] = concertid;
            rowScreening[1] = screeningsid;
            rowScreening[2] = location;
            rowScreening[3] = time;
            rowScreening[4] = price;
            dataTableScreenings.Rows.Add(rowScreening);
            
            for (int j = 1; j <= seatCount; j++)
            {
                DataRow rowSeat = dataTableSeats.NewRow();
                rowSeat[0] = screeningsid;
                rowSeat[1] = '1';
                rowSeat[2] = j;
                dataTableSeats.Rows.Add(rowSeat);
            }

        }

        SqlConnection con = InfoSqlConnect.createConnection();
        SqlBulkCopy bulkCopy = new SqlBulkCopy(con);
        con.Open();
        bulkCopy.DestinationTableName = "screenings";
        bulkCopy.BatchSize = lines.Length;

        try
        {
            if (dataTableScreenings.Rows.Count > 0)
                bulkCopy.WriteToServer(dataTableScreenings);
        }
        catch
        {
            con.Close();
            return false;
        }

        bulkCopy.DestinationTableName = "seats";
        bulkCopy.BatchSize = AllSeats;
        bulkCopy.WriteToServer(dataTableSeats);
        
        con.Close();

        return true;
    }
}