namespace EDUZAGO_PROJECT_DATABASE.Models
{
    using Microsoft.Data.SqlClient;
    using System.Data;

    public class DB
    {
        private string connectionstring = "Data Source=DESKTOP-SQFUII7;Initial Catalog=EDUZAGO_DB;Integrated Security=True;Trust Server Certificate=True";
        public SqlConnection con { get; set; }

        public DB()
        {
            con = new SqlConnection(connectionstring);
        }
        public int Get_StudentCount()
        {
            int count = 0;
            string query = "SELECT COUNT(*) FROM Students";
            SqlCommand cmd = new SqlCommand(query, con);
            try
            {
                con.Open();
                count = (int)cmd.ExecuteScalar();
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
            }
            finally
            {
                con.Close();
            }

            return count;

        }
        public int Get_InstructorCount()
        {
            int count = 0;
            string query = "SELECT COUNT(*) FROM Instructors";
            SqlCommand cmd = new SqlCommand(query, con);
            try
            {
                con.Open();
                count = (int)cmd.ExecuteScalar();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally 
            {
                con.Close();
            }
            return count;
        }
        public int Get_CourseCount()
        {
            int count = 0;
            string query = "Select Count(*) From Course";
            SqlCommand cmd = new SqlCommand(query, con);
            try
            {
                con.Open();
                count = (int)cmd.ExecuteScalar();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                con.Close();
            }
            return count;
        }
        public DataTable GetAllCategories()
        {
            DataTable dt = new DataTable();
            string query = "Select * From Category";
            SqlCommand cmd = new SqlCommand(query, con);
            try
            {
                con.Open();
                dt.Load(cmd.ExecuteReader());
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally 
            {
                con.Close();
            }
            return dt;
        }
        public DataTable GetAllCourses()
        {
            DataTable dt = new DataTable();
            string query = "SELECT * FROM Course";
            SqlCommand cmd = new SqlCommand(query, con);
            try
            {
                con.Open();
                dt.Load(cmd.ExecuteReader());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                con.Close();
            }
            return dt;
        }
        public DataTable SearchCourses(string searchText)
        {
            DataTable dt = new DataTable();

            string query = @"SELECT * FROM COURSE
                     WHERE Title LIKE @search" ;

            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@search", "%" + searchText + "%");
            try
            {
                con.Open();
                dt.Load(cmd.ExecuteReader());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                con.Close();
            }
            return dt;
        }
        public void AddCategory(Category category)
        {
            string query = "Insert Into Category ( CategoryID, CategoryName, Description, Admin_ID) Values (@CategoryID, @CategoryName, @ Description, @Admin_ID)";
            SqlCommand cmd = new SqlCommand(query, con);

            cmd.Parameters.AddWithValue("@CategoryID", category.CategoryID);
            cmd.Parameters.AddWithValue("@CategoryName", category.CategoryName);
            cmd.Parameters.AddWithValue("@Description", category.Description);
            cmd.Parameters.AddWithValue("@Admin_ID", category.Admin_ID);
            try
            {
                con.Open();
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
            }
            finally
            {
                con.Close();
            }


        }

    }
}
