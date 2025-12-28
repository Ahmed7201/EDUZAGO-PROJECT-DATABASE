using Microsoft.Data.SqlClient;

namespace EDUZAGO_PROJECT_DATABASE.Models;

using System.Data;
//done ghoneim
public class DB
{
    private string connectionstring = "Data Source=DESKTOP-SQFUII7;Initial Catalog=DB_EDUZAGO;Integrated Security=True;Trust Server Certificate=True;MultipleActiveResultSets=True";
    public SqlConnection con { get; set; }

    public class WeeklyRevenue
    {
        public string Week { get; set; }
        public decimal Revenue { get; set; }
    }

    public class CoursePopularity
    {
        public string CourseTitle { get; set; }
        public int StudentCount { get; set; }
    }

    public class DashboardCounts
    {
        public int ActiveStudents { get; set; }
        public int IdleStudents { get; set; }
        public int ActiveInstructors { get; set; }
        public int IdleInstructors { get; set; }
        public int ActiveCourses { get; set; }
        public int IdleCourses { get; set; }
    }

    public class CategoryIncome
    {
        public string CategoryName { get; set; }
        public decimal TotalIncome { get; set; }
    }

    public class CourseIncome
    {
        public string CourseTitle { get; set; }
        public decimal TotalIncome { get; set; }
        public string CategoryName { get; set; }
    }

    public class CourseEnrollmentStats
    {
        public string CourseTitle { get; set; }
        public int StudentCount { get; set; }
        public string CategoryName { get; set; }
    }

    public DB()
    {
        con = new SqlConnection(connectionstring);
    }

