using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using EDUZAGO_PROJECT_DATABASE.Models;

namespace EDUZAGO_PROJECT_DATABASE.Pages.StudentNamespace
{
    public class CourseDetailsModel : PageModel
    {
        public EDUZAGO_PROJECT_DATABASE.Models.Course Course { get; set; } = new EDUZAGO_PROJECT_DATABASE.Models.Course();
        public List<MockResource> Resources { get; set; } = new List<MockResource>();
        public List<MockSchedule> Schedules { get; set; } = new List<MockSchedule>();
        public string MyGrade { get; set; } = "A-";

        public void OnGet(int courseId)
        {
            // Mock Data
            Course = new EDUZAGO_PROJECT_DATABASE.Models.Course
            {
                CourseCode = courseId,
                Title = "Mock Course " + courseId,
                Description = "Deep dive into the subject.",
                Instructor = new EDUZAGO_PROJECT_DATABASE.Models.Instructor { Name = "Dr. Mock" }
            };

            Resources = new List<MockResource>
             {
                 new MockResource { ResourceName = "Lecture 1 Notes", Link = "#" },
                 new MockResource { ResourceName = "Lab Helper", Link = "#" }
             };

            Schedules = new List<MockSchedule>
             {
                 new MockSchedule { Day = "Monday", StartTime = "10:00 AM", EndTime = "12:00 PM", Topic = "Introduction" },
                 new MockSchedule { Day = "Wednesday", StartTime = "10:00 AM", EndTime = "12:00 PM", Topic = "Advanced Topics" }
             };
        }

        public IActionResult OnPostReview()
        {
            // Mock Save Review
            return RedirectToPage(new { courseId = Course.CourseCode });
        }

        public class MockResource { public string ResourceName { get; set; } public string Link { get; set; } }
        public class MockSchedule { public string Day { get; set; } public string StartTime { get; set; } public string EndTime { get; set; } public string Topic { get; set; } }
    }
}
