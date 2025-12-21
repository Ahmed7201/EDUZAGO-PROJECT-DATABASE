namespace EDUZAGO_PROJECT_DATABASE.Models;
using Microsoft.Data.SqlClient;


{
using System.Data;

public class DB
    {
        private string connectionstring = "Data Source=DESKTOP-SQFUII7;Initial Catalog=EDUZAGO_DB;Integrated Security=True;Trust Server Certificate=True";
    public SqlConnection con { get; set; }

    public DB()
    {
        con = new SqlConnection(connectionstring);
    }


        public DataTable GetInstructorCourses(int instructor_id) {

        DataTable dt = new DataTable();



        string q = "select  c.Course_Code,c.Title from course c , iNSTRUCTOR i where c.Instructor_ID = i.@InstructorID";
;
        SqlCommand cmd = new SqlCommand(q, con);

        try
        {
            con.Open();
            cmd.Parameters.AddWithValue("@InstructorID", instructor_id);
            dt.Load(cmd.ExecuteReader());

            

        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            
            throw;
        }
        finally
        {
            con.Close();
        }
        return dt;
        }


    public void Addcourse(Course newcourse)
    {
        string q = "Insert into (Course_Code , Title, Fees, Description, Duration, Category_ID, Admin_ID, Instructor_ID) Values (@ccode,@title,@fee,@des,@duration,@categoryid,@adminid,@instructorid)";
        SqlCommand cmd = new SqlCommand(q, con);

        try
        {
            con.Open();

            cmd.Parameters.AddWithValue("@ccode", newcourse.CourseCode);
            cmd.Parameters.AddWithValue("@title", newcourse.CourseCode);
            cmd.Parameters.AddWithValue("@fee", newcourse.CourseCode);
            cmd.Parameters.AddWithValue("@des", newcourse.CourseCode);
            cmd.Parameters.AddWithValue("@duration", newcourse.CourseCode);
            cmd.Parameters.AddWithValue("@categoryid", newcourse.CourseCode);
            cmd.Parameters.AddWithValue("@adminid", newcourse.CourseCode);
            cmd.Parameters.AddWithValue("@instructorid", newcourse.CourseCode);

            cmd.ExecuteNonQuery();

        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw;
        }
        finally
        {
            con.Close();
        }

    }






}
