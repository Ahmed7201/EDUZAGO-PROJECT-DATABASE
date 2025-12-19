using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using EDUZAGO_PROJECT_DATABASE.Models;

namespace EDUZAGO_PROJECT_DATABASE.Pages.StudentNamespace
{
    public class CourseDetailsModel : PageModel
    {
        public EDUZAGO_PROJECT_DATABASE.Models.Course Course { get; set; } = new EDUZAGO_PROJECT_DATABASE.Models.Course();
        public List<EDUZAGO_PROJECT_DATABASE.Models.Resource> Resources { get; set; } = new List<EDUZAGO_PROJECT_DATABASE.Models.Resource>();
        public List<EDUZAGO_PROJECT_DATABASE.Models.Schedule> Schedules { get; set; } = new List<EDUZAGO_PROJECT_DATABASE.Models.Schedule>();
        public string MyGrade { get; set; } = "A-";

        public void OnGet(string courseId)
        {
            // Mock Data
            Course = new EDUZAGO_PROJECT_DATABASE.Models.Course
            {
                CourseCode = courseId,
                Title = "Mock Course " + courseId,
                Description = "Deep dive into the subject.",
                Instructor = new EDUZAGO_PROJECT_DATABASE.Models.Instructor { Name = "Dr. Mock" }
            };

            Resources = new List<EDUZAGO_PROJECT_DATABASE.Models.Resource>
             {
                 new EDUZAGO_PROJECT_DATABASE.Models.Resource { ResourceType = "PDF Notes", URL = "#" },
                 new EDUZAGO_PROJECT_DATABASE.Models.Resource { ResourceType = "Video Link", URL = "#" }
             };

            Schedules = new List<EDUZAGO_PROJECT_DATABASE.Models.Schedule>
             {
                 new EDUZAGO_PROJECT_DATABASE.Models.Schedule { SessionTime = DateTime.Now.AddDays(1), SessionDetails = "Introduction" },
                 new EDUZAGO_PROJECT_DATABASE.Models.Schedule { SessionTime = DateTime.Now.AddDays(3), SessionDetails = "Advanced Topics" }
             };
        }

        public IActionResult OnPostReview()
        {
            // Mock Save Review
            return RedirectToPage(new { courseId = Course.CourseCode });
        }
    }
}
