using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using EDUZAGO_PROJECT_DATABASE.Models;
using System.Data;

namespace EDUZAGO_PROJECT_DATABASE.Pages.AdminNamespace
{
    public class ManageCoursesModel : PageModel
    {
        public DB db { get; set; }
        public ManageCoursesModel(DB d)
        {
            db = d;
        }

        public DataTable AllCourses { get; set; } = new DataTable();

        public IActionResult OnGet()
        {
            var role = HttpContext.Session.GetString("Role");
            if (role != "Admin") return RedirectToPage("/Account/Login");

            AllCourses = db.GetAllCourses();
            return Page();
        }

        public IActionResult OnPostDelete(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                db.DeleteCourse(id);
            }
            return RedirectToPage();
        }
    }
}
