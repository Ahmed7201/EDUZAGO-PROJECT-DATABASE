using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using EDUZAGO_PROJECT_DATABASE.Models;

namespace EDUZAGO_PROJECT_DATABASE.Pages.InstructorNamespace
{
    public class ManageResourcesModel : PageModel
    {
        public string CourseTitle { get; set; } = "Mock Course";
        public List<MockResource> Resources { get; set; } = new List<MockResource>();

        public void OnGet(int courseId)
        {
            CourseTitle = $"Mock Course {courseId}";
            Resources = new List<MockResource>
            {
                new MockResource { ResourceID = 1, ResourceName = "Week 1 Slides", Link = "slides.pdf" },
                new MockResource { ResourceID = 2, ResourceName = "Assignment 1 Spec", Link = "assign1.pdf" }
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

        public class MockResource
        {
            public int ResourceID { get; set; }
            public string ResourceName { get; set; }
            public string Link { get; set; }
        }
    }
}
