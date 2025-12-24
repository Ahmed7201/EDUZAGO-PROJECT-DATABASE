using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using EDUZAGO_PROJECT_DATABASE.Models;
using System.Data;

namespace EDUZAGO_PROJECT_DATABASE.Pages.InstructorNamespace
{
    public class ManageGradesModel : PageModel
    {
        public DB db { get; set; }
        public string CourseTitle { get; set; } = "";

        [BindProperty]
        public List<StudentGradeViewModel> StudentGrades { get; set; } = new List<StudentGradeViewModel>();

        public List<ReviewViewModel> Reviews { get; set; } = new List<ReviewViewModel>();

        public ManageGradesModel()
        {
            db = new DB();
        }

        public void OnGet(string courseId)
        {
            CourseTitle = db.GetCourse(courseId).Title;
            if (string.IsNullOrEmpty(CourseTitle)) CourseTitle = courseId;

            // Fetch Grades
            DataTable dt = db.GetStudentsWithGrades(courseId);
            foreach (DataRow row in dt.Rows)
            {
                StudentGrades.Add(new StudentGradeViewModel
                {
                    GradeID = row["Grade_ID"] != DBNull.Value ? Convert.ToInt32(row["Grade_ID"]) : 0,
                    StudentID = Convert.ToInt32(row["Student_ID"]),
                    StudentName = row["Name"].ToString(),
                    Email = row["Email"].ToString(),
                    CompletionStatus = row["Completion_Status"].ToString(),
                    Progress = row["Progress"].ToString()
                });
            }

            // Fetch Reviews
            DataTable dtReviews = db.GetCourseReviews(courseId);
            foreach (DataRow row in dtReviews.Rows)
            {
                Reviews.Add(new ReviewViewModel
                {
                    StudentName = row["StudentName"].ToString(),
                    Rating = Convert.ToInt32(row["Rating"]),
                    ReviewText = row["Review_Text"].ToString()
                });
            }
        }

        public IActionResult OnPost(string courseId)
        {
            foreach (var student in StudentGrades)
            {
                if (student.GradeID > 0)
                {
                    db.UpdateGrade(student.GradeID, student.Progress, student.CompletionStatus);
                }
            }
            return RedirectToPage(new { courseId = courseId });
        }

        public class StudentGradeViewModel
        {
            public int GradeID { get; set; }
            public int StudentID { get; set; }
            public string StudentName { get; set; } = "";
            public string Email { get; set; } = "";
            public string CompletionStatus { get; set; } = "";
            public string Progress { get; set; } = "";
            public string CertificateDetails { get; set; } = "";
        }

        public class ReviewViewModel
        {
            public string StudentName { get; set; }
            public int Rating { get; set; }
            public string ReviewText { get; set; }
        }
    }
}
