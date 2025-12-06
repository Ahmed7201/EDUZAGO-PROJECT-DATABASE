using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EDUZAGO_PROJECT_DATABASE.Pages.InstructorNamespace
{
    public class ManageScheduleModel : PageModel
    {
        public string CourseTitle { get; set; } = "Mock Course";
        public List<MockScheduleItem> Schedules { get; set; } = new List<MockScheduleItem>();

        public void OnGet(int courseId)
        {
            CourseTitle = $"Mock Course {courseId}";
            Schedules = new List<MockScheduleItem>
            {
                new MockScheduleItem { ScheduleID = 1, Day = "Monday", StartTime = "10:00", EndTime = "12:00", Topic = "Intro to C#" },
                new MockScheduleItem { ScheduleID = 2, Day = "Wednesday", StartTime = "14:00", EndTime = "16:00", Topic = "OOP Concepts" }
            };
        }

        public IActionResult OnPostAdd()
        {
            // Mock Add
            return RedirectToPage();
        }

        public IActionResult OnPostDelete(int id)
        {
            // Mock Delete
            return RedirectToPage();
        }

        public class MockScheduleItem
        {
            public int ScheduleID { get; set; }
            public string Day { get; set; } = "";
            public string StartTime { get; set; } = "";
            public string EndTime { get; set; } = "";
            public string Topic { get; set; } = "";
        }
    }
}
