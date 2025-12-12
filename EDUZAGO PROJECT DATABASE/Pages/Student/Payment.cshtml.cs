using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EDUZAGO_PROJECT_DATABASE.Pages.StudentNamespace
{
    public class PaymentModel : PageModel
    {
        public IActionResult OnGet()
        {
            var role = HttpContext.Session.GetString("Role");
            if (role != "Student")
            {
                return RedirectToPage("/Account/Login");
            }

            
            return Page();
        }
    }
}
