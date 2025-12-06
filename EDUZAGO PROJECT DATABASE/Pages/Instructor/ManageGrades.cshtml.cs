using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EDUZAGO_PROJECT_DATABASE.Pages.InstructorNamespace
{
    public class ManageGradesModel : PageModel
    {
        public string CourseTitle { get; set; } = "Mock Course";

        [BindProperty]
        public List<MockStudentGrade> StudentGrades { get; set; } = new List<MockStudentGrade>();

        public void OnGet(int courseId)
        {
            CourseTitle = $"Mock Course {courseId}";
            StudentGrades = new List<MockStudentGrade>
            {
                new MockStudentGrade { StudentID = 1, StudentName = "Alice Student", Email="alice@t.com", Grade = "A" },
                new MockStudentGrade { StudentID = 2, StudentName = "Bob Learner", Email="bob@t.com", Grade = "B+" }
            };
        }

        public IActionResult OnPost()
        {
            // Mock Save
            return RedirectToPage();
        }

        public class MockStudentGrade
        {
            public int StudentID { get; set; }
            public string StudentName { get; set; } = "";
            public string Email { get; set; } = "";
            public string Grade { get; set; } = "";
        }
    }
}
