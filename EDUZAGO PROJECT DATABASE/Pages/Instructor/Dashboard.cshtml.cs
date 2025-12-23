using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using EDUZAGO_PROJECT_DATABASE.Models;
using System.Data;

namespace EDUZAGO_PROJECT_DATABASE.Pages.InstructorNamespace
{
    public class DashboardModel : PageModel
    {
        private readonly DB db;

        public DashboardModel(DB db)
        {
            this.db = db;
        }

        public EDUZAGO_PROJECT_DATABASE.Models.Instructor CurrentInstructor { get; set; } = new EDUZAGO_PROJECT_DATABASE.Models.Instructor();
        public DataTable MyCourses { get; set; } = new DataTable();

        // Simple class for the schedule view
        public class AggregatedScheduleItem
        {
            public string CourseCode { get; set; }
            public string CourseTitle { get; set; }
            public string SessionDetails { get; set; }
            public DateTime SessionTime { get; set; }
        }
        public List<AggregatedScheduleItem> WeeklySchedule { get; set; } = new List<AggregatedScheduleItem>();

        public IActionResult OnGet()
        {
            var role = HttpContext.Session.GetString("Role");
            if (role != "Instructor")
            {
                return RedirectToPage("/Account/Login");
            }

            // Populate from session
            CurrentInstructor = new EDUZAGO_PROJECT_DATABASE.Models.Instructor();
            CurrentInstructor.USER_ID = Convert.ToInt32(HttpContext.Session.GetString("UserId"));
            CurrentInstructor.Name = HttpContext.Session.GetString("UserName") ?? "Instructor";

            // Use the object's ID to fetch courses
            MyCourses = db.GetInstructorCourses(CurrentInstructor);

            // Populate schedule from real courses (mocking the time slots)
            WeeklySchedule = new List<AggregatedScheduleItem>();
            int dayOffset = 0;
            foreach (DataRow row in MyCourses.Rows)
            {
                WeeklySchedule.Add(new AggregatedScheduleItem
                {
                    CourseCode = row["Course_Code"].ToString(),
                    CourseTitle = row["Title"].ToString(),
                    SessionDetails = "Weekly Session",
                    SessionTime = DateTime.Now.AddDays(dayOffset % 7) // Spread across the week
                });
                dayOffset++;
            }

            return Page();
        }

        public IActionResult OnPostDeleteCourse(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                db.DeleteCourse(id);
            }
            return RedirectToPage();
        }
    }
}
