using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using EDUZAGO_PROJECT_DATABASE.Models;
using System.Data;

namespace EDUZAGO_PROJECT_DATABASE.Pages.AdminNamespace
{
    public class ManageUsersModel : PageModel
    {
        [BindProperty]
        public EDUZAGO_PROJECT_DATABASE.Models.Instructor instructor {  get; set; }

        [BindProperty]
        public string errror_msg { get; set; }
        public DB db { get; set; }

        public DataTable instructor_table { get; set; } = new DataTable();
        public DataTable student_table { get; set; } = new DataTable();
        
        public ManageUsersModel(DB d)
        {
            db = d;
        }


        public IActionResult OnGet()
        {
            var role = HttpContext.Session.GetString("Role");
            if (role != "Admin") return RedirectToPage("/Account/Login");
            instructor_table = db.GetAllInstructors();
            student_table = db.GetAllStudents();


            return Page();
        }

        public IActionResult OnPostApprove()
        {
            if(db.Approve_Instructor(instructor)==1)
            {
                return Page();
            }
            else
            {
                errror_msg = "The approve operation was not succesfull";
                return Page();
            }

               
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
