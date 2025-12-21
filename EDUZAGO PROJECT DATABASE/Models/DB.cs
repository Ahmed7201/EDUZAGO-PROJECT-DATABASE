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
                Console.Write(ex.Message);
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
        public DataTable GetCoursesByCategory(int Category_ID)
        {
            DataTable dt = new DataTable();

            string query = "SELECT * FROM COURSE AND CATEGORY WHERE COURSE.Category_ID = @Cat_ID";
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@Cat_ID", Category_ID);
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
        public void EnrollStudent(int StudentId, string CourseCode)
        {
            string query = "INSERT INTO Enrollment(Student_ID,Course_Code,Enrollment_Date) VALUES (@sid, @cc, @edate)";
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@sid", StudentId);
            cmd.Parameters.AddWithValue("@cc", CourseCode);
            cmd.Parameters.AddWithValue("@edate", DateTime.Now);
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

        public DataTable GetStudentEnrollments(int StudentId)
        {
            DataTable dt = new DataTable();

            string query = @"SELECT C.*, E.Enrollment_Date
                     FROM COURSE C
                     WHERE C.Course_Code = E.Course_Code AND E.Student_ID = @sid";
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@sid", StudentId);
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

    }
}
