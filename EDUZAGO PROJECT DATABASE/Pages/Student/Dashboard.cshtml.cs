using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using EDUZAGO_PROJECT_DATABASE.Models;

namespace EDUZAGO_PROJECT_DATABASE.Pages.StudentNamespace
{
    public class DashboardModel : PageModel
    {
        public DashboardModel()
        {
        }

        public EDUZAGO_PROJECT_DATABASE.Models.Student Student { get; set; }
        public List<EDUZAGO_PROJECT_DATABASE.Models.Course> EnrolledCourses { get; set; } = new List<EDUZAGO_PROJECT_DATABASE.Models.Course>();
        public List<AggregatedScheduleItem> WeeklySchedule { get; set; } = new List<AggregatedScheduleItem>();

        public IActionResult OnGet()
        {
            var role = HttpContext.Session.GetString("Role");
            if (role != "Student")
            {
                return RedirectToPage("/Account/Login");
            }

            // Mock Data
            Student = new EDUZAGO_PROJECT_DATABASE.Models.Student
            {
                Name = "Mock Student",
                Email = "s-user@eduzago.com"
            };

            EnrolledCourses = new List<EDUZAGO_PROJECT_DATABASE.Models.Course>
            {
                new EDUZAGO_PROJECT_DATABASE.Models.Course { CourseCode = "CS-101", Title = "Intro to C# (Mock)", Duration = "4 Weeks" },
                new EDUZAGO_PROJECT_DATABASE.Models.Course { CourseCode = "WD-102", Title = "Web Dev Basics (Mock)", Duration = "6 Weeks" }
            };

            // Mock Aggregated Schedule
            WeeklySchedule = new List<AggregatedScheduleItem>
            {
                new AggregatedScheduleItem { CourseCode="CS-101", CourseTitle="Intro to C#", SessionDetails="Loops & Logic", SessionTime=DateTime.Now.AddDays(0) }, // Today
                new AggregatedScheduleItem { CourseCode="WD-102", CourseTitle="Web Dev Basics", SessionDetails="HTML Forms", SessionTime=DateTime.Now.AddDays(2) }
            };

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
