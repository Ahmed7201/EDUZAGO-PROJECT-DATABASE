using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using EDUZAGO_PROJECT_DATABASE.Models;
using System.Data;

namespace EDUZAGO_PROJECT_DATABASE.Pages.InstructorNamespace
{
    public class ManageCoursesModel : PageModel
    {
        public DB db { get; set; }

        public ManageCoursesModel(DB d)
        {
            db = d;
        }

        public DataTable MyCourses { get; set; } = new DataTable();

        public IActionResult OnGet()
        {
            var role = HttpContext.Session.GetString("Role");
            var userIdStr = HttpContext.Session.GetString("UserId");

            if (role != "Instructor" || string.IsNullOrEmpty(userIdStr))
            {
                return RedirectToPage("/Account/Login");
            }

            int userId = int.Parse(userIdStr);
            EDUZAGO_PROJECT_DATABASE.Models.Instructor instructor = new EDUZAGO_PROJECT_DATABASE.Models.Instructor { USER_ID = userId };

            MyCourses = db.GetInstructorCourses(instructor);

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
