using System.ComponentModel.DataAnnotations;

namespace EDUZAGO_PROJECT_DATABASE.Models
{
    public class Instructor : User
    {
        public string Expertise { get; set; } = string.Empty;

        public string Bio { get; set; } = string.Empty;

        // Keeping IsApproved as an inferred requirement for system management, though not explicitly in text schema
        public string IsApproved { get; set; } = "Pending";
    }
}
