using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using EDUZAGO_PROJECT_DATABASE.Models;
using System.Data;

namespace EDUZAGO_PROJECT_DATABASE.Pages.StudentNamespace
{
    public class CoursePreviewModel : PageModel
    {
        private readonly DB db;

        public CoursePreviewModel(DB db)
        {
            this.db = db;
        }

        public Course Course { get; set; }
        public DataTable Reviews { get; set; } = new DataTable();
        public bool IsEnrolled { get; set; } = false;

        public IActionResult OnGet(string courseId)
        {
            if (string.IsNullOrEmpty(courseId)) return RedirectToPage("./BrowseCourses");

            var role = HttpContext.Session.GetString("Role");
            if (role != "Student") return RedirectToPage("/Account/Login");

            int studentId = int.Parse(HttpContext.Session.GetString("UserId"));

            // Get Course Details
            Course = db.GetCourse(courseId);
            if (Course == null || string.IsNullOrEmpty(Course.CourseCode))
            {
                return RedirectToPage("./BrowseCourses");
            }

            // Get Reviews
            // Get Reviews
            Reviews = db.GetCourseReviews(courseId);

            // Check if already enrolled
            IsEnrolled = db.IsEnrolled(studentId, courseId);

            return Page();
        }

        public IActionResult OnPostEnroll(string courseCode)
        {
            if (string.IsNullOrEmpty(courseCode)) return Page();

            var studentIdStr = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(studentIdStr)) return RedirectToPage("/Account/Login");

            try
            {
                int studentId = int.Parse(studentIdStr);
                db.EnrollStudent(studentId, courseCode);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Enrollment Error: " + ex.Message);
            }

            return RedirectToPage("./Payment", new { courseCode = courseCode });
        }
    }
}
