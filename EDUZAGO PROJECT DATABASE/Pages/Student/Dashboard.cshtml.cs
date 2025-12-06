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
                new EDUZAGO_PROJECT_DATABASE.Models.Course { CourseCode = 101, Title = "Intro to C# (Mock)", Duration = "4 Weeks" },
                new EDUZAGO_PROJECT_DATABASE.Models.Course { CourseCode = 102, Title = "Web Dev Basics (Mock)", Duration = "6 Weeks" }
            };

            return Page();
        }
    }
}
