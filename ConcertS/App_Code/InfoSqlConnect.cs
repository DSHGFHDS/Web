
public class InfoSqlConnect
{
    public static System.Data.SqlClient.SqlConnection createConnection()
    {
        return new System.Data.SqlClient.SqlConnection("server=(local);database=concert_info;uid=sa;pwd=sa");
    }
}