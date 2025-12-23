using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EDUZAGO_PROJECT_DATABASE.Models
{
    public class Instructor : User
    {
        public string Expertise { get; set; } = string.Empty;

        public string Bio { get; set; } = string.Empty;

        // Keeping IsApproved as an inferred requirement for system management, though not explicitly in text schema
        public string Approval_Status { get; set; } = "pending";

        public int Admin_ID { get; set; }
        [ForeignKey("Admin_ID")]
        public Admin? Admin { get; set; }
    }
}
