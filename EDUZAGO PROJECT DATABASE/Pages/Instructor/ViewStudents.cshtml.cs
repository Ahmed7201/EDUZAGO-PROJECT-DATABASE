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

        public string CourseId { get; set; }
        public DataTable Students { get; set; } = new DataTable();

        public IActionResult OnGet(string courseId)
        {
            if (string.IsNullOrEmpty(courseId))
            {
                return RedirectToPage("./Dashboard");
            }

            CourseId = courseId;
            Students = db.GetStudentsForCourse(courseId);

            return Page();
        }
    }
}
