using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using EDUZAGO_PROJECT_DATABASE.Models;

namespace EDUZAGO_PROJECT_DATABASE.Pages.StudentNamespace
{
    public class ProfileModel : PageModel
    {
        private readonly DB db;
        public ProfileModel(DB db) { this.db = db; }

        [BindProperty]
        public EDUZAGO_PROJECT_DATABASE.Models.Student MockStudent { get; set; } = new EDUZAGO_PROJECT_DATABASE.Models.Student();

        public IActionResult OnGet()
        {
            var role = HttpContext.Session.GetString("Role");
            var userId = HttpContext.Session.GetString("UserId");
            if (role != "Student" || string.IsNullOrEmpty(userId)) return RedirectToPage("/Account/Login");

            MockStudent = db.GetStudentProfile(int.Parse(userId));
            return Page();
        }

        public IActionResult OnPost()
        {
            var userId = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userId)) return RedirectToPage("/Account/Login");

            MockStudent.USER_ID = int.Parse(userId);
            db.UpdateStudentProfile(MockStudent);
            return RedirectToPage();
        }
    }
}
