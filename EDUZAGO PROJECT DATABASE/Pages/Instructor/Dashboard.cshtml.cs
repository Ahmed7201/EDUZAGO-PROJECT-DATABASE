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
                new EDUZAGO_PROJECT_DATABASE.Models.Course { CourseCode = 201, Title = "Advanced AI (Mock)", Duration = "8 Weeks" },
                new EDUZAGO_PROJECT_DATABASE.Models.Course { CourseCode = 202, Title = "Cloud Computing (Mock)", Duration = "5 Weeks" }
            };

            return Page();
        }
    }
}