    public List<WeeklyRevenue> GetWeeklyRevenue()
    {
        List<WeeklyRevenue> data = new List<WeeklyRevenue>();
        try
        {
            if (con.State != ConnectionState.Open) con.Open();
            // Group by Year-Week. SQL Server: DATEPART(iso_week, Enrollment_Date)
            string query = @"
                SELECT TOP 10 
                    CONCAT(YEAR(Enrollment_Date), '-W', DATEPART(iso_week, Enrollment_Date)) as WeekLabel, 
                    SUM(C.Fees) as Revenue
                FROM Enrollment E
                JOIN Course C ON E.Course_Code = C.Course_Code
                GROUP BY YEAR(Enrollment_Date), DATEPART(iso_week, Enrollment_Date)
                ORDER BY YEAR(Enrollment_Date) DESC, DATEPART(iso_week, Enrollment_Date) DESC";
            SqlCommand cmd = new SqlCommand(query, con);
            SqlDataReader r = cmd.ExecuteReader();
            while (r.Read())
            {
                data.Add(new WeeklyRevenue
                {
                    Week = r["WeekLabel"].ToString(),
                    Revenue = r["Revenue"] != DBNull.Value ? Convert.ToDecimal(r["Revenue"]) : 0
                });
            }
            r.Close();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        finally
        {
            con.Close();
        }
        data.Reverse();
        return data;
    }

    public List<CategoryIncome> GetCategoryIncome()
    {
        List<CategoryIncome> data = new List<CategoryIncome>();
        try
        {
            if (con.State != ConnectionState.Open) con.Open();
            string query = @"
                SELECT Cat.Category_Name, SUM(C.Fees) as TotalIncome
                FROM Enrollment E
                JOIN Course C ON E.Course_Code = C.Course_Code
                JOIN Category Cat ON C.Category_ID = Cat.Category_ID
                GROUP BY Cat.Category_Name";
            SqlCommand cmd = new SqlCommand(query, con);
            SqlDataReader r = cmd.ExecuteReader();
            while (r.Read())
            {
                data.Add(new CategoryIncome
                {
                    CategoryName = r["Category_Name"].ToString(),
                    TotalIncome = r["TotalIncome"] != DBNull.Value ? Convert.ToDecimal(r["TotalIncome"]) : 0
                });
            }
        }
        catch (Exception ex) { Console.WriteLine(ex.Message); }
        finally { con.Close(); }
        return data;
    }

    public List<CourseIncome> GetCourseIncome()
    {
        List<CourseIncome> data = new List<CourseIncome>();
        try
        {
            if (con.State != ConnectionState.Open) con.Open();
            string query = @"
                SELECT TOP 10 C.Title, Cat.Category_Name, SUM(C.Fees) as TotalIncome
                FROM Enrollment E
                JOIN Course C ON E.Course_Code = C.Course_Code
                JOIN Category Cat ON C.Category_ID = Cat.Category_ID
                GROUP BY C.Title, Cat.Category_Name
                ORDER BY TotalIncome DESC";
            SqlCommand cmd = new SqlCommand(query, con);
            SqlDataReader r = cmd.ExecuteReader();
            while (r.Read())
            {
                data.Add(new CourseIncome
                {
                    CourseTitle = r["Title"].ToString(),
                    CategoryName = r["Category_Name"].ToString(),
                    TotalIncome = r["TotalIncome"] != DBNull.Value ? Convert.ToDecimal(r["TotalIncome"]) : 0
                });
            }
        }
        catch (Exception ex) { Console.WriteLine(ex.Message); }
        finally { con.Close(); }
        return data;
    }

    public List<CourseEnrollmentStats> GetCourseEnrollmentStats()
    {
        List<CourseEnrollmentStats> data = new List<CourseEnrollmentStats>();
        try
        {
            if (con.State != ConnectionState.Open) con.Open();
            string query = @"
                SELECT TOP 10 C.Title, Cat.Category_Name, COUNT(E.Student_ID) as StudentCount
                FROM Enrollment E
                JOIN Course C ON E.Course_Code = C.Course_Code
                JOIN Category Cat ON C.Category_ID = Cat.Category_ID
                GROUP BY C.Title, Cat.Category_Name
                ORDER BY StudentCount DESC";
            SqlCommand cmd = new SqlCommand(query, con);
            SqlDataReader r = cmd.ExecuteReader();
            while (r.Read())
            {
                data.Add(new CourseEnrollmentStats
                {
                    CourseTitle = r["Title"].ToString(),
                    CategoryName = r["Category_Name"].ToString(),
                    StudentCount = r["StudentCount"] != DBNull.Value ? Convert.ToInt32(r["StudentCount"]) : 0
                });
            }
        }
        catch (Exception ex) { Console.WriteLine(ex.Message); }
        finally { con.Close(); }
        return data;
    }


    public List<CoursePopularity> GetCoursePopularity()
    {
        List<CoursePopularity> data = new List<CoursePopularity>();
        try
        {
            if (con.State != ConnectionState.Open) con.Open();
            string query = @"
                SELECT TOP 5 C.Title, COUNT(E.Student_ID) as Count
                FROM Course C
                LEFT JOIN Enrollment E ON C.Course_Code = E.Course_Code
                GROUP BY C.Title
                ORDER BY Count DESC";
            SqlCommand cmd = new SqlCommand(query, con);
            SqlDataReader r = cmd.ExecuteReader();
            while (r.Read())
            {
                data.Add(new CoursePopularity
                {
                    CourseTitle = r["Title"].ToString(),
                    StudentCount = Convert.ToInt32(r["Count"])
                });
            }
            r.Close();
        }
        catch (Exception ex) { Console.WriteLine(ex.Message); }
        finally { con.Close(); }
        return data;
    }
    public DashboardCounts GetDashboardCounts()
    {
        DashboardCounts counts = new DashboardCounts();
        try
        {
            if (con.State != ConnectionState.Open) con.Open();

            // Active Students (Enrolled in at least one course) vs Idle (Registered but zero enrollments)
            string qStudents = @"
                SELECT 
                    (SELECT COUNT(DISTINCT Student_ID) FROM Enrollment) as Active,
                    (SELECT COUNT(*) FROM Student WHERE Student_ID NOT IN (SELECT DISTINCT Student_ID FROM Enrollment WHERE Student_ID IS NOT NULL)) as Idle";

            // Active Instructors (Teaches > 0 courses) vs Idle
            string qInstructors = @"
                SELECT 
                    (SELECT COUNT(DISTINCT Instructor_ID) FROM Course) as Active,
                    (SELECT COUNT(*) FROM Instructor WHERE Instructor_ID NOT IN (SELECT DISTINCT Instructor_ID FROM Course WHERE Instructor_ID IS NOT NULL)) as Idle";

            // Active Courses (Has > 0 students) vs Idle
            string qCourses = @"
                SELECT 
                    (SELECT COUNT(DISTINCT Course_Code) FROM Enrollment) as Active,
                    (SELECT COUNT(*) FROM Course WHERE Course_Code NOT IN (SELECT DISTINCT Course_Code FROM Enrollment WHERE Course_Code IS NOT NULL)) as Idle";

            SqlCommand cmd1 = new SqlCommand(qStudents, con);
            SqlDataReader r1 = cmd1.ExecuteReader();
            if (r1.Read()) { counts.ActiveStudents = (int)r1[0]; counts.IdleStudents = (int)r1[1]; }
            r1.Close();

            SqlCommand cmd2 = new SqlCommand(qInstructors, con);
            SqlDataReader r2 = cmd2.ExecuteReader();
            if (r2.Read()) { counts.ActiveInstructors = (int)r2[0]; counts.IdleInstructors = (int)r2[1]; }
            r2.Close();

            SqlCommand cmd3 = new SqlCommand(qCourses, con);
            SqlDataReader r3 = cmd3.ExecuteReader();
            if (r3.Read()) { counts.ActiveCourses = (int)r3[0]; counts.IdleCourses = (int)r3[1]; }
            r3.Close();
        }
        catch (Exception ex) { Console.WriteLine(ex.Message); }
        finally { con.Close(); }
        return counts;
    }

    public DataTable SearchInstructors(string term, string statusFilter)
    {
        DataTable dt = new DataTable();
        string query = "Select * From Instructor,[USER] WHERE Instructor_ID=USER_ID";

        if (!string.IsNullOrEmpty(statusFilter) && statusFilter != "All")
        {
            query += " AND Approval_Status=@status";
        }

        if (!string.IsNullOrEmpty(term))
        {
            query += " AND (Name LIKE @term OR Email LIKE @term)";
        }

        SqlCommand cmd = new SqlCommand(query, con);

        if (!string.IsNullOrEmpty(statusFilter) && statusFilter != "All")
        {
            cmd.Parameters.AddWithValue("@status", statusFilter);
        }

        if (!string.IsNullOrEmpty(term))
        {
            cmd.Parameters.AddWithValue("@term", "%" + term + "%");
        }

        try
        {
            if (con.State != ConnectionState.Open) con.Open();
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

    public User GetAdminProfile(int userId)
    {
        User u = new User();
        try
        {
            if (con.State != ConnectionState.Open) con.Open();
            string query = "SELECT * FROM [User] WHERE USER_ID = @uid";
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@uid", userId);
            SqlDataReader r = cmd.ExecuteReader();
            if (r.Read())
            {
                u.USER_ID = (int)r["User_ID"];
                u.Name = r["Name"].ToString();
                u.Email = r["Email"].ToString();
                u.Role = "Admin";
            }
            r.Close();
        }
        catch (Exception ex) { Console.WriteLine(ex.Message); }
        finally { con.Close(); }
        return u;
    }

    public void UpdateAdminProfile(User u)
    {
        try
        {
            if (con.State != ConnectionState.Open) con.Open();
            string query = "UPDATE [User] SET Name = @name WHERE USER_ID = @uid";
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@name", u.Name);
            cmd.Parameters.AddWithValue("@uid", u.USER_ID);
            cmd.ExecuteNonQuery();
        }
        catch (Exception ex) { Console.WriteLine(ex.Message); }
        finally { con.Close(); }
    }
    public bool IsEnrolled(int studentId, string courseCode)
    {
        bool enrolled = false;
        string query = "SELECT COUNT(*) FROM Enrollment WHERE Student_ID = @sid AND Course_Code = @cc";
        SqlCommand cmd = new SqlCommand(query, con);
        cmd.Parameters.AddWithValue("@sid", studentId);
        cmd.Parameters.AddWithValue("@cc", courseCode);
        try
        {
            if (con.State != ConnectionState.Open) con.Open();
            int count = (int)cmd.ExecuteScalar();
            if (count > 0) enrolled = true;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        finally
        {
            con.Close();
        }
        return enrolled;
    }

    public void MarkResourceCompleted(int studentId, int resourceId)
    {
        try
        {
            if (con.State != ConnectionState.Open) con.Open();
            // Check if already completed
            string check = "SELECT COUNT(*) FROM ResourceCompletion WHERE Student_ID=@sid AND Resource_ID=@rid";
            SqlCommand cmdCheck = new SqlCommand(check, con);
            cmdCheck.Parameters.AddWithValue("@sid", studentId);
            cmdCheck.Parameters.AddWithValue("@rid", resourceId);
            if ((int)cmdCheck.ExecuteScalar() > 0) return;

            string query = "INSERT INTO ResourceCompletion (Student_ID, Resource_ID, DateCompleted) VALUES (@sid, @rid, @date)";
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@sid", studentId);
            cmd.Parameters.AddWithValue("@rid", resourceId);
            cmd.Parameters.AddWithValue("@date", DateTime.Now);
            cmd.ExecuteNonQuery();
        }
        catch (Exception ex) { Console.WriteLine(ex.Message); }
        finally { con.Close(); }
    }

    public List<int> GetCompletedResources(int studentId, string courseCode)
    {
        List<int> ids = new List<int>();
        try
        {
            if (con.State != ConnectionState.Open) con.Open();
            string query = @"
                SELECT RC.Resource_ID 
                FROM ResourceCompletion RC
                JOIN Resource R ON RC.Resource_ID = R.Resource_ID
                WHERE RC.Student_ID = @sid AND R.Course_Code = @cc";
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@sid", studentId);
            cmd.Parameters.AddWithValue("@cc", courseCode);
            SqlDataReader r = cmd.ExecuteReader();
            while (r.Read())
            {
                ids.Add(Convert.ToInt32(r["Resource_ID"]));
            }
            r.Close();
        }
        catch (Exception ex) { Console.WriteLine(ex.Message); }
        finally { con.Close(); }
        return ids;
    }

    // Centralized DB connection handler
    public SqlConnection GetConnection()
    {
        if (con.State != ConnectionState.Open)
            con.Open();

        return con;
    }

    // Validate user login credentials
    // Authenticate user and return User object with details, or null if failed
    public User Login(string email, string password)
    {
        string query = "SELECT User_ID, Name, Email FROM [User] WHERE Email = @email AND Password = @password";
        SqlCommand cmd = new SqlCommand(query, con);
        cmd.Parameters.AddWithValue("@email", email);
        cmd.Parameters.AddWithValue("@password", password);

        try
        {
            if (con.State != ConnectionState.Open) con.Open();
            SqlDataReader reader = cmd.ExecuteReader();

            User user = null;
            if (reader.Read())
            {
                user = new User();
                user.USER_ID = Convert.ToInt32(reader["User_ID"]);
                user.Name = reader["Name"] != DBNull.Value ? reader["Name"].ToString() : "";
                user.Email = reader["Email"] != DBNull.Value ? reader["Email"].ToString() : "";
            }
            reader.Close(); // Close reader before running other commands

            if (user != null)
            {
                // Determine Role
                int userId = user.USER_ID;

                // Check Admin
                SqlCommand roleCmd = new SqlCommand("SELECT COUNT(*) FROM Admin WHERE Admin_ID = @uid", con);
                roleCmd.Parameters.AddWithValue("@uid", userId);
                if ((int)roleCmd.ExecuteScalar() > 0)
                {
                    user.Role = "Admin";
                    return user;
                }

                // Check Instructor
                roleCmd = new SqlCommand("SELECT Approval_Status FROM Instructor WHERE Instructor_ID = @uid", con);
                roleCmd.Parameters.AddWithValue("@uid", userId);
                var statusObj = roleCmd.ExecuteScalar();
                if (statusObj != null)
                {
                    user.Role = "Instructor";
                    user.ApprovalStatus = statusObj.ToString();
                    return user;
                }

                // Check Student
                roleCmd = new SqlCommand("SELECT COUNT(*) FROM Student WHERE Student_ID = @uid", con);
                roleCmd.Parameters.AddWithValue("@uid", userId);
                if ((int)roleCmd.ExecuteScalar() > 0)
                {
                    user.Role = "Student";
                    return user;
                }

                // Fallback default
                user.Role = "Student";
                return user;
            }

            return null;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return null;
        }
        finally
        {
            con.Close();
        }
    }

    // Register a new user with Role handling
    public bool RegisterUser(string name, string email, string password, string role)
    {
        try
        {
            if (con.State != ConnectionState.Open) con.Open();

            // 1. Get Next ID (Since USER_ID is not Identity)
            SqlCommand cmdId = new SqlCommand("SELECT ISNULL(MAX(USER_ID), 0) + 1 FROM [User]", con);
            int newUserId = (int)cmdId.ExecuteScalar();

            // 2. Insert into [User]
            string queryUser = "INSERT INTO [User] (USER_ID, Name, Email, Password) VALUES (@uid, @name, @email, @password)";
            SqlCommand cmd = new SqlCommand(queryUser, con);
            cmd.Parameters.AddWithValue("@uid", newUserId);
            cmd.Parameters.AddWithValue("@name", name);
            cmd.Parameters.AddWithValue("@email", email);
            cmd.Parameters.AddWithValue("@password", password);
            cmd.ExecuteNonQuery();

            // 3. Insert into Role Table
            if (role == "Student")
            {
                // Insert into STUDENT with empty defaults
                string queryStudent = "INSERT INTO Student (Student_ID, Address, Phone_Number) VALUES (@uid, 'Unknown', 'Unknown')";
                SqlCommand cmdStudent = new SqlCommand(queryStudent, con);
                cmdStudent.Parameters.AddWithValue("@uid", newUserId);
                cmdStudent.ExecuteNonQuery();
            }
            else if (role == "Instructor")
            {
                // Insert into INSTRUCTOR with Pending status and Default Admin ID (4)
                string queryInstructor = "INSERT INTO Instructor (Instructor_ID, Bio, Expertise, Approval_Status, Admin_ID) VALUES (@uid, 'Bio Pending', 'Expertise Pending', 'Pending', 4)";
                SqlCommand cmdInstructor = new SqlCommand(queryInstructor, con);
                cmdInstructor.Parameters.AddWithValue("@uid", newUserId);
                cmdInstructor.ExecuteNonQuery();
            }

            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return false;
        }
        finally
        {
            con.Close();
        }
    }

    public int Get_Pendingcount()
    {
        int count = 0;
        string query = "Select Count(*) From Instructor where Approval_Status Like'Pending'";
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
            if (con.State != ConnectionState.Open) con.Open();
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

    // Profile Methods
    public Student GetStudentProfile(int userId)
    {
        Student s = new Student();
        string query = "SELECT U.Name, U.Email, S.Phone_Number, S.Address FROM Student S JOIN [User] U ON S.Student_ID = U.USER_ID WHERE S.Student_ID = @uid";
        SqlCommand cmd = new SqlCommand(query, con);
        cmd.Parameters.AddWithValue("@uid", userId);
        try
        {
            if (con.State != ConnectionState.Open) con.Open();
            SqlDataReader r = cmd.ExecuteReader();
            if (r.Read())
            {
                s.USER_ID = userId;
                s.Name = r["Name"].ToString();
                s.Email = r["Email"].ToString();
                s.PhoneNumber = r["Phone_Number"] != DBNull.Value ? r["Phone_Number"].ToString() : "";
                s.Address = r["Address"] != DBNull.Value ? r["Address"].ToString() : "";
            }
            r.Close();
        }
        catch (Exception ex) { Console.WriteLine(ex.Message); }
        finally { con.Close(); }
        return s;
    }

    public void UpdateStudentProfile(Student s)
    {
        // Update User (Name) and Student (Phone, Address)
        try
        {
            if (con.State != ConnectionState.Open) con.Open();
            // Update User Table
            string q1 = "UPDATE [User] SET Name = @name WHERE USER_ID = @uid";
            SqlCommand cmd1 = new SqlCommand(q1, con);
            cmd1.Parameters.AddWithValue("@name", s.Name);
            cmd1.Parameters.AddWithValue("@uid", s.USER_ID);
            cmd1.ExecuteNonQuery();

            // Update Student Table
            string q2 = "UPDATE Student SET Phone_Number = @phone, Address = @addr WHERE Student_ID = @uid";
            SqlCommand cmd2 = new SqlCommand(q2, con);
            cmd2.Parameters.AddWithValue("@phone", s.PhoneNumber);
            cmd2.Parameters.AddWithValue("@addr", s.Address);
            cmd2.Parameters.AddWithValue("@uid", s.USER_ID);
            cmd2.ExecuteNonQuery();
        }
        catch (Exception ex) { Console.WriteLine(ex.Message); }
        finally { con.Close(); }
    }

    public Instructor GetInstructorProfile(int userId)
    {
        Instructor i = new Instructor();
        string query = "SELECT U.Name, U.Email, I.Bio, I.Expertise FROM Instructor I JOIN [User] U ON I.Instructor_ID = U.USER_ID WHERE I.Instructor_ID = @uid";
        SqlCommand cmd = new SqlCommand(query, con);
        cmd.Parameters.AddWithValue("@uid", userId);
        try
        {
            if (con.State != ConnectionState.Open) con.Open();
            SqlDataReader r = cmd.ExecuteReader();
            if (r.Read())
            {
                i.USER_ID = userId;
                i.Name = r["Name"].ToString();
                i.Email = r["Email"].ToString();
                i.Bio = r["Bio"] != DBNull.Value ? r["Bio"].ToString() : "";
                i.Expertise = r["Expertise"] != DBNull.Value ? r["Expertise"].ToString() : "";
            }
            r.Close();
        }
        catch (Exception ex) { Console.WriteLine(ex.Message); }
        finally { con.Close(); }
        return i;
    }

    public void UpdateInstructorProfile(Instructor i)
    {
        try
        {
            if (con.State != ConnectionState.Open) con.Open();
            // Update User Table
            string q1 = "UPDATE [User] SET Name = @name WHERE USER_ID = @uid";
            SqlCommand cmd1 = new SqlCommand(q1, con);
            cmd1.Parameters.AddWithValue("@name", i.Name);
            cmd1.Parameters.AddWithValue("@uid", i.USER_ID);
            cmd1.ExecuteNonQuery();

            // Update Instructor Table
            string q2 = "UPDATE Instructor SET Bio = @bio, Expertise = @expert WHERE Instructor_ID = @uid";
            SqlCommand cmd2 = new SqlCommand(q2, con);
            cmd2.Parameters.AddWithValue("@bio", i.Bio);
            cmd2.Parameters.AddWithValue("@expert", i.Expertise);
            cmd2.Parameters.AddWithValue("@uid", i.USER_ID);
            cmd2.ExecuteNonQuery();
        }
        catch (Exception ex) { Console.WriteLine(ex.Message); }
        finally { con.Close(); }
    }

    public bool UpdatePassword(int userId, string oldPassword, string newPassword)
    {
        bool success = false;
        try
        {
            if (con.State != ConnectionState.Open) con.Open();

            // Verify old password
            string checkQuery = "SELECT COUNT(*) FROM [User] WHERE USER_ID = @uid AND Password = @old";
            SqlCommand checkCmd = new SqlCommand(checkQuery, con);
            checkCmd.Parameters.AddWithValue("@uid", userId);
            checkCmd.Parameters.AddWithValue("@old", oldPassword);

            int count = (int)checkCmd.ExecuteScalar();

            if (count > 0)
            {
                // Update to new password
                string updateQuery = "UPDATE [User] SET Password = @new WHERE USER_ID = @uid";
                SqlCommand updateCmd = new SqlCommand(updateQuery, con);
                updateCmd.Parameters.AddWithValue("@new", newPassword);
                updateCmd.Parameters.AddWithValue("@uid", userId);
                updateCmd.ExecuteNonQuery();
                success = true;
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
        return success;
    }

    public DataTable GetAllCategories()
    {
        DataTable dt = new DataTable();
        string query = "Select * From Category";
        SqlCommand cmd = new SqlCommand(query, con);
        try
        {
            if (con.State != ConnectionState.Open) con.Open();
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

    public DataTable SearchCategories(string term)
    {
        DataTable dt = new DataTable();
        string query = "SELECT * FROM Category WHERE Category_Name LIKE @term OR Description LIKE @term";
        SqlCommand cmd = new SqlCommand(query, con);
        cmd.Parameters.AddWithValue("@term", "%" + term + "%");
        try
        {
            if (con.State != ConnectionState.Open) con.Open();
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
        string query = @"SELECT C.Course_Code, C.Title, C.Category_ID, C.Fees, C.Description, C.Duration, U.Name AS InstructorName 
                         FROM Course C 
                         LEFT JOIN Instructor I ON C.Instructor_ID = I.Instructor_ID 
                         LEFT JOIN [User] U ON I.Instructor_ID = U.USER_ID";
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

        string query = @"SELECT C.*, U.Name AS InstructorName 
                 FROM COURSE C 
                 LEFT JOIN Instructor I ON C.Instructor_ID = I.Instructor_ID 
                 LEFT JOIN [User] U ON I.Instructor_ID = U.USER_ID 
                 WHERE C.Title LIKE @search";

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

    public DataTable SearchCoursesAdmin(string term, string categoryFilter)
    {
        DataTable dt = new DataTable();
        string query = @"SELECT C.Course_Code, C.Title, C.Category_ID, C.Fees, C.Description, C.Duration, U.Name AS InstructorName 
                         FROM Course C 
                         LEFT JOIN Instructor I ON C.Instructor_ID = I.Instructor_ID 
                         LEFT JOIN [User] U ON I.Instructor_ID = U.USER_ID 
                         WHERE 1=1";

        if (!string.IsNullOrEmpty(term))
        {
            query += " AND C.Title LIKE @term";
        }
        if (!string.IsNullOrEmpty(categoryFilter))
        {
            query += " AND C.Category_ID = @cat";
        }

        SqlCommand cmd = new SqlCommand(query, con);

        if (!string.IsNullOrEmpty(term))
        {
            cmd.Parameters.AddWithValue("@term", "%" + term + "%");
        }
        if (!string.IsNullOrEmpty(categoryFilter))
        {
            cmd.Parameters.AddWithValue("@cat", categoryFilter);
        }

        try
        {
            if (con.State != ConnectionState.Open) con.Open();
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
        // Check if already enrolled to prevent PK violation
        string checkQuery = "SELECT COUNT(*) FROM Enrollment WHERE Student_ID = @sid AND Course_Code = @cc";
        SqlCommand checkCmd = new SqlCommand(checkQuery, con);
        checkCmd.Parameters.AddWithValue("@sid", StudentId);
        checkCmd.Parameters.AddWithValue("@cc", CourseCode);

        try
        {
            if (con.State != ConnectionState.Open) con.Open();
            int count = (int)checkCmd.ExecuteScalar();
            if (count > 0) return; // Already enrolled

            // 1. Insert into Enrollment
            string query = "INSERT INTO Enrollment(Student_ID,Course_Code,Enrollment_Date) VALUES (@sid, @cc, @edate)";
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@sid", StudentId);
            cmd.Parameters.AddWithValue("@cc", CourseCode);
            cmd.Parameters.AddWithValue("@edate", DateTime.Now);
            cmd.ExecuteNonQuery();

            // 2. Initialize Grade Record (So Instructor can see them)
            // 2. Initialize Grade Record (So Instructor can see them)
            // Fix: Include Instructor_ID and let Grade_ID be IDENTITY
            string gradeQuery = @"
                INSERT INTO Grade(Student_ID, Course_Code, Completion_Status, Progress, Instructor_ID)
                SELECT @sid, @cc, 'In Progress', 0, Instructor_ID 
                FROM Course WHERE Course_Code = @cc";

            SqlCommand gradeCmd = new SqlCommand(gradeQuery, con);
            gradeCmd.Parameters.AddWithValue("@sid", StudentId);
            gradeCmd.Parameters.AddWithValue("@cc", CourseCode);
            gradeCmd.ExecuteNonQuery();
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

    public DataTable GetGradeDetails(int gradeId)
    {
        DataTable dt = new DataTable();
        // Join with Student and User tables to get Name/Email
        string query = @"SELECT G.*, U.Name, U.Email, C.Title AS CourseTitle 
                         FROM Grade G 
                         LEFT JOIN Student S ON G.Student_ID = S.Student_ID 
                         LEFT JOIN [User] U ON S.Student_ID = U.USER_ID 
                         LEFT JOIN Course C ON G.Course_Code = C.Course_Code 
                         WHERE G.Grade_ID = @gid";
        try
        {
            if (con.State != ConnectionState.Open) con.Open();
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@gid", gradeId);
            dt.Load(cmd.ExecuteReader());
        }
        catch (Exception ex) { Console.WriteLine(ex.Message); }
        finally { con.Close(); }
        return dt;
    }

    public int EnsureGradeExists(int studentId, string courseCode)
    {
        int gradeId = 0;
        try
        {
            if (con.State != ConnectionState.Open) con.Open();

            // Check existing
            string check = "SELECT Grade_ID FROM Grade WHERE Student_ID=@sid AND Course_Code=@cc";
            SqlCommand cmdCheck = new SqlCommand(check, con);
            cmdCheck.Parameters.AddWithValue("@sid", studentId);
            cmdCheck.Parameters.AddWithValue("@cc", courseCode);

            object res = cmdCheck.ExecuteScalar();
            if (res != null && res != DBNull.Value)
            {
                return Convert.ToInt32(res);
            }

            // Create new if missing
            // Grade_ID is IDENTITY, Progress is INT
            try
            {
                string insert = @"
                    INSERT INTO Grade(Student_ID, Course_Code, Completion_Status, Progress, Instructor_ID) 
                    OUTPUT INSERTED.Grade_ID 
                    SELECT @sid, @cc, 'In Progress', 0, Instructor_ID 
                    FROM Course WHERE Course_Code = @cc";

                SqlCommand cmd = new SqlCommand(insert, con);
                cmd.Parameters.AddWithValue("@sid", studentId);
                cmd.Parameters.AddWithValue("@cc", courseCode);
                gradeId = (int)cmd.ExecuteScalar();
            }
            catch (Exception)
            {
                // If Insert failed (e.g. Unique Constraint race condition), try selecting again
                string retryCheck = "SELECT Grade_ID FROM Grade WHERE Student_ID=@sid AND Course_Code=@cc";
                SqlCommand cmdRetry = new SqlCommand(retryCheck, con);
                cmdRetry.Parameters.AddWithValue("@sid", studentId);
                cmdRetry.Parameters.AddWithValue("@cc", courseCode);
                object resRetry = cmdRetry.ExecuteScalar();
                if (resRetry != null && resRetry != DBNull.Value)
                {
                    gradeId = Convert.ToInt32(resRetry);
                }
            }
        }
        catch (Exception ex) { Console.WriteLine(ex.Message); }
        finally { con.Close(); }
        return gradeId;
    }

    public int GetGradeId(int studentId, string courseCode)
    {
        int id = 0;
        try
        {
            if (con.State != ConnectionState.Open) con.Open();
            string query = "SELECT Grade_ID FROM Grade WHERE Student_ID=@sid AND Course_Code=@cc";
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@sid", studentId);
            cmd.Parameters.AddWithValue("@cc", courseCode);
            object result = cmd.ExecuteScalar();
            if (result != null && result != DBNull.Value)
            {
                id = Convert.ToInt32(result);
            }
        }
        catch (Exception ex) { Console.WriteLine(ex.Message); }
        finally { con.Close(); }
        return id;
    }

    public DataTable GetStudentEnrollments(int StudentId)
    {
        DataTable dt = new DataTable();

        // Fix: Added missing JOIN to Enrollment table
        string query = @"SELECT C.*, E.Enrollment_Date
                 FROM COURSE C
                 JOIN Enrollment E ON C.Course_Code = E.Course_Code
                 WHERE E.Student_ID = @sid";
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

    public DataTable GetInstructorCourses(Instructor I)
    {
        DataTable dt = new DataTable();

        string q = "SELECT Course_Code, Title, Category_ID, Fees, Description, Duration\r\n FROM Course \r\n WHERE Instructor_ID = @InstructorID";
        Console.WriteLine("DEBUG: Fetching courses for InstructorID: " + I.USER_ID);
        SqlCommand cmd = new SqlCommand(q, con);
        cmd.Parameters.AddWithValue("@InstructorID", I.USER_ID);
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

    public DataTable GetStudentsForCourse(string courseCode)
    {
        DataTable dt = new DataTable();
        string query = @"SELECT S.Student_ID, U.Name, U.Email, E.Enrollment_Date 
                         FROM Student S 
                         JOIN [User] U ON S.Student_ID = U.USER_ID 
                         JOIN Enrollment E ON S.Student_ID = E.Student_ID 
                         WHERE E.Course_Code = @cc";
        SqlCommand cmd = new SqlCommand(query, con);
        cmd.Parameters.AddWithValue("@cc", courseCode);
        try
        {
            if (con.State != ConnectionState.Open) con.Open();
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

    public void Addcourse(Course newcourse)
    {
        string q = "Insert into Course (Course_Code , Title, Fees, Description, Duration, Category_ID, Admin_ID, Instructor_ID) Values (@ccode,@title,@fee,@des,@duration,@categoryid,@adminid,@instructorid)";
        SqlCommand cmd = new SqlCommand(q, con);
        cmd.Parameters.AddWithValue("@ccode", newcourse.CourseCode);
        cmd.Parameters.AddWithValue("@title", newcourse.Title);
        cmd.Parameters.AddWithValue("@fee", newcourse.Fees);
        cmd.Parameters.AddWithValue("@des", newcourse.Description);
        cmd.Parameters.AddWithValue("@duration", newcourse.Duration);
        cmd.Parameters.AddWithValue("@categoryid", newcourse.Category_ID);
        cmd.Parameters.AddWithValue("@adminid", newcourse.Admin_ID);
        cmd.Parameters.AddWithValue("@instructorid", newcourse.Instructor_ID);

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
        try
        {
            if (con.State != ConnectionState.Open) con.Open();

            // Check duplicate name
            SqlCommand chk = new SqlCommand("SELECT Count(*) From Category Where Category_Name=@cn", con);
            chk.Parameters.AddWithValue("@cn", category.CategoryName);
            if ((int)chk.ExecuteScalar() > 0) return;

            // Manual ID Generation
            SqlCommand cmdId = new SqlCommand("SELECT ISNULL(MAX(Category_ID), 0) + 1 FROM Category", con);
            int newId = (int)cmdId.ExecuteScalar();

            string query = "INSERT INTO CATEGORY (Category_ID, Category_Name, Description, Admin_ID) VALUES (@CategoryID, @CategoryName, @Description, @Admin_ID)";
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@CategoryID", newId);
            cmd.Parameters.AddWithValue("@CategoryName", category.CategoryName);
            cmd.Parameters.AddWithValue("@Description", category.Description);
            cmd.Parameters.AddWithValue("@Admin_ID", category.Admin_ID);
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
        try
        {
            if (con.State != ConnectionState.Open) con.Open();

            // Manual ID Generation
            SqlCommand cmdId = new SqlCommand("SELECT ISNULL(MAX(Resource_ID), 0) + 1 FROM Resource", con);
            int newId = (int)cmdId.ExecuteScalar();

            string query = "INSERT INTO Resource (Resource_ID, Resource_Type, URL, Course_Code, Instructor_ID) VALUES (@id, @type, @url, @cc, @iid)";
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@id", newId);
            cmd.Parameters.AddWithValue("@type", resource.ResourceType);
            cmd.Parameters.AddWithValue("@url", resource.URL);
            cmd.Parameters.AddWithValue("@cc", resource.Course_Code);
            cmd.Parameters.AddWithValue("@iid", resource.Instructor_ID);
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
        try
        {
            if (con.State != ConnectionState.Open) con.Open();

            // Manual ID Generation
            SqlCommand cmdId = new SqlCommand("SELECT ISNULL(MAX(Schedule_ID), 0) + 1 FROM Schedule", con);
            int newId = (int)cmdId.ExecuteScalar();

            string query = "INSERT INTO Schedule (Schedule_ID, Session_Time, Session_Details, Course_Code, Instructor_ID) VALUES (@id, @time, @details, @cc, @iid)";
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@id", newId);
            cmd.Parameters.AddWithValue("@time", schedule.SessionTime);
            cmd.Parameters.AddWithValue("@details", schedule.SessionDetails);
            cmd.Parameters.AddWithValue("@cc", schedule.Course_Code);
            cmd.Parameters.AddWithValue("@iid", schedule.Instructor_ID);
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

    public void UpdateGrade(int initialGradeID, int newGrade, string status)
    {
        string query = "UPDATE Grade SET Progress = @grade, Completion_Status = @status WHERE Grade_ID = @gid";
        SqlCommand cmd = new SqlCommand(query, con);
        cmd.Parameters.AddWithValue("@grade", newGrade);
        cmd.Parameters.AddWithValue("@status", status);
        cmd.Parameters.AddWithValue("@gid", initialGradeID);
        try
        {
            if (con.State != ConnectionState.Open) con.Open();
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

    public DataTable GetCourseReviews(string courseCode)
    {
        DataTable dt = new DataTable();
        string query = "SELECT R.Review_Text, R.Rating, U.Name AS StudentName FROM Review R JOIN Student S ON R.Student_ID = S.Student_ID JOIN [User] U ON S.Student_ID = U.USER_ID WHERE R.Course_Code = @cc";
        SqlCommand cmd = new SqlCommand(query, con);
        cmd.Parameters.AddWithValue("@cc", courseCode);
        try
        {
            if (con.State != ConnectionState.Open) con.Open();
            dt.Load(cmd.ExecuteReader());
        }
        catch (Exception ex) { Console.WriteLine(ex.Message); }
        finally { con.Close(); }
        return dt;
    }

    public void AddReview(Review review)
    {
        try
        {
            if (con.State != ConnectionState.Open) con.Open();

            // Manual ID Generation for Review
            SqlCommand cmdId = new SqlCommand("SELECT ISNULL(MAX(Review_ID), 0) + 1 FROM Review", con);
            int newId = (int)cmdId.ExecuteScalar();

            string query = "INSERT INTO Review (Review_ID, Review_Text, Rating, Course_Code, Student_ID) VALUES (@id, @content, @rating, @cc, @sid)";
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@id", newId);
            cmd.Parameters.AddWithValue("@content", review.Content);
            cmd.Parameters.AddWithValue("@rating", review.Rating);
            cmd.Parameters.AddWithValue("@cc", review.Course_Code);
            cmd.Parameters.AddWithValue("@sid", review.Student_ID);
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

    public bool HasStudentReviewed(int studentId, string courseCode)
    {
        bool hasReviewed = false;
        try
        {
            if (con.State != ConnectionState.Open) con.Open();
            string query = "SELECT COUNT(*) FROM Review WHERE Student_ID = @sid AND Course_Code = @cc";
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@sid", studentId);
            cmd.Parameters.AddWithValue("@cc", courseCode);
            int count = (int)cmd.ExecuteScalar();
            if (count > 0) hasReviewed = true;
        }
        catch (Exception ex) { Console.WriteLine(ex.Message); }
        finally { con.Close(); }
        return hasReviewed;
    }

    public DataTable GetStudentsWithGrades(string courseCode)
    {
        DataTable dt = new DataTable();

        // Use Enrollment as base table to ensure all students appear
        // LEFT JOIN Grade to show progress if it exists, or defaults if not
        string betterQuery = @"
            SELECT 
                ISNULL(G.Grade_ID, 0) AS Grade_ID, 
                E.Student_ID, 
                U.Name, 
                U.Email, 
                ISNULL(G.Completion_Status, 'Not Started') AS Completion_Status, 
                ISNULL(G.Progress, 0) AS Progress
            FROM Enrollment E
            JOIN Student S ON E.Student_ID = S.Student_ID
            JOIN [User] U ON S.Student_ID = U.USER_ID
            LEFT JOIN Grade G ON E.Student_ID = G.Student_ID AND E.Course_Code = G.Course_Code
            WHERE E.Course_Code = @cc";

        SqlCommand cmd = new SqlCommand(betterQuery, con);
        cmd.Parameters.AddWithValue("@cc", courseCode);
        try
        {
            if (con.State != ConnectionState.Open) con.Open();
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

    public int UpdateCourse(Course course)
    {
        int result = 0;
        string q = "Update Course Set Course_Code=@ccode,Title=@title,Fees=@fee,Description=@des,Duration=@duration,Category_ID=@categoryid,Admin_ID=@adminid,Instructor_ID=@instructorid Where Course_Code=@ccode";
        SqlCommand cmd = new SqlCommand(q, con);
        cmd.Parameters.AddWithValue("@ccode", course.CourseCode);
        cmd.Parameters.AddWithValue("@title", course.Title);
        cmd.Parameters.AddWithValue("@fee", course.Fees);
        cmd.Parameters.AddWithValue("@des", course.Description);
        cmd.Parameters.AddWithValue("@duration", course.Duration);
        cmd.Parameters.AddWithValue("@categoryid", course.Category_ID);
        cmd.Parameters.AddWithValue("@adminid", course.Admin_ID);
        cmd.Parameters.AddWithValue("@instructorid", course.Instructor_ID);
        try
        {
            con.Open();
            result = cmd.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        finally
        {
            con.Close();
        }
        return result;
    }

    public void DeleteCourse(string courseCode)
    {
        string q = "Delete from Course where Course_Code=@ccode";
        SqlCommand cmd = new SqlCommand(q, con);
        cmd.Parameters.AddWithValue("@ccode", courseCode);
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
    public void DeleteCategory(Category c)
    {
        string query = "Delete From Category where Category_ID=@cat_id";
        SqlCommand cmd = new SqlCommand(query, con);
        cmd.Parameters.AddWithValue("@cat_id", c.CategoryID);
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

    public void ArchiveCategory(int categoryId)
    {
        try
        {
            if (con.State != ConnectionState.Open) con.Open();
            // Prefix name with "Archived - "
            string query = "UPDATE Category SET Category_Name = CONCAT('Archived - ', Category_Name) WHERE Category_ID=@id AND Category_Name NOT LIKE 'Archived - %'";
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@id", categoryId);
            cmd.ExecuteNonQuery();

            // CASCADE: Archive Courses by renaming them too
            string queryCourses = "UPDATE Course SET Title = CONCAT('Archived - ', Title) WHERE Category_ID=@id AND Title NOT LIKE 'Archived - %'";
            SqlCommand cmd2 = new SqlCommand(queryCourses, con);
            cmd2.Parameters.AddWithValue("@id", categoryId);
            cmd2.ExecuteNonQuery();
        }
        catch (Exception ex) { Console.WriteLine(ex.Message); }
        finally { con.Close(); }
    }

    public void UnarchiveCategory(int categoryId)
    {
        try
        {
            if (con.State != ConnectionState.Open) con.Open();
            // Remove "Archived - " prefix (Length 11)
            string query = "UPDATE Category SET Category_Name = SUBSTRING(Category_Name, 12, LEN(Category_Name)) WHERE Category_ID=@id AND Category_Name LIKE 'Archived - %'";
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@id", categoryId);
            cmd.ExecuteNonQuery();

            // CASCADE: Unarchive Courses
            string queryCourses = "UPDATE Course SET Title = SUBSTRING(Title, 12, LEN(Title)) WHERE Category_ID=@id AND Title LIKE 'Archived - %'";
            SqlCommand cmd2 = new SqlCommand(queryCourses, con);
            cmd2.Parameters.AddWithValue("@id", categoryId);
            cmd2.ExecuteNonQuery();
        }
        catch (Exception ex) { Console.WriteLine(ex.Message); }
        finally { con.Close(); }
    }

    public void ArchiveCourse(string courseCode)
    {
        try
        {
            if (con.State != ConnectionState.Open) con.Open();
            string query = "UPDATE Course SET Title = CONCAT('Archived - ', Title) WHERE Course_Code=@id AND Title NOT LIKE 'Archived - %'";
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@id", courseCode);
            cmd.ExecuteNonQuery();
        }
        catch (Exception ex) { Console.WriteLine(ex.Message); }
        finally { con.Close(); }
    }

    public void UnarchiveCourse(string courseCode)
    {
        try
        {
            if (con.State != ConnectionState.Open) con.Open();
            string query = "UPDATE Course SET Title = SUBSTRING(Title, 12, LEN(Title)) WHERE Course_Code=@id AND Title LIKE 'Archived - %'";
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@id", courseCode);
            cmd.ExecuteNonQuery();
        }
        catch (Exception ex) { Console.WriteLine(ex.Message); }
        finally { con.Close(); }
    }

    public void DeleteInstructor(Instructor I)
    {
        // Delete from [User] triggering cascade to Instructor, Teaches, etc.
        string query = "Delete From [User] where USER_ID=@i_id ";
        SqlCommand cmd = new SqlCommand(query, con);
        cmd.Parameters.AddWithValue("@i_id", I.USER_ID);

        try
        {
            if (con.State != ConnectionState.Open) con.Open();
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
    public void DeleteStudent(Student S)
    {
        // Delete from [User] triggering cascade to Student, Enrollment, etc.
        string query = "Delete From [User] where USER_ID=@s_id";
        SqlCommand cmd = new SqlCommand(query, con);
        cmd.Parameters.AddWithValue("@s_id", S.USER_ID);

        try
        {
            if (con.State != ConnectionState.Open) con.Open();
            cmd.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }





    public Course GetCourse(string courseCode)
    {
        Course c = new Course();
        // Updated query to fetch Instructor Name
        string query = @"SELECT C.*, U.Name AS InstructorName 
                         FROM COURSE C 
                         LEFT JOIN Instructor I ON C.Instructor_ID = I.Instructor_ID 
                         LEFT JOIN [User] U ON I.Instructor_ID = U.USER_ID 
                         WHERE C.Course_Code = @ccode";
        SqlCommand cmd = new SqlCommand(query, con);
        cmd.Parameters.AddWithValue("@ccode", courseCode);
        try
        {
            if (con.State != ConnectionState.Open) con.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                c.CourseCode = reader["Course_Code"].ToString();
                c.Title = reader["Title"].ToString();
                c.Description = reader["Description"] != DBNull.Value ? reader["Description"].ToString() : "";
                c.Duration = reader["Duration"] != DBNull.Value ? reader["Duration"].ToString() : "";
                c.Fees = reader["Fees"] != DBNull.Value ? Convert.ToDecimal(reader["Fees"]) : 0;
                c.Instructor_ID = reader["Instructor_ID"] != DBNull.Value ? Convert.ToInt32(reader["Instructor_ID"]) : 0;
                c.Category_ID = reader["Category_ID"] != DBNull.Value ? Convert.ToInt32(reader["Category_ID"]) : 0;
                c.Admin_ID = reader["Admin_ID"] != DBNull.Value ? Convert.ToInt32(reader["Admin_ID"]) : 0;

                // Populate Instructor Object
                c.Instructor = new Instructor();
                c.Instructor.Name = reader["InstructorName"] != DBNull.Value ? reader["InstructorName"].ToString() : "Unknown";
            }
            reader.Close();
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
    public int Approve_Instructor(Instructor I)
    {
        int result = 0;
        string query = $"Update Instructor Set Approval_Status='Approved' where Instructor_ID={I.USER_ID}";
        SqlCommand cmd = new SqlCommand(query, con);

        try
        {
            con.Open();
            result = cmd.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        finally
        {
            con.Close();
        }
        return result;
    }
    public int Donot_Approve_Instructor(Instructor I)
    {
        int result = 0;
        string query = $"Update Instructor Set Approval_Status='Rejected' where Instructor_ID={I.USER_ID}";
        SqlCommand cmd = new SqlCommand(query, con);

        try
        {
            con.Open();
            result = cmd.ExecuteNonQuery();


        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        finally
        {
            con.Close();
        }
        return result;

    }
    public int GETAdminWhoApproved(Instructor I)
    {
        int Admin_ID = 0;
        DataTable dt = new DataTable();
        string query = " Select Admin_ID\r\n From INSTRUCTOR I\r\n where I.Approval_Status='Approved' and I.Instructor_ID=@I_id";
        SqlCommand cmd = new SqlCommand(query, con);
        cmd.Parameters.AddWithValue("@I_id", I.USER_ID);

        try
        {
            if (con.State != ConnectionState.Open) con.Open();
            Admin_ID = (int)cmd.ExecuteScalar();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        finally
        {
            con.Close();
        }
        return Admin_ID;
    }

    public DataTable GetInstructors(string statusFilter)
    {
        DataTable dt = new DataTable();
        string query = "Select * From Instructor,[USER] WHERE Instructor_ID=USER_ID";

        if (!string.IsNullOrEmpty(statusFilter) && statusFilter != "All")
        {
            query += " AND Approval_Status = @status";
        }

        SqlCommand cmd = new SqlCommand(query, con);
        if (!string.IsNullOrEmpty(statusFilter) && statusFilter != "All")
        {
            cmd.Parameters.AddWithValue("@status", statusFilter);
        }

        try
        {
            if (con.State != ConnectionState.Open) con.Open();
            dt.Load(cmd.ExecuteReader());
        }
        catch (Exception ex) { Console.WriteLine(ex.Message); }
        finally { con.Close(); }
        return dt;
    }

    public DataTable SearchStudents(string term)
    {
        DataTable dt = new DataTable();
        string query = "Select * From Student,[User] WHERE Student_ID=USER_ID";
        if (!string.IsNullOrEmpty(term))
        {
            query += " AND (Name LIKE @term OR Email LIKE @term)";
        }

        SqlCommand cmd = new SqlCommand(query, con);
        if (!string.IsNullOrEmpty(term))
        {
            cmd.Parameters.AddWithValue("@term", "%" + term + "%");
        }

        try
        {
            if (con.State != ConnectionState.Open) con.Open();
            dt.Load(cmd.ExecuteReader());
        }
        catch (Exception ex) { Console.WriteLine(ex.Message); }
        finally { con.Close(); }
        return dt;
    }




}

