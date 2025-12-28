using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using EDUZAGO_PROJECT_DATABASE.Models;
using System.Data;

namespace EDUZAGO_PROJECT_DATABASE.Pages.InstructorNamespace
{
    public class DashboardModel : PageModel
    {
        private readonly DB db;

        public DashboardModel(DB db)
        {
            this.db = db;
        }

        public EDUZAGO_PROJECT_DATABASE.Models.Instructor CurrentInstructor { get; set; } = new EDUZAGO_PROJECT_DATABASE.Models.Instructor();
        public DataTable MyCourses { get; set; } = new DataTable();

        // Simple class for the schedule view
        public class AggregatedScheduleItem
        {
            public string CourseCode { get; set; }
            public string CourseTitle { get; set; }
            public string SessionDetails { get; set; }
            public DateTime SessionTime { get; set; }
        }
        public List<AggregatedScheduleItem> WeeklySchedule { get; set; } = new List<AggregatedScheduleItem>();

        public IActionResult OnGet()
        {
            var role = HttpContext.Session.GetString("Role");
            if (role != "Instructor")
            {
                return RedirectToPage("/Account/Login");
            }

            // Populate from session
            CurrentInstructor = new EDUZAGO_PROJECT_DATABASE.Models.Instructor();
            CurrentInstructor.USER_ID = Convert.ToInt32(HttpContext.Session.GetString("UserId"));
            CurrentInstructor.Name = HttpContext.Session.GetString("UserName") ?? "Instructor";

            // Use the object's ID to fetch courses
            MyCourses = db.GetInstructorCourses(CurrentInstructor);

            // Populate schedule from real courses
            WeeklySchedule = new List<AggregatedScheduleItem>();
            foreach (DataRow row in MyCourses.Rows)
            {
                if (row["Course_Code"] != DBNull.Value)
                {
                    string courseCode = row["Course_Code"].ToString() ?? "";
                    string courseTitle = row["Title"] != DBNull.Value ? row["Title"].ToString() : "Untitled";
                    DataTable schedule = db.GetSchedule(courseCode);
                    foreach (DataRow sRow in schedule.Rows)
                    {
                        WeeklySchedule.Add(new AggregatedScheduleItem
                        {
                            CourseCode = courseCode,
                            CourseTitle = courseTitle,
                            SessionDetails = sRow["Session_Details"]?.ToString(),
                            SessionTime = sRow["Session_Time"] != DBNull.Value ? Convert.ToDateTime(sRow["Session_Time"]) : DateTime.MinValue
                        });
                    }
                }
            }

            // Sort schedule by date
            WeeklySchedule = WeeklySchedule.OrderBy(s => s.SessionTime).ToList();

            // Default Date
            if (TargetDate == default) TargetDate = DateTime.Today;

            return Page();
        }

        [BindProperty(SupportsGet = true)]
        public string ViewMode { get; set; } = "Week";

        [BindProperty(SupportsGet = true)]
        public DateTime TargetDate { get; set; } = DateTime.Today;

        public DateTime ViewStartDate { get; set; }
        public DateTime ViewEndDate { get; set; }

        public void CalculateDateRange()
        {
            if (ViewMode == "Month")
            {
                ViewStartDate = new DateTime(TargetDate.Year, TargetDate.Month, 1);
                ViewEndDate = ViewStartDate.AddMonths(1).AddDays(-1);
            }
            else // Week
            {
                ViewStartDate = TargetDate.AddDays(-(int)TargetDate.DayOfWeek);
                ViewEndDate = ViewStartDate.AddDays(6);
            }
        }

        public IActionResult OnPostDeleteCourse(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                db.DeleteCourse(id);
            }
            return RedirectToPage();
        }

        public IActionResult OnPostArchiveCourse(string id)
        {
            db.ArchiveCourse(id);
            return RedirectToPage();
        }
    }
}
