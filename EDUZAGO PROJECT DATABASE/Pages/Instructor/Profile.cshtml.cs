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

        [BindProperty]
        public string OldPassword { get; set; }

        [BindProperty]
        public string NewPassword { get; set; }

        public string Message { get; set; }

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

        public IActionResult OnPostChangePassword()
        {
            var userId = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userId)) return RedirectToPage("/Account/Login");

            if (string.IsNullOrEmpty(OldPassword) || string.IsNullOrEmpty(NewPassword))
            {
                Message = "Please enter both old and new passwords.";
            }
            else
            {
                if (db.UpdatePassword(int.Parse(userId), OldPassword, NewPassword))
                {
                    Message = "Password updated successfully!";
                }
                else
                {
                    Message = "Incorrect old password. Please try again.";
                }
            }

            // Reload profile data 
            MockInstructor = db.GetInstructorProfile(int.Parse(userId));
            return Page();
        }
    }
}
