using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using EDUZAGO_PROJECT_DATABASE.Models;

namespace EDUZAGO_PROJECT_DATABASE.Pages.AdminNamespace
{
    public class ManageCategoriesModel : PageModel
    {
        public ManageCategoriesModel()
        {
        }

        public List<Category> Categories { get; set; } = new List<Category>();

        [BindProperty]
        public Category NewCategory { get; set; } = new Category();

        public IActionResult OnGet()
        {
            var role = HttpContext.Session.GetString("Role");
            if (role != "Admin") return RedirectToPage("/Account/Login");

            Categories = new List<Category>
             {
                 new Category { CategoryID = 1, CategoryName = "Mock Category 1", Description = "Desc 1" },
                 new Category { CategoryID = 2, CategoryName = "Mock Category 2", Description = "Desc 2" }
             };
            return Page();
        }

        public IActionResult OnPostAdd()
        {
            return RedirectToPage();
        }

        public IActionResult OnPostDelete(int id)
        {
            return RedirectToPage();
        }
    }
}
