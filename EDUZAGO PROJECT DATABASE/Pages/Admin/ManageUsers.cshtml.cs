using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using EDUZAGO_PROJECT_DATABASE.Models;

namespace EDUZAGO_PROJECT_DATABASE.Pages.AdminNamespace
{
    public class ManageUsersModel : PageModel
    {
        public ManageUsersModel()
        {
        }

        public List<EDUZAGO_PROJECT_DATABASE.Models.Instructor> Instructors { get; set; } = new List<EDUZAGO_PROJECT_DATABASE.Models.Instructor>();
        public List<EDUZAGO_PROJECT_DATABASE.Models.Student> Students { get; set; } = new List<EDUZAGO_PROJECT_DATABASE.Models.Student>();

        public IActionResult OnGet()
        {
            var role = HttpContext.Session.GetString("Role");
            if (role != "Admin") return RedirectToPage("/Account/Login");

            // Mock Users
            Instructors = new List<EDUZAGO_PROJECT_DATABASE.Models.Instructor>
             {
                 new EDUZAGO_PROJECT_DATABASE.Models.Instructor { InstructorID = 1, Name = "Dr. Strange", Email = "strange@edu.com", IsApproved = true },
                 new EDUZAGO_PROJECT_DATABASE.Models.Instructor { InstructorID = 2, Name = "Pending Prof", Email = "pending@edu.com", IsApproved = false }
             };

            Students = new List<EDUZAGO_PROJECT_DATABASE.Models.Student>
             {
                 new EDUZAGO_PROJECT_DATABASE.Models.Student { StudentID = 1, Name = "Peter Parker", Email = "spidey@edu.com" }
             };

            return Page();
        }

        public IActionResult OnPostApprove(int id)
        {
            return RedirectToPage();
        }

        public IActionResult OnPostDeleteInstructor(int id)
        {
            return RedirectToPage();
        }

        public IActionResult OnPostDeleteStudent(int id)
        {
            return RedirectToPage();
        }
    }
}
