using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;

public partial class Login : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["id"] != null && Session["id"].ToString().Length > 0 &&
            Session["isLogin"] != null && Session["isLogin"].ToString() == "1")
        {
            Response.Redirect("MainWeb.aspx");
        }
       
    }

    protected void BtnLogin_Click(object sender, EventArgs e)
    {
        if (TextBoxN.Text.Length <= 0)
        {
            Response.Write("<script language='javascript'>alert('用户名不能为空');</script>");
            return;
        }
        else if (TextBoxP.Text.Length < 6)
        {
            Response.Write("<script language='javascript'>alert('密码错误');</script>");
            return;
        }
        SqlConnection con = UserSqlConnect.createConnection();
        con.Open();
        SqlCommand cmd = new SqlCommand("select * from [user] where name='" + TextBoxN.Text + "' and pw='"+TextBoxP.Text+"'",con);
        SqlDataReader reader = cmd.ExecuteReader();
        if(reader.Read()){
            Session["id"] = reader["id"].ToString();
            Session["isLogin"] = "1";
            Session["isAdmin"] = reader["authority"].ToString();
            Response.Redirect("MainWeb.aspx");
        }
        else
        {
            Response.Write("<script language='javascript'>alert('用户名或密码错误');</script>");
        }
        reader.Close();
        con.Close();
    }
}