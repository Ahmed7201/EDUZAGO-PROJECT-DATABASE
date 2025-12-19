using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using EDUZAGO_PROJECT_DATABASE.Models;

namespace EDUZAGO_PROJECT_DATABASE.Pages.InstructorNamespace
{
    public class ManageCourseModel : PageModel
    {
        public ManageCourseModel()
        {
        }

        [BindProperty]
        public Course Course { get; set; } = new Course();

        public List<SelectListItem> Categories { get; set; } = new List<SelectListItem>();

        public IActionResult OnGet(string? id)
        {
            var role = HttpContext.Session.GetString("Role");
            if (role != "Instructor") return RedirectToPage("/Account/Login");

            Categories = new List<SelectListItem>
            {
                new SelectListItem { Value = "1", Text = "IT" },
                new SelectListItem { Value = "2", Text = "Business" }
            };

            if (!string.IsNullOrEmpty(id))
            {
                // Mock Existing Course
                Course = new Course
                {
                    CourseCode = id,
                    Title = "Existing Mock Course",
                    Description = "This is a mock description",
                    Duration = "5 Weeks",
                    Fees = 50
                };
            }

            return Page();
        }

        public IActionResult OnPost()
        {

            return RedirectToPage("./Dashboard");
        }
    }
}
