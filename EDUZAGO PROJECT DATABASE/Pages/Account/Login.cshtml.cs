using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace EDUZAGO_PROJECT_DATABASE.Pages.Account
{
    public class LoginModel : PageModel
    {
        public LoginModel()
        {
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }
        }

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // MOCK AUTHENTICATION LOGIC (GUI ONLY)
            // Convention:
            // "s-" prefix -> Student
            // "i-" prefix -> Instructor
            // "a-" prefix -> Admin

            string role = "";
            string redirectPage = "";
            string userName = "User";

            if (Input.Email.ToLower().StartsWith("s-"))
            {
                role = "Student";
                userName = "Student User";
                redirectPage = "/Student/Dashboard";
            }
            else if (Input.Email.ToLower().StartsWith("i-"))
            {
                role = "Instructor";
                userName = "Instructor User";
                redirectPage = "/Instructor/Dashboard";
            }
            else if (Input.Email.ToLower().StartsWith("a-"))
            {
                role = "Admin";
                userName = "Admin User";
                redirectPage = "/Admin/Dashboard";
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Invalid mock prefix. Use 's-', 'i-', or 'a-' at start of email.");
                return Page();
            }

            // Set Session
            HttpContext.Session.SetString("Role", role);
            HttpContext.Session.SetString("UserId", "1"); // Mock ID
            HttpContext.Session.SetString("UserName", userName);

            return RedirectToPage(redirectPage);
        }
    }
}
