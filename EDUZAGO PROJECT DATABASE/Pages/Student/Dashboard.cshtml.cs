using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using EDUZAGO_PROJECT_DATABASE.Models;
using System.Data;

namespace EDUZAGO_PROJECT_DATABASE.Pages.StudentNamespace
{
    public class DashboardModel : PageModel
    {
        private readonly DB db;

        public DashboardModel(DB db)
        {
            this.db = db;
        }

        public EDUZAGO_PROJECT_DATABASE.Models.Student Student { get; set; }
        public List<EDUZAGO_PROJECT_DATABASE.Models.Course> EnrolledCourses { get; set; } = new List<EDUZAGO_PROJECT_DATABASE.Models.Course>();
        public List<AggregatedScheduleItem> WeeklySchedule { get; set; } = new List<AggregatedScheduleItem>();

        public IActionResult OnGet()
        {
            var role = HttpContext.Session.GetString("Role");
            var userIdStr = HttpContext.Session.GetString("UserId");

            if (role != "Student" || string.IsNullOrEmpty(userIdStr))
            {
                return RedirectToPage("/Account/Login");
            }

            int studentId = int.Parse(userIdStr);

            // Fetch Student Details (Optional given Session, but good for completeness)
            // Student = db.GetStudent(studentId); // Assuming you might add this later

            // Fetch Real Enrollments
            DataTable dtEnrollments = db.GetStudentEnrollments(studentId);

            foreach (DataRow row in dtEnrollments.Rows)
            {
                EnrolledCourses.Add(new Course
                {
                    CourseCode = row["Course_Code"].ToString(),
                    Title = row["Title"].ToString(),
                    Description = row["Description"]?.ToString(),
                    Duration = row["Duration"]?.ToString()
                });
            }

            // Fetch Schedules for all enrolled courses
            // This logic aggregates schedule from all enrolled courses
            foreach (var course in EnrolledCourses)
            {
                DataTable dtSchedule = db.GetSchedule(course.CourseCode);
                foreach (DataRow row in dtSchedule.Rows)
                {
                    WeeklySchedule.Add(new AggregatedScheduleItem
                    {
                        CourseCode = course.CourseCode,
                        CourseTitle = course.Title,
                        SessionDetails = row["Session_Details"]?.ToString(),
                        SessionTime = row["Session_Time"] != DBNull.Value ? Convert.ToDateTime(row["Session_Time"]) : DateTime.MinValue
                    });
                }
            }

            // Sort schedule by date
            WeeklySchedule = WeeklySchedule.OrderBy(s => s.SessionTime).ToList();

            return Page();
        }

        public class AggregatedScheduleItem
        {
            public string CourseCode { get; set; }
            public string CourseTitle { get; set; }
            public string SessionDetails { get; set; }
            public DateTime SessionTime { get; set; }
        }
    }
}
