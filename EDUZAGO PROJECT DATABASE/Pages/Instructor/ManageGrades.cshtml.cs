using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EDUZAGO_PROJECT_DATABASE.Pages.InstructorNamespace
{
    public class ManageGradesModel : PageModel
    {
        public string CourseTitle { get; set; } = "Mock Course";

        [BindProperty]
        public List<StudentGradeViewModel> StudentGrades { get; set; } = new List<StudentGradeViewModel>();

        public void OnGet(string courseId)
        {
            CourseTitle = $"Mock Course {courseId}";
            StudentGrades = new List<StudentGradeViewModel>
            {
                new StudentGradeViewModel { StudentID = 1, StudentName = "Alice Student", Email="alice@t.com", CompletionStatus = "Completed", Progress = "100%" },
                new StudentGradeViewModel { StudentID = 2, StudentName = "Bob Learner", Email="bob@t.com", CompletionStatus = "In Progress", Progress = "60%" }
            };
        }

        public IActionResult OnPost()
        {
            // Mock Save
            return RedirectToPage();
        }

        public class StudentGradeViewModel
        {
            public int StudentID { get; set; }
            public string StudentName { get; set; } = "";
            public string Email { get; set; } = "";
            public string CompletionStatus { get; set; } = "In Progress";
            public string Progress { get; set; } = "0%";
            public string CertificateDetails { get; set; } = "";
        }
    }
}
