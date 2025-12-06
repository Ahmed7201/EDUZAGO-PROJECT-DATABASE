using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using EDUZAGO_PROJECT_DATABASE.Models;

namespace EDUZAGO_PROJECT_DATABASE.Pages.StudentNamespace
{
    public class BrowseCoursesModel : PageModel
    {
        public BrowseCoursesModel()
        {
        }

        public List<EDUZAGO_PROJECT_DATABASE.Models.Course> Courses { get; set; } = new List<EDUZAGO_PROJECT_DATABASE.Models.Course>();

        [BindProperty(SupportsGet = true)]
        public string SearchTerm { get; set; } = string.Empty;

        public IActionResult OnGet()
        {
            var role = HttpContext.Session.GetString("Role");
            if (role != "Student") return RedirectToPage("/Account/Login");

            // Mock Data
            var allCourses = new List<EDUZAGO_PROJECT_DATABASE.Models.Course>
             {
                 new EDUZAGO_PROJECT_DATABASE.Models.Course { CourseCode = 1, Title = "Machine Learning A-Z", Description = "Learn ML", Duration = "10 Weeks", Fees = 99.99m },
                 new EDUZAGO_PROJECT_DATABASE.Models.Course { CourseCode = 2, Title = "Web Development Bootcamp", Description = "Full Stack Dev", Duration = "12 Weeks", Fees = 149.99m },
                 new EDUZAGO_PROJECT_DATABASE.Models.Course { CourseCode = 3, Title = "Data Science with Python", Description = "Data Analysis", Duration = "8 Weeks", Fees = 89.99m }
             };

            if (!string.IsNullOrEmpty(SearchTerm))
            {
                Courses = allCourses.Where(c => c.Title.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase)).ToList();
            }
            else
            {
                Courses = allCourses;
            }

            return Page();
        }

        public IActionResult OnPostEnroll(int courseId)
        {
            // Mock Enrollment
            // Just redirect to Dashboard
            return RedirectToPage("./Dashboard");
        }
    }
}
