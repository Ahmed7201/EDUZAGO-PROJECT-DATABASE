namespace EDUZAGO_PROJECT_DATABASE.Models;
using Microsoft.Data.SqlClient;
{
    public class DB
    {
        private string connectionstring = "Data Source=DESKTOP-SQFUII7;Initial Catalog=EDUZAGO_DB;Integrated Security=True;Trust Server Certificate=True";
    public SqlConnection con { get; set; };

    }
}
