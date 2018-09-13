using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;

public partial class NewUser : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void ButtonReg_Click(object sender, EventArgs e)
    {
        if (TextBoxN.Text.Length < 3)
        {
            Response.Write("<script language='javascript'>alert('用户名不能小于3位');</script>");
            return;
        }
        else if (TextBoxP.Text.Length < 6)
        {
            Response.Write("<script language='javascript'>alert('密码不能小于6位');</script>");
            return;
        }
        else if (TextBoxP.Text != TextBoxPR.Text)
        {
            Response.Write("<script language='javascript'>alert('两次密码不一致');</script>");
            return;
        }

        SqlConnection con = UserSqlConnect.createConnection();
        con.Open();
        SqlCommand cmd = new SqlCommand("insert into [user](name,pw) values ('" + TextBoxN.Text + "','" + TextBoxP.Text + "')", con);
        try
        {
            int re = cmd.ExecuteNonQuery();
            con.Close();
            if (re > 0)
            {
                Response.Write("<script language='javascript'>alert('欢迎加入！');self.location='MainWeb.aspx'</script>");
                return;
            }
        }
        catch
        {
        }
        Response.Write("<script language='javascript'>alert('用户名已存在');</script>");
    }
}