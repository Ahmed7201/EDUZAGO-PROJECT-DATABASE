using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using EDUZAGO_PROJECT_DATABASE.Models;
using System.Data;
using System.Collections.Generic;

namespace EDUZAGO_PROJECT_DATABASE.Pages.InstructorNamespace
{
    public class ManageCourseModel : PageModel
    {
        private readonly DB db;

        public ManageCourseModel(DB db)
        {
            this.db = db;
        }

        [BindProperty]
        public Course Course { get; set; } = new Course();

        public List<SelectListItem> Categories { get; set; } = new List<SelectListItem>();

        public IActionResult OnGet(string? id)
        {
            var role = HttpContext.Session.GetString("Role");
            if (role != "Instructor" && role != "Admin") return RedirectToPage("/Account/Login");

            // Load Categories for Dropdown from DB
            DataTable catDt = db.GetAllCategories();
            foreach (DataRow row in catDt.Rows)
            {
                Categories.Add(new SelectListItem
                {
                    // Ensure column names match what your DB returns
                    Value = row["Category_ID"].ToString(),
                    Text = row["Category_Name"].ToString()
                });
            }

            if (!string.IsNullOrEmpty(id))
            {
                // Edit Mode: Fetch existing course using DB
                Course = db.GetCourse(id);
            }

            return Page();
        }

        public IActionResult OnPost()
        {
            // Directly assign the Instructor ID from session to the Course object
            Course.Instructor_ID = Convert.ToInt32(HttpContext.Session.GetString("UserId"));

            // Get the Admin who approved this instructor to link them to the course
            EDUZAGO_PROJECT_DATABASE.Models.Instructor tempInst = new EDUZAGO_PROJECT_DATABASE.Models.Instructor();
            tempInst.USER_ID = Course.Instructor_ID;
            Course.Admin_ID = db.GETAdminWhoApproved(tempInst);
            if (Course.Admin_ID == 0) Course.Admin_ID = 1; // Fallback if 0 returned

            // Check if course exists to decide Add or Update
            Course existing = db.GetCourse(Course.CourseCode);
            if (!string.IsNullOrEmpty(existing.CourseCode) && !string.IsNullOrEmpty(existing.Title))
            {
                // Update if found
                db.UpdateCourse(Course);
            }
            else
            {
                // Add if new
                db.Addcourse(Course);
            }

            return RedirectToPage("./Dashboard");
        }
    }
}
