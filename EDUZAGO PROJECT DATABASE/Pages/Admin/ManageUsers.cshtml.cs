using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using EDUZAGO_PROJECT_DATABASE.Models;
using System.Data;

namespace EDUZAGO_PROJECT_DATABASE.Pages.AdminNamespace
{
    public class ManageUsersModel : PageModel
    {
        [BindProperty]
        public EDUZAGO_PROJECT_DATABASE.Models.Instructor instructor { get; set; }

        [BindProperty]
        public string errror_msg { get; set; }
        public DB db { get; set; }

        public DataTable instructor_table { get; set; } = new DataTable();
        public DataTable student_table { get; set; } = new DataTable();

        public ManageUsersModel(DB d)
        {
            db = d;
        }


        [BindProperty(SupportsGet = true)]
        public string InstructorStatus { get; set; } = "All";

        [BindProperty(SupportsGet = true)]
        public string StudentSearch { get; set; }

        [BindProperty(SupportsGet = true)]
        public string InstructorSearch { get; set; }

        public IActionResult OnGet()
        {
            var role = HttpContext.Session.GetString("Role");
            if (role != "Admin") return RedirectToPage("/Account/Login");

            // Filter & Search Instructors
            instructor_table = db.SearchInstructors(InstructorSearch, InstructorStatus);

            // Search Students
            student_table = db.SearchStudents(StudentSearch);

            return Page();
        }



        public IActionResult OnPostApprove(int id)
        {
            EDUZAGO_PROJECT_DATABASE.Models.Instructor i = new EDUZAGO_PROJECT_DATABASE.Models.Instructor();
            i.USER_ID = id;
            // Approve
            db.Approve_Instructor(i);
            return RedirectToPage();
        }

        public IActionResult OnPostReject(int id)
        {
            EDUZAGO_PROJECT_DATABASE.Models.Instructor i = new EDUZAGO_PROJECT_DATABASE.Models.Instructor();
            i.USER_ID = id;
            // Reject
            db.Donot_Approve_Instructor(i);
            return RedirectToPage();
        }

        public IActionResult OnPostDeleteInstructor(int id)
        {
            EDUZAGO_PROJECT_DATABASE.Models.Instructor i = new EDUZAGO_PROJECT_DATABASE.Models.Instructor();
            i.USER_ID = id;
            db.DeleteInstructor(i);
            return RedirectToPage();
        }

        public IActionResult OnPostDeleteStudent(int id)
        {
            EDUZAGO_PROJECT_DATABASE.Models.Student s = new EDUZAGO_PROJECT_DATABASE.Models.Student();
            s.USER_ID = id;
            db.DeleteStudent(s);
            return RedirectToPage();
        }
    }
}
