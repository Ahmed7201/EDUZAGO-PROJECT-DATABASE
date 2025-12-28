using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using EDUZAGO_PROJECT_DATABASE.Models;

namespace EDUZAGO_PROJECT_DATABASE.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly DB db;

        public RegisterModel(DB db)
        {
            this.db = db;
        }

        [BindProperty]
        public InputModel Input { get; set; } = new InputModel();

        public IActionResult OnGet()
        {
            return Page();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (db.RegisterUser(Input.Name, Input.Email, Input.Password, Input.Role))
            {
                // Registration successful
                // Registration successful
                if (Input.Role == "Instructor") return RedirectToPage("./InstructorPending");
                return RedirectToPage("./Login");
            }

            ModelState.AddModelError(string.Empty, "Registration failed. Email might already exist.");
            return Page();
        }

        public class InputModel
        {
            [Required]
            public string Name { get; set; } = "";
            [Required]
            [EmailAddress]
            public string Email { get; set; } = "";
            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; } = "";

            [Required]
            [DataType(DataType.Password)]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; } = "";

            public string Role { get; set; } = "Student";
        }
    }
}
