using System.ComponentModel.DataAnnotations;

namespace EDUZAGO_PROJECT_DATABASE.Models
{
    public class Instructor
    {
        public int InstructorID { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        public string Expertise { get; set; } = string.Empty;

        public string Bio { get; set; } = string.Empty;

        public bool IsApproved { get; set; } = false;
    }
}
