using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;
using EDUZAGO_PROJECT_DATABASE.Models;

namespace EDUZAGO_PROJECT_DATABASE.Pages.StudentNamespace
{
    public class BrowseCoursesModel : PageModel
    {
        private readonly DB db;

        public BrowseCoursesModel(DB db)
        {
            this.db = db;
        }

        public DataTable Courses { get; set; } = new DataTable();

        [BindProperty(SupportsGet = true)]
        public string SearchText { get; set; } = string.Empty;

        public IActionResult OnGet()
        {
            var role = HttpContext.Session.GetString("Role");
            if (role != "Student") return RedirectToPage("/Account/Login");

            if (!string.IsNullOrEmpty(SearchText))
            {
                Courses = db.SearchCourses(SearchText);
            }
            else
            {
                Courses = db.GetAllCourses();
            }
            return Page();
        }

        public IActionResult OnPostEnroll(string courseCode)
        {
            var studentIdStr = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(studentIdStr)) return RedirectToPage("/Account/Login");

            int studentId = int.Parse(studentIdStr);
            // Assuming EnrollStudent exists in DB as per project context
            db.EnrollStudent(studentId, courseCode);

            return RedirectToPage("./Dashboard");
        }
    }
}
