using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EDUZAGO_PROJECT_DATABASE.Pages.AdminNamespace
{
    public class DashboardModel : PageModel
    {
        public DashboardModel()
        {
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
            StudentCount = 1250;
            InstructorCount = 45;
            CourseCount = 82;
            PendingInstructors = 3;

            return Page();
        }
    }
}
