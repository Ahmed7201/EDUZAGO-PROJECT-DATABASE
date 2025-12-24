using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EDUZAGO_PROJECT_DATABASE.Pages.Account
{
    public class InstructorPendingModel : PageModel
    {
        public string Status { get; set; } = "pending";

        public void OnGet(string status)
        {
            if (!string.IsNullOrEmpty(status))
            {
                Status = status;
            }
        }
    }
}
