using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using EDUZAGO_PROJECT_DATABASE.Models;
using Microsoft.Data.SqlClient;

namespace EDUZAGO_PROJECT_DATABASE.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly DB db;

        public LoginModel(DB db)
        {
            this.db = db;
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

            var user = db.Login(Input.Email, Input.Password);
            if (user != null)
            {
                // Login successful
                HttpContext.Session.SetString("UserId", user.USER_ID.ToString());
                HttpContext.Session.SetString("UserName", user.Name);
                HttpContext.Session.SetString("Role", user.Role);

                if (user.Role == "Admin") return RedirectToPage("/Admin/Dashboard");
                if (user.Role == "Instructor") return RedirectToPage("/Instructor/Dashboard");
                if (user.Role == "Student") return RedirectToPage("/Student/Dashboard");

                return RedirectToPage("/Index");
            }

            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            return Page();
        }
    }
}
