using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using EDUZAGO_PROJECT_DATABASE.Models;
using System.Data;

namespace EDUZAGO_PROJECT_DATABASE.Pages.InstructorNamespace
{
    public class ViewStudentsModel : PageModel
    {
        private readonly DB db;

        public ViewStudentsModel(DB db)
        {
            this.db = db;
        }

        public Course CourseObj { get; set; } = new Course();
        public DataTable StudentsTable { get; set; } = new DataTable();

        public IActionResult OnGet(string courseId)
        {
            var role = HttpContext.Session.GetString("Role");
            if (role != "Instructor") return RedirectToPage("/Account/Login");

            // Use DB to get info
            CourseObj = db.GetCourse(courseId); // Populates basic course info like Title
            StudentsTable = db.GetStudentsForCourse(courseId); // Returns DataTable of students

            return Page();
        }
    }
}
