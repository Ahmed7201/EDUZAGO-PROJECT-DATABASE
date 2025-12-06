using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using EDUZAGO_PROJECT_DATABASE.Models;

namespace EDUZAGO_PROJECT_DATABASE.Pages.InstructorNamespace
{
    public class ProfileModel : PageModel
    {
        [BindProperty]
        public EDUZAGO_PROJECT_DATABASE.Models.Instructor MockInstructor { get; set; } = new EDUZAGO_PROJECT_DATABASE.Models.Instructor();

        public IActionResult OnGet()
        {
            var role = HttpContext.Session.GetString("Role");
            if (role != "Instructor") return RedirectToPage("/Account/Login");

            // Mock Data
            MockInstructor = new EDUZAGO_PROJECT_DATABASE.Models.Instructor
            {
                Name = "Mock Instructor",
                Email = "i-user@eduzago.com",
                Expertise = "Computer Science, AI",
                Bio = "PhD in Artificial Intelligence with 10 years of teaching experience."
            };

            return Page();
        }

        public IActionResult OnPost()
        {
            // Mock Save
            return RedirectToPage("./Dashboard");
        }
    }
}
