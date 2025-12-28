using EDUZAGO_PROJECT_DATABASE.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EDUZAGO_PROJECT_DATABASE.Pages.Admin
{
    public class ProfileModel : PageModel
    {
        [BindProperty]
        public User Admin { get; set; } = new User();

        [BindProperty]
        public string? OldPassword { get; set; }

        [BindProperty]
        public string? NewPassword { get; set; }

        public string? Message { get; set; }

        public void OnGet()
        {
            DB db = new DB();
            if (HttpContext.Session.GetString("Role") != "Admin")
            {
                Response.Redirect("/Account/Login");
                return;
            }

            int? uid = HttpContext.Session.GetInt32("UserId");
            if (uid.HasValue)
            {
                Admin = db.GetAdminProfile(uid.Value);
            }
        }

        public void OnPost()
        {
            DB db = new DB();
            int? uid = HttpContext.Session.GetInt32("UserId");
            if (uid.HasValue)
            {
                Admin.USER_ID = uid.Value;
                db.UpdateAdminProfile(Admin);
                Message = "Profile updated successfully!";
                Admin = db.GetAdminProfile(uid.Value);
            }
        }

        public void OnPostChangePassword()
        {
            DB db = new DB();
            int? uid = HttpContext.Session.GetInt32("UserId");

            if (uid.HasValue)
            {
                Admin = db.GetAdminProfile(uid.Value);

                if (string.IsNullOrEmpty(OldPassword) || string.IsNullOrEmpty(NewPassword))
                {
                    Message = "Please enter both old and new passwords.";
                    return;
                }

                try
                {
                    db.UpdatePassword(uid.Value, OldPassword, NewPassword);
                    Message = "Password updated successfully!";
                }
                catch (Exception ex)
                {
                    Message = ex.Message;
                }
            }
        }
    }
}
