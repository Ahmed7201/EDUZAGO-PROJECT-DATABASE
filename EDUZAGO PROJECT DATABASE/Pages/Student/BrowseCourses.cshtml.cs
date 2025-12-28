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

            DataTable rawCourses;
            if (!string.IsNullOrEmpty(SearchText))
            {
                rawCourses = db.SearchCourses(SearchText); // Already filtered in DB
            }
            else
            {
                rawCourses = db.GetAllCourses(); // Returns All (including Archived)
            }

            // Filter out Archived courses from display
            // Use DataView for easy filtering
            DataView dv = rawCourses.DefaultView;
            dv.RowFilter = "Title NOT LIKE 'Archived - %'";
            Courses = dv.ToTable();

            return Page();
        }

        public IActionResult OnPostEnroll(string courseCode)
        {
            var studentIdStr = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(studentIdStr)) return RedirectToPage("/Account/Login");

            // Redirect to Payment page with the selected Course Code
            return RedirectToPage("./Payment", new { courseCode = courseCode });
        }
    }
}
