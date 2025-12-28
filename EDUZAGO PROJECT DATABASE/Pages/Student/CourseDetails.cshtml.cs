using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using EDUZAGO_PROJECT_DATABASE.Models;
using System.Data;

namespace EDUZAGO_PROJECT_DATABASE.Pages.StudentNamespace
{
    public class CourseDetailsModel : PageModel
    {
        public DB db { get; set; }
        public EDUZAGO_PROJECT_DATABASE.Models.Course Course { get; set; } = new EDUZAGO_PROJECT_DATABASE.Models.Course();
        public List<EDUZAGO_PROJECT_DATABASE.Models.Resource> Resources { get; set; } = new List<EDUZAGO_PROJECT_DATABASE.Models.Resource>();
        public List<EDUZAGO_PROJECT_DATABASE.Models.Schedule> Schedules { get; set; } = new List<EDUZAGO_PROJECT_DATABASE.Models.Schedule>();
        public string MyGrade { get; set; } = "N/A";

        [BindProperty]
        public EDUZAGO_PROJECT_DATABASE.Models.Review NewReview { get; set; } = new EDUZAGO_PROJECT_DATABASE.Models.Review();

        public CourseDetailsModel()
        {
            db = new DB();
        }

        public List<int> CompletedResourceIds { get; set; } = new List<int>();

        public bool HasReviewed { get; set; } = false;

        public IActionResult OnGet(string courseId)
        {
            var role = HttpContext.Session.GetString("Role");
            if (role != "Student") return RedirectToPage("/Account/Login");

            int studentId = int.Parse(HttpContext.Session.GetString("UserId") ?? "0");

            Course = db.GetCourse(courseId);
            // ... existing code ...
            if (string.IsNullOrEmpty(Course.Title)) Course.Title = courseId;
            if (Course.Instructor == null) Course.Instructor = new EDUZAGO_PROJECT_DATABASE.Models.Instructor { Name = "Unknown" };

            // Logic to populate Resources list
            DataTable dtRes = db.GetResources(courseId);
            foreach (DataRow row in dtRes.Rows)
            {
                Resources.Add(new EDUZAGO_PROJECT_DATABASE.Models.Resource
                {
                    ResourceID = Convert.ToInt32(row["Resource_ID"]),
                    ResourceType = row["Resource_Type"].ToString(),
                    URL = row["URL"].ToString(),
                    Course_Code = row["Course_Code"].ToString(),
                    Instructor_ID = Convert.ToInt32(row["Instructor_ID"])
                });
            }

            // Get Completed Resources
            CompletedResourceIds = db.GetCompletedResources(studentId, courseId);

            DataTable dtSch = db.GetSchedule(courseId);
            foreach (DataRow row in dtSch.Rows)
            {
                Schedules.Add(new EDUZAGO_PROJECT_DATABASE.Models.Schedule
                {
                    ScheduleID = Convert.ToInt32(row["Schedule_ID"]),
                    SessionTime = Convert.ToDateTime(row["Session_Time"]),
                    SessionDetails = row["Session_Details"].ToString(),
                    Course_Code = row["Course_Code"].ToString(),
                    Instructor_ID = Convert.ToInt32(row["Instructor_ID"])
                });
            }

            // Fetch Grade
            int gradeId = db.GetGradeId(studentId, courseId);
            if (gradeId > 0)
            {
                DataTable dtGrade = db.GetGradeDetails(gradeId);
                if (dtGrade.Rows.Count > 0)
                {
                    var progressVal = dtGrade.Rows[0]["Progress"];
                    MyGrade = (progressVal != DBNull.Value ? progressVal.ToString() : "0") + "%";
                }
            }

            // Check if already reviewed
            HasReviewed = db.HasStudentReviewed(studentId, courseId);

            return Page();
        }

        public IActionResult OnGetMarkResource(int resourceId)
        {
            var userId = HttpContext.Session.GetString("UserId");
            if (!string.IsNullOrEmpty(userId))
            {
                db.MarkResourceCompleted(int.Parse(userId), resourceId);
            }
            return new JsonResult(new { success = true });
        }

        public IActionResult OnPostReview(string courseId)
        {
            NewReview.Course_Code = courseId;
            var userId = HttpContext.Session.GetString("UserId");
            if (!string.IsNullOrEmpty(userId))
            {
                NewReview.Student_ID = int.Parse(userId);
            }
            else
            {
                return RedirectToPage("/Account/Login");
            }

            // Double Check on Server Side
            if (db.HasStudentReviewed(NewReview.Student_ID, courseId))
            {
                // Optionally add error message or just redirect
                return RedirectToPage(new { courseId = courseId });
            }

            db.AddReview(NewReview);
            return RedirectToPage(new { courseId = courseId });
        }
    }
}
