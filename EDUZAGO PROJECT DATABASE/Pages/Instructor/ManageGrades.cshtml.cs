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
                int studentId = Convert.ToInt32(row["Student_ID"]);

                int finalGid = 0;
                if (row["Grade_ID"] != DBNull.Value)
                {
                    finalGid = Convert.ToInt32(row["Grade_ID"]);
                }

                // Guarantee Grade Exists (Fixes redirect issue for old/broken data)
                if (finalGid == 0)
                {
                    // EnsureGradeExists now returns the new (or existing) ID directly
                    finalGid = db.EnsureGradeExists(studentId, courseId);
                    // Do not update row["Grade_ID"] since it's read-only
                }


                StudentGrades.Add(new StudentGradeViewModel
                {
                    GradeID = finalGid,
                    StudentID = studentId,
                    StudentName = row["Name"].ToString(),
                    Email = row["Email"].ToString(),
                    CompletionStatus = row["Completion_Status"] != DBNull.Value ? row["Completion_Status"].ToString() : "In Progress",
                    Progress = row["Progress"] != DBNull.Value ? Convert.ToInt32(row["Progress"]) : 0
                });
            }

            // Fetch Reviews
            DataTable dtReviews = db.GetCourseReviews(courseId);
            foreach (DataRow row in dtReviews.Rows)
            {
                Reviews.Add(new ReviewViewModel
                {
                    StudentName = row["StudentName"].ToString(),
                    Rating = row["Rating"] != DBNull.Value ? Convert.ToInt32(row["Rating"]) : 0,
                    ReviewText = row["Review_Text"].ToString()
                });
            }
        }



        public class StudentGradeViewModel
        {
            public int GradeID { get; set; }
            public int StudentID { get; set; }
            public string StudentName { get; set; } = "";
            public string Email { get; set; } = "";
            public string CompletionStatus { get; set; } = "";
            public int Progress { get; set; }
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
