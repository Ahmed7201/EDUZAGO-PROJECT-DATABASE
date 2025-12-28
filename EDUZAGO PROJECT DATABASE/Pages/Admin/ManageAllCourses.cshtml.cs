using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using EDUZAGO_PROJECT_DATABASE.Models;
using System.Data;

namespace EDUZAGO_PROJECT_DATABASE.Pages.AdminNamespace
{
    public class ManageCoursesModel : PageModel
    {
        public DB db { get; set; }
        public ManageCoursesModel(DB d)
        {
            db = d;
        }

        public DataTable AllCourses { get; set; } = new DataTable();
        public List<SelectListItem> CategoryOptions { get; set; } = new List<SelectListItem>();

        [BindProperty(SupportsGet = true)]
        public string? SearchTerm { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? CategoryFilter { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? SortOrder { get; set; }

        public IActionResult OnGet()
        {
            var role = HttpContext.Session.GetString("Role");
            if (role != "Admin") return RedirectToPage("/Account/Login");

            // Load Categories for Dropdown - PROACTIVE POPULATION
            DataTable cats = db.GetAllCategories();
            CategoryOptions = new List<SelectListItem>();

            if (cats != null && cats.Rows.Count > 0)
            {
                foreach (DataRow row in cats.Rows)
                {
                    CategoryOptions.Add(new SelectListItem
                    {
                        Value = row["Category_ID"].ToString(),
                        Text = row["Category_Name"].ToString()
                    });
                }
            }

            // Load Courses with Filter
            if (!string.IsNullOrEmpty(SearchTerm) || !string.IsNullOrEmpty(CategoryFilter))
            {
                AllCourses = db.SearchCoursesAdmin(SearchTerm ?? "", CategoryFilter ?? "");
            }
            else
            {
                AllCourses = db.GetAllCourses(); // Now includes StudentCount
            }

            // Apply Sorting
            DataView dv = AllCourses.DefaultView;
            if (!string.IsNullOrEmpty(SortOrder))
            {
                switch (SortOrder)
                {
                    case "price_asc": dv.Sort = "Fees ASC"; break;
                    case "price_desc": dv.Sort = "Fees DESC"; break;
                    case "title_asc": dv.Sort = "Title ASC"; break;
                    case "title_desc": dv.Sort = "Title DESC"; break;
                    default: dv.Sort = "Title ASC"; break;
                }
                AllCourses = dv.ToTable();
            }

            return Page();
        }

        public IActionResult OnPostDelete(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                db.DeleteCourse(id);
            }
            return RedirectToPage();
        }

        public IActionResult OnPostArchive(string id)
        {
            if (!string.IsNullOrEmpty(id)) db.ArchiveCourse(id);
            return RedirectToPage();
        }

        public IActionResult OnPostUnarchive(string id)
        {
            if (!string.IsNullOrEmpty(id)) db.UnarchiveCourse(id);
            return RedirectToPage();
        }
    }
}
