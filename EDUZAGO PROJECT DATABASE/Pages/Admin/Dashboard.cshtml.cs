using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using EDUZAGO_PROJECT_DATABASE.Models;
using System.Data;
using static EDUZAGO_PROJECT_DATABASE.Models.DB;
namespace EDUZAGO_PROJECT_DATABASE.Pages.AdminNamespace
{
    public class DashboardModel : PageModel
    {
        public DB db { get; set; }
        public DashboardModel(DB d)
        {
            db = d;
        }

        public int StudentCount { get; set; }
        public int InstructorCount { get; set; }
        public int CourseCount { get; set; }
        public int PendingInstructors { get; set; }

        public decimal RevenueGrowth { get; set; } = 0;

        public DB.DashboardCounts Counts { get; set; } = new DB.DashboardCounts();
        public List<DB.WeeklyRevenue> RevenueData { get; set; } = new List<DB.WeeklyRevenue>();
        public List<DB.CoursePopularity> PopularityData { get; set; } = new List<DB.CoursePopularity>();
        public List<DB.CategoryIncome> IncomeData { get; set; } = new List<DB.CategoryIncome>();
        public List<DB.CourseIncome> CourseIncomeData { get; set; } = new List<DB.CourseIncome>();
        public List<DB.CourseEnrollmentStats> StudentStatsData { get; set; } = new List<DB.CourseEnrollmentStats>();

        public void OnGet()
        {
            var role = HttpContext.Session.GetString("Role");
            if (role != "Admin")
            {
                Response.Redirect("/Account/Login");
                return;
            }

            // Stats
            StudentCount = db.Get_StudentCount();
            InstructorCount = db.Get_InstructorCount();
            CourseCount = db.Get_CourseCount();
            PendingInstructors = db.Get_Pendingcount();

            // Analytics - Direct Assignment from DB methods
            Counts = db.GetDashboardCounts();

            // Income Data
            IncomeData = db.GetCategoryIncome();
            CourseIncomeData = db.GetCourseIncome();
            StudentStatsData = db.GetCourseEnrollmentStats();

            // Revenue Data (Already returned Chronologically: Oldest -> Newest)
            RevenueData = db.GetWeeklyRevenue();

            // Calculate Growth (Compare last 2 entries if available)
            // RevenueData is Oldest -> Newest, so [Count-1] is current/newest.
            if (RevenueData.Count >= 2)
            {
                decimal current = RevenueData[RevenueData.Count - 1].Revenue;
                decimal previous = RevenueData[RevenueData.Count - 2].Revenue;
                if (previous > 0)
                {
                    RevenueGrowth = ((current - previous) / previous) * 100;
                }
                else if (current > 0)
                {
                    RevenueGrowth = 100;
                }
            }

            // Popularity Data
            PopularityData = db.GetCoursePopularity();
        }
    }
}
