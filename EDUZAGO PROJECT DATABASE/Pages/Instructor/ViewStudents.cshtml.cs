using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using EDUZAGO_PROJECT_DATABASE.Models;

namespace EDUZAGO_PROJECT_DATABASE.Pages.InstructorNamespace
{
    public class ViewStudentsModel : PageModel
    {
        public ViewStudentsModel()
        {
        }

        public EDUZAGO_PROJECT_DATABASE.Models.Course Course { get; set; }
        public List<EDUZAGO_PROJECT_DATABASE.Models.Student> EnrolledStudents { get; set; } = new List<EDUZAGO_PROJECT_DATABASE.Models.Student>();

        public IActionResult OnGet(int courseId)
        {
            var role = HttpContext.Session.GetString("Role");
            if (role != "Instructor") return RedirectToPage("/Account/Login");

            // Mock Data
            Course = new EDUZAGO_PROJECT_DATABASE.Models.Course { CourseCode = courseId, Title = "Mock Course " + courseId };

            EnrolledStudents = new List<EDUZAGO_PROJECT_DATABASE.Models.Student>
             {
                 new EDUZAGO_PROJECT_DATABASE.Models.Student { Name = "Alice Mock", Email = "alice@test.com" },
                 new EDUZAGO_PROJECT_DATABASE.Models.Student { Name = "Bob Mock", Email = "bob@test.com" }
             };

            return Page();
        }
    }
}
