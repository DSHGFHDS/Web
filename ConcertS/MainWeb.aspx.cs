using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;

public partial class MainWeb : System.Web.UI.Page
{
    private const int CONCERT_PAGE_COUNT = 5;

    private const int SHORT_DESCRIPTION_COUNT = 100;

    private int nowPage = 1;
    private string strSearch;

    private bool isSearchMode = false;

    protected void Page_Load(object sender, EventArgs e)
    {
        strSearch = Request.QueryString["search"];
        if (strSearch != null && strSearch.Length > 0)
        {
            isSearchMode = true;
        }
        string strPage = Request.QueryString["page"];
        if (strPage != null && strPage.Length > 0)
        {
            nowPage = int.Parse(strPage);
        }

        int PageAcount;
        if (isSearchMode)
               PageAcount = getSearchForPage();
        else
            PageAcount = getConcertsForPage();

        if (PageAcount <= CONCERT_PAGE_COUNT)
        {
            BtnMore.Visible = false;
        }
        if (Session["id"] == null || Session["id"].ToString().Length <= 0 ||
            Session["isLogin"] == null || Session["isLogin"].ToString() != "1")
        {
            BtnUser.Text = "未登录";
        }
        else
        {
            string name = getUserNameById(Session["id"].ToString());
            if (name != null)
            {
                BtnUser.Text = name;
                BtnUser.Attributes["onclick"] = "javascript:return confirm('注销登录？');";

                if (Session["isAdmin"] != null && Session["isAdmin"].ToString() == "1")
                {
                    BtnEditor.Visible = true;
                    BtnInsert.Visible = true;
                }
            }
            else
            {
                Session["isLogin"] = "0";
                BtnUser.Text = "未登录";
            }
        }
    }

    private string getUserNameById(string id)
    {
        string name = null;
        SqlConnection con = UserSqlConnect.createConnection();
        con.Open();
        SqlCommand cmd = new SqlCommand("select name from [user] where id='" + id + "'", con);
        SqlDataReader reader = cmd.ExecuteReader();
        if (reader.Read())
            name = reader["name"].ToString();
        con.Close();
        return name;
    }

    private int getConcertsForPage()
    {
        return displayConcertsBySqlCommand("select top(" + (nowPage * CONCERT_PAGE_COUNT + 1) + ") * from concerts " +
        "where (id not in(select top(" + (nowPage - 1) * CONCERT_PAGE_COUNT + ") id from concerts))");
    }

    private int getSearchForPage()
    {
        return displayConcertsBySqlCommand("select top(" + (nowPage * CONCERT_PAGE_COUNT + 1) + ") * from concerts " +
        "where (id not in(select top(" + (nowPage - 1) * CONCERT_PAGE_COUNT + ") id from concerts where name like '%" + strSearch + "%') and name like '%" + strSearch + "%')");
    }

    private int displayConcertsBySqlCommand(string sql)
    {

        SqlConnection con = InfoSqlConnect.createConnection();
        con.Open();
        SqlCommand cmd = new SqlCommand(sql, con);

        SqlDataReader reader = cmd.ExecuteReader();
        int index = 0;

        Response.Write("<div class='concert_list'>");

        while (reader.Read())
        {
            if (index > CONCERT_PAGE_COUNT - 1)
            {
                index++;
                break;
            }

            ConcertInfo CInfo = new ConcertInfo();
            CInfo.ConcertID = long.Parse(reader["id"].ToString());
            CInfo.Name = reader["name"].ToString();
            CInfo.Poster = reader["poster"].ToString();
            CInfo.Description = reader["description"].ToString();
            CInfo.Actor = reader["actor"].ToString();

            string strShortDes = (CInfo.Description.Length <= SHORT_DESCRIPTION_COUNT) ?
                                      CInfo.Description : CInfo.Description.Substring(0, SHORT_DESCRIPTION_COUNT);

            Response.Write("<div class='home_poster'>" +
                                "<a href='DetailWeb.aspx?id=" + CInfo.ConcertID + "'>" +
                                "<img class='home_poster' src='/" + CInfo.Poster + "'/>" +
                                "</a></div>");

            Response.Write("<div class='home_text_info'>");

            Response.Write("<a href='DetailWeb.aspx?id=" + CInfo.ConcertID + "'>" +
                "<span class='concert_name'>" + CInfo.Name + "</span></a><br/>" +
                "表演：" + CInfo.Actor + "<br/>" +
                "<br/>" + strShortDes);

            Response.Write("</div>");


            index++;
        }

        Response.Write("</div>");
        reader.Close();
        con.Close();
        return index;
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        if (isSearchMode)
        {
            Response.Redirect("MainWeb.aspx?search=" + strSearch + "&page=" + (nowPage + 1));
        }
        else
        {
            Response.Redirect("MainWeb.aspx?page=" + (nowPage + 1));
        }
    }
    protected void BtnSearch_Click(object sender, EventArgs e)
    {
        if (TbSearch.Text.Length > 0)
            Response.Redirect("MainWeb.aspx?search=" + TbSearch.Text);
    }
    protected void BtnUser_Click(object sender, EventArgs e)
    {
        if (Session["isLogin"] == null || Session["isLogin"].ToString() != "1")
        {
            Response.Redirect("Login.aspx");
        }
        else
        {
            Session["isLogin"] = "0";
            Response.Redirect("MainWeb.aspx");
        }
    }

    protected void BtnInsert_Click(object sender, EventArgs e)
    {
        Response.Redirect("InsertConcert.aspx");
    }

    protected void BtnEditor_Click(object sender, EventArgs e)
    {
        Response.Redirect("ConcertsEditor.aspx");
    }

    protected void BtnBill_Click(object sender, EventArgs e)
    {
        Response.Redirect("BillWeb.aspx");
    }
}
