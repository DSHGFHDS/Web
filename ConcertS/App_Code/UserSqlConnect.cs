
public class UserSqlConnect
{
    public static System.Data.SqlClient.SqlConnection createConnection()
    {
        return new System.Data.SqlClient.SqlConnection("server=(local);database=user_info;uid=sa;pwd=sa");
    }
}