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

        public void OnGet(string courseId)
        {
            Course = db.GetCourse(courseId);
            if (string.IsNullOrEmpty(Course.Title)) Course.Title = courseId;
            if (string.IsNullOrEmpty(Course.Title)) Course.Title = courseId;
            // Removed TBD fallback - Instructor Name is now fetched from DB
            if (Course.Instructor == null) Course.Instructor = new EDUZAGO_PROJECT_DATABASE.Models.Instructor { Name = "Unknown" };

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

            db.AddReview(NewReview);
            return RedirectToPage(new { courseId = courseId });
        }
    }
}
