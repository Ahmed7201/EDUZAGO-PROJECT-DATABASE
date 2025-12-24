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

        [BindProperty]
        public EDUZAGO_PROJECT_DATABASE.Models.Instructor instructor { get; set; } = new EDUZAGO_PROJECT_DATABASE.Models.Instructor();

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
            var role = HttpContext.Session.GetString("Role");
            var userId = Convert.ToInt32(HttpContext.Session.GetString("UserId"));

            // Check if course exists to decide Add or Update
            Course existing = db.GetCourse(Course.CourseCode);
            bool isUpdate = !string.IsNullOrEmpty(existing.CourseCode);

            if (role == "Instructor")
            {
                // Instructors can only add/edit their own courses
                Course.Instructor_ID = userId;
                instructor.USER_ID = userId;
                Course.Admin_ID = db.GETAdminWhoApproved(instructor);
            }
            else if (role == "Admin")
            {
                // Admins editing: Preserve existing Instructor_ID for updates
                if (isUpdate)
                {
                    Course.Instructor_ID = existing.Instructor_ID;
                    Course.Admin_ID = existing.Admin_ID; // Preserve original admin or set to current admin? Let's preserve.
                }
                else
                {
                    // Admin creating new course? Logic is tricky as Instructor_ID is required.
                    // For now, assume Admin assigns a default instructor or themselves if they are also instructor (unlikely).
                    // This is a business logic gap. We'll set a default generic instructor ID (e.g. 1) if creating new, 
                    // BUT heavily warn user or let them pick (UI doesn't support picking yet).
                    // BETTER: If Admin edits, we just keep existing. If Admin creates, we fallback to 1 (system instructor).
                    Course.Instructor_ID = 1;
                    Course.Admin_ID = userId;
                }
            }

            if (Course.Admin_ID == 0) Course.Admin_ID = 4; // Use a valid Admin ID fallback

            if (isUpdate)
            {
                db.UpdateCourse(Course);
            }
            else
            {
                db.Addcourse(Course);
            }

            if (role == "Admin") return RedirectToPage("/Admin/ManageAllCourses");
            return RedirectToPage("./Dashboard");
        }
    }
}
