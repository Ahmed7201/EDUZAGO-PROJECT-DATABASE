using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using EDUZAGO_PROJECT_DATABASE.Models;
using System.Data;

namespace EDUZAGO_PROJECT_DATABASE.Pages.InstructorNamespace
{
    public class ManageResourcesModel : PageModel
    {
        public DB db { get; set; }
        public string CourseTitle { get; set; } = "";
        public List<EDUZAGO_PROJECT_DATABASE.Models.Resource> Resources { get; set; } = new List<EDUZAGO_PROJECT_DATABASE.Models.Resource>();

        [BindProperty]
        public EDUZAGO_PROJECT_DATABASE.Models.Resource NewResource { get; set; } = new EDUZAGO_PROJECT_DATABASE.Models.Resource();

        public ManageResourcesModel()
        {
            db = new DB();
        }

        public void OnGet(string courseId)
        {
            CourseTitle = db.GetCourse(courseId).Title;
            if(string.IsNullOrEmpty(CourseTitle)) CourseTitle = courseId;
            DataTable dt = db.GetResources(courseId);
            foreach (DataRow row in dt.Rows)
            {
                Resources.Add(new EDUZAGO_PROJECT_DATABASE.Models.Resource
                {
                    ResourceID = Convert.ToInt32(row["ResourceID"]),
                    ResourceType = row["ResourceType"].ToString(),
                    URL = row["URL"].ToString(),
                    Course_Code = row["Course_Code"].ToString(),
                    Instructor_ID = Convert.ToInt32(row["Instructor_ID"])
                });
            }
        }

        public IActionResult OnPostAdd(string courseId)
        {
            NewResource.Course_Code = courseId;
            NewResource.Instructor_ID = 1; 

            db.AddResource(NewResource);
            return RedirectToPage(new { courseId = courseId });
        }
    }
}
