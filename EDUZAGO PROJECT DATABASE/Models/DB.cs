namespace EDUZAGO_PROJECT_DATABASE.Models;

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
        string query = "SELECT COUNT(*) FROM Student";
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
        string query = "SELECT COUNT(*) FROM Instructor";
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
    public DataTable GetAllInstructors()
    {
        DataTable dt = new DataTable();
        string query = "Select * From Instructor,[USER] WHERE Instructor_ID=USER_ID";
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
    public DataTable GetAllStudents()
    {
        DataTable dt = new DataTable();
        string query = "Select * From Student,[User] WHERE Student_ID=USER_ID";
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
                 WHERE Title LIKE @search";

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

    public DataTable GetInstructorCourses(int instructor_id)
    {
        DataTable dt = new DataTable();

        string q = "select  c.Course_Code,c.Title from course c , iNSTRUCTOR i where c.Instructor_ID = i.@InstructorID";
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
        cmd.Parameters.AddWithValue("@ccode", newcourse.CourseCode);
        cmd.Parameters.AddWithValue("@title", newcourse.CourseCode);
        cmd.Parameters.AddWithValue("@fee", newcourse.CourseCode);
        cmd.Parameters.AddWithValue("@des", newcourse.CourseCode);
        cmd.Parameters.AddWithValue("@duration", newcourse.CourseCode);
        cmd.Parameters.AddWithValue("@categoryid", newcourse.CourseCode);
        cmd.Parameters.AddWithValue("@adminid", newcourse.CourseCode);
        cmd.Parameters.AddWithValue("@instructorid", newcourse.CourseCode);

        try
        {
            con.Open();
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

    public void AddCategory(Category category)
    {
        string query = "INSERT INTO CATEGORY (Category_ID, Category_Name, Description, Admin_ID) VALUES (@CategoryID, @CategoryName, @Description, @Admin_ID)";
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

    public DataTable GetResources(string courseCode)
    {
        DataTable dt = new DataTable();
        string query = "SELECT * FROM Resource WHERE Course_Code = @cc";
        SqlCommand cmd = new SqlCommand(query, con);
        cmd.Parameters.AddWithValue("@cc", courseCode);
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

    public void AddResource(Resource resource)
    {
        string query = "INSERT INTO Resource (ResourceType, URL, Course_Code, Instructor_ID) VALUES (@type, @url, @cc, @iid)";
        SqlCommand cmd = new SqlCommand(query, con);
        cmd.Parameters.AddWithValue("@type", resource.ResourceType);
        cmd.Parameters.AddWithValue("@url", resource.URL);
        cmd.Parameters.AddWithValue("@cc", resource.Course_Code);
        cmd.Parameters.AddWithValue("@iid", resource.Instructor_ID);
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

    public DataTable GetSchedule(string courseCode)
    {
        DataTable dt = new DataTable();
        string query = "SELECT * FROM Schedule WHERE Course_Code = @cc";
        SqlCommand cmd = new SqlCommand(query, con);
        cmd.Parameters.AddWithValue("@cc", courseCode);
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

    public void AddSchedule(Schedule schedule)
    {
        string query = "INSERT INTO Schedule (SessionTime, SessionDetails, Course_Code, Instructor_ID) VALUES (@time, @details, @cc, @iid)";
        SqlCommand cmd = new SqlCommand(query, con);
        cmd.Parameters.AddWithValue("@time", schedule.SessionTime);
        cmd.Parameters.AddWithValue("@details", schedule.SessionDetails);
        cmd.Parameters.AddWithValue("@cc", schedule.Course_Code);
        cmd.Parameters.AddWithValue("@iid", schedule.Instructor_ID);
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

    public void UpdateGrade(int initialGradeID, string newGrade)
    {
        string query = "UPDATE Grade SET Progress = @grade WHERE GradeID = @gid";
        SqlCommand cmd = new SqlCommand(query, con);
        cmd.Parameters.AddWithValue("@grade", newGrade);
        cmd.Parameters.AddWithValue("@gid", initialGradeID);
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

    public void AddReview(Review review)
    {
        string query = "INSERT INTO Review (Content, Rating, Course_Code, Student_ID) VALUES (@content, @rating, @cc, @sid)";
        SqlCommand cmd = new SqlCommand(query, con);
        cmd.Parameters.AddWithValue("@content", review.Content);
        cmd.Parameters.AddWithValue("@rating", review.Rating);
        cmd.Parameters.AddWithValue("@cc", review.Course_Code);
        cmd.Parameters.AddWithValue("@sid", review.Student_ID);
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

    public Course GetCourse(string courseCode)
    {
        Course c = new Course();
        string query = "SELECT * FROM Course WHERE Course_Code = @cc";
        SqlCommand cmd = new SqlCommand(query, con);
        cmd.Parameters.AddWithValue("@cc", courseCode);
        try
        {
            con.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                c.CourseCode = reader["Course_Code"].ToString();
                c.Title = reader["Title"].ToString();
                c.Description = reader["Description"].ToString();
                // We'd ideally fetch Instructor name too via join, keeping it simple for now or adding another query if strictly needed.
                // Assuming Instructor_ID is in Course table based on DB.AddCourse structure (though it had typos in AddCourse args).
                // Let's stick to reading available columns.
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        finally
        {
            con.Close();
        }
        return c;
    }

    public DataTable GetStudentsWithGrades(string courseCode)
    {
        DataTable dt = new DataTable();
        // Join Student, Grade, and User (for email/name)
        // Assuming Grade table has Student_ID and Course_Code
        // And Student_ID matches User_ID for Name/Email as per GetAllStudents
        string query = @"
            SELECT S.Student_ID, U.Name, U.Email, G.CompletionStatus, G.Progress, G.GradeID
            FROM Student S
            JOIN [User] U ON S.Student_ID = U.User_ID
            LEFT JOIN Grade G ON S.Student_ID = G.Student_ID AND G.Course_Code = @cc
            WHERE G.Course_Code = @cc OR G.Course_Code IS NULL"; 
            // This query might return all students if we don't filter properly. 
            // Better: Get grades for this course, join with student info.
        
        string betterQuery = @"
            SELECT G.GradeID, S.Student_ID, U.Name, U.Email, G.CompletionStatus, G.Progress
            FROM Grade G
            JOIN Student S ON G.Student_ID = S.Student_ID
            JOIN [User] U ON S.Student_ID = U.User_ID
            WHERE G.Course_Code = @cc";

        SqlCommand cmd = new SqlCommand(betterQuery, con);
        cmd.Parameters.AddWithValue("@cc", courseCode);
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