using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using EDUZAGO_PROJECT_DATABASE.Models;
using System.Data;

namespace EDUZAGO_PROJECT_DATABASE.Pages.AdminNamespace
{
    public class ManageCategoriesModel : PageModel
    {
        public DB db { get; set; }
        public ManageCategoriesModel(DB d)
        {
            db = d;
        }

        public DataTable Category_table { get; set; } = new DataTable();

        [BindProperty]
        public Category NewCategory { get; set; } = new Category();

        public IActionResult OnGet()
        {
            var role = HttpContext.Session.GetString("Role");
            if (role != "Admin") return RedirectToPage("/Account/Login");

            Category_table = db.GetAllCategories();
           
            return Page();
        }

        public IActionResult OnPostAdd()
        {
            db.AddCategory(NewCategory);
            return Page();
        }

        public IActionResult OnPostDelete(int id)
        {
            return RedirectToPage();
        }
    }
}
