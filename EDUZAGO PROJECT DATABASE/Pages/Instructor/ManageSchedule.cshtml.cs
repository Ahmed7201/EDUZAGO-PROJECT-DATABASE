using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using EDUZAGO_PROJECT_DATABASE.Models;
using System.Data;

namespace EDUZAGO_PROJECT_DATABASE.Pages.InstructorNamespace
{
    public class ManageScheduleModel : PageModel
    {
        public DB db { get; set; }
        public string CourseTitle { get; set; } = "";
        public List<EDUZAGO_PROJECT_DATABASE.Models.Schedule> Schedules { get; set; } = new List<EDUZAGO_PROJECT_DATABASE.Models.Schedule>();

        [BindProperty]
        public EDUZAGO_PROJECT_DATABASE.Models.Schedule NewSchedule { get; set; } = new EDUZAGO_PROJECT_DATABASE.Models.Schedule();

        public ManageScheduleModel()
        {
            db = new DB();
        }

        public void OnGet(string courseId)
        {
            CourseTitle = db.GetCourse(courseId).Title;
            if(string.IsNullOrEmpty(CourseTitle)) CourseTitle = courseId;
            DataTable dt = db.GetSchedule(courseId);
            foreach (DataRow row in dt.Rows)
            {
                Schedules.Add(new EDUZAGO_PROJECT_DATABASE.Models.Schedule
                {
                    ScheduleID = Convert.ToInt32(row["ScheduleID"]),
                    SessionTime = Convert.ToDateTime(row["SessionTime"]),
                    SessionDetails = row["SessionDetails"].ToString(),
                    Course_Code = row["Course_Code"].ToString(),
                    Instructor_ID = Convert.ToInt32(row["Instructor_ID"])
                });
            }
        }

        public IActionResult OnPostAdd(string courseId)
        {
            NewSchedule.Course_Code = courseId;
            NewSchedule.Instructor_ID = 1;

            db.AddSchedule(NewSchedule);
            return RedirectToPage(new { courseId = courseId });
        }
    }
}
