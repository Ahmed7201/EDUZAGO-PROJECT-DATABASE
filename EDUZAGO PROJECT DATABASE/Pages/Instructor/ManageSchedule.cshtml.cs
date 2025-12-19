using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EDUZAGO_PROJECT_DATABASE.Pages.InstructorNamespace
{
    public class ManageScheduleModel : PageModel
    {
        public string CourseTitle { get; set; } = "Mock Course";
        public List<EDUZAGO_PROJECT_DATABASE.Models.Schedule> Schedules { get; set; } = new List<EDUZAGO_PROJECT_DATABASE.Models.Schedule>();

        public void OnGet(string courseId)
        {
            CourseTitle = $"Mock Course {courseId}";
            Schedules = new List<EDUZAGO_PROJECT_DATABASE.Models.Schedule>
            {
                new EDUZAGO_PROJECT_DATABASE.Models.Schedule { ScheduleID = 1, SessionTime = DateTime.Now.AddDays(2).AddHours(14), SessionDetails = "Intro to C#" },
                new EDUZAGO_PROJECT_DATABASE.Models.Schedule { ScheduleID = 2, SessionTime = DateTime.Now.AddDays(5).AddHours(10),  SessionDetails = "OOP Concepts" }
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
    }
}
