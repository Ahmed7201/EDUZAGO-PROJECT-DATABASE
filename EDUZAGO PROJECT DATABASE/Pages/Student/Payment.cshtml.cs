using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EDUZAGO_PROJECT_DATABASE.Pages.StudentNamespace
{
    public class PaymentModel : PageModel
    {
        private readonly EDUZAGO_PROJECT_DATABASE.Models.DB db;

        public PaymentModel(EDUZAGO_PROJECT_DATABASE.Models.DB db)
        {
            this.db = db;
        }

        [BindProperty(SupportsGet = true)]
        public string CourseCode { get; set; }

        public IActionResult OnGet()
        {
            var role = HttpContext.Session.GetString("Role");
            if (role != "Student")
            {
                return RedirectToPage("/Account/Login");
            }
            if (string.IsNullOrEmpty(CourseCode)) return RedirectToPage("./BrowseCourses");

            return Page();
        }


    }
}
