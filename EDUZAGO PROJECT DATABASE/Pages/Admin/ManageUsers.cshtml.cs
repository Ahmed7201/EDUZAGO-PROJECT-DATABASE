using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using EDUZAGO_PROJECT_DATABASE.Models;
using System.Data;

namespace EDUZAGO_PROJECT_DATABASE.Pages.AdminNamespace
{
    public class ManageUsersModel : PageModel
    {
        public DB db { get; set; }

        public DataTable instructor_table { get; set; }
        public DataTable student_table { get; set; }
        public ManageUsersModel(DB d)
        {
            db = d;
        }

        
        public IActionResult OnGet()
        {
            var role = HttpContext.Session.GetString("Role");
            if (role != "Admin") return RedirectToPage("/Account/Login");
            instructor_table=db.GetAllInstructors();
            student_table=db.GetAllStudents();
            

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
