using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using EDUZAGO_PROJECT_DATABASE.Models;

namespace EDUZAGO_PROJECT_DATABASE.Pages.InstructorNamespace
{
    public class ProfileModel : PageModel
    {
        private readonly DB db;
        public ProfileModel(DB db) { this.db = db; }

        [BindProperty]
        public EDUZAGO_PROJECT_DATABASE.Models.Instructor MockInstructor { get; set; } = new EDUZAGO_PROJECT_DATABASE.Models.Instructor();

        public IActionResult OnGet()
        {
            var role = HttpContext.Session.GetString("Role");
            var userId = HttpContext.Session.GetString("UserId");
            if (role != "Instructor" || string.IsNullOrEmpty(userId)) return RedirectToPage("/Account/Login");

            MockInstructor = db.GetInstructorProfile(int.Parse(userId));
            return Page();
        }

        public IActionResult OnPost()
        {
            var userId = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userId)) return RedirectToPage("/Account/Login");

            MockInstructor.USER_ID = int.Parse(userId);
            db.UpdateInstructorProfile(MockInstructor);
            return RedirectToPage();
        }
    }
}
