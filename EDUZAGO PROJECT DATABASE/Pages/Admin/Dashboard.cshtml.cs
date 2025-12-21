using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using EDUZAGO_PROJECT_DATABASE.Models;
using System.Data;
namespace EDUZAGO_PROJECT_DATABASE.Pages.AdminNamespace
{
    public class DashboardModel : PageModel
    {
        public DB db { get; set; }
        public DashboardModel(DB d)
        {
            db = d;
        }

        public int StudentCount { get; set; }
        public int InstructorCount { get; set; }
        public int CourseCount { get; set; }
        public int PendingInstructors { get; set; }

        public IActionResult OnGet()
        {
            var role = HttpContext.Session.GetString("Role");
            if (role != "Admin") return RedirectToPage("/Account/Login");

            // Mock Stats
            StudentCount = db.Get_StudentCount();
            InstructorCount = db.Get_InstructorCount();
            CourseCount = db.Get_CourseCount();
            PendingInstructors = 0;

            return Page();
        }
    }
}
