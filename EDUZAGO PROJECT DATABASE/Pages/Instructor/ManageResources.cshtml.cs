using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using EDUZAGO_PROJECT_DATABASE.Models;

namespace EDUZAGO_PROJECT_DATABASE.Pages.InstructorNamespace
{
    public class ManageResourcesModel : PageModel
    {
        public string CourseTitle { get; set; } = "Mock Course";
        public List<EDUZAGO_PROJECT_DATABASE.Models.Resource> Resources { get; set; } = new List<EDUZAGO_PROJECT_DATABASE.Models.Resource>();

        public void OnGet(string courseId)
        {
            CourseTitle = $"Mock Course {courseId}";
            Resources = new List<EDUZAGO_PROJECT_DATABASE.Models.Resource>
            {
                new EDUZAGO_PROJECT_DATABASE.Models.Resource { ResourceID = 1, ResourceType = "Week 1 Slides", URL = "slides.pdf" },
                new EDUZAGO_PROJECT_DATABASE.Models.Resource { ResourceID = 2, ResourceType = "Assignment 1 Spec", URL = "assign1.pdf" }
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
