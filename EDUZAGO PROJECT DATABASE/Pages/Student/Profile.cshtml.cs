using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using EDUZAGO_PROJECT_DATABASE.Models;

namespace EDUZAGO_PROJECT_DATABASE.Pages.StudentNamespace
{
    public class ProfileModel : PageModel
    {
        [BindProperty]
        public EDUZAGO_PROJECT_DATABASE.Models.Student MockStudent { get; set; } = new EDUZAGO_PROJECT_DATABASE.Models.Student();

        public IActionResult OnGet()
        {
            var role = HttpContext.Session.GetString("Role");
            if (role != "Student") return RedirectToPage("/Account/Login");

            // Mock Data
            MockStudent = new EDUZAGO_PROJECT_DATABASE.Models.Student
            {
                Name = "Mock Student",
                Email = "s-user@eduzago.com",
                PhoneNumber = "123-456-7890",
                Address = "123 University Ave"
            };

            return Page();
        }

        public IActionResult OnPost()
        {
            // Mock Save
            return RedirectToPage("./Dashboard");
        }
    }
}
