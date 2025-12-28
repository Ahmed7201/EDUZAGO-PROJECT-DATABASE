using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using EDUZAGO_PROJECT_DATABASE.Models;
using System.Data;

namespace EDUZAGO_PROJECT_DATABASE.Pages.InstructorNamespace
{
    public class EditGradeModel : PageModel
    {
        private readonly DB db;

        public EditGradeModel(DB db)
        {
            this.db = db;
        }

        [BindProperty]
        public int GradeID { get; set; }

        public string StudentName { get; set; } = "";
        public string Email { get; set; } = "";
        public string CourseTitle { get; set; } = "";
        public string CourseCode { get; set; } = "";

        [BindProperty]
        public string CompletionStatus { get; set; } = "";

        [BindProperty]
        public int Progress { get; set; }

        public IActionResult OnGet(int gradeId)
        {
            var role = HttpContext.Session.GetString("Role");
            if (role != "Instructor") return RedirectToPage("/Account/Login");

            if (gradeId == 0) return RedirectToPage("./Dashboard");

            DataTable dt = db.GetGradeDetails(gradeId);
            if (dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];
                GradeID = gradeId;
                StudentName = row["Name"].ToString();
                Email = row["Email"].ToString();
                CourseTitle = row["CourseTitle"].ToString();
                CourseCode = row["Course_Code"].ToString();
                CompletionStatus = row["Completion_Status"].ToString();
                Progress = row["Progress"] != DBNull.Value ? Convert.ToInt32(row["Progress"]) : 0;
                return Page();
            }

            return RedirectToPage("./Dashboard");
        }

        public IActionResult OnPost()
        {
            if (GradeID > 0)
            {
                db.UpdateGrade(GradeID, Progress, CompletionStatus);

                // Get course code to redirect back correctly try to fetch it again or store in hidden field
                // For simplicity, we fetch it again briefly or pass it via hidden input
                DataTable dt = db.GetGradeDetails(GradeID);
                if (dt.Rows.Count > 0)
                {
                    string cc = dt.Rows[0]["Course_Code"].ToString();
                    return RedirectToPage("./ManageGrades", new { courseId = cc });
                }
            }
            return RedirectToPage("./Dashboard");
        }
    }
}
