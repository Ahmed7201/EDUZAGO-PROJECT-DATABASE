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

            // Mock schedule for display (as DB doesn't have an aggregated schedule query yet)
            WeeklySchedule = new List<AggregatedScheduleItem>
            {
                new AggregatedScheduleItem { CourseCode="AI-201", CourseTitle="Advanced AI", SessionDetails="Neural Networks", SessionTime=DateTime.Now.AddDays(0) },
                new AggregatedScheduleItem { CourseCode="CC-202", CourseTitle="Cloud Computing", SessionDetails="AWS Basics", SessionTime=DateTime.Now.AddDays(1) }
            };

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
