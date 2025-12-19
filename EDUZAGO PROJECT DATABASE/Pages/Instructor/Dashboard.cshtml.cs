using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using EDUZAGO_PROJECT_DATABASE.Models;

namespace EDUZAGO_PROJECT_DATABASE.Pages.InstructorNamespace
{
    public class DashboardModel : PageModel
    {
        public DashboardModel()
        {
        }

        public EDUZAGO_PROJECT_DATABASE.Models.Instructor Instructor { get; set; }
        public List<EDUZAGO_PROJECT_DATABASE.Models.Course> MyCourses { get; set; } = new List<EDUZAGO_PROJECT_DATABASE.Models.Course>();
        public List<AggregatedScheduleItem> WeeklySchedule { get; set; } = new List<AggregatedScheduleItem>();

        public IActionResult OnGet()
        {
            var role = HttpContext.Session.GetString("Role");
            if (role != "Instructor")
            {
                return RedirectToPage("/Account/Login");
            }

            // Mock Data
            Instructor = new EDUZAGO_PROJECT_DATABASE.Models.Instructor
            {
                Name = "Mock Instructor",
                Email = "i-user@eduzago.com"
            };

            MyCourses = new List<EDUZAGO_PROJECT_DATABASE.Models.Course>
            {
                new EDUZAGO_PROJECT_DATABASE.Models.Course { CourseCode = "AI-201", Title = "Advanced AI (Mock)", Duration = "8 Weeks" },
                new EDUZAGO_PROJECT_DATABASE.Models.Course { CourseCode = "CC-202", Title = "Cloud Computing (Mock)", Duration = "5 Weeks" }
            };

            // Mock Aggregated Schedule
            WeeklySchedule = new List<AggregatedScheduleItem>
            {
                new AggregatedScheduleItem { CourseCode="AI-201", CourseTitle="Advanced AI", SessionDetails="Neural Networks", SessionTime=DateTime.Now.AddDays(0) }, // Today
                new AggregatedScheduleItem { CourseCode="CC-202", CourseTitle="Cloud Computing", SessionDetails="AWS Basics", SessionTime=DateTime.Now.AddDays(1) },
                new AggregatedScheduleItem { CourseCode="AI-201", CourseTitle="Advanced AI", SessionDetails="Labs", SessionTime=DateTime.Now.AddDays(3) }
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
