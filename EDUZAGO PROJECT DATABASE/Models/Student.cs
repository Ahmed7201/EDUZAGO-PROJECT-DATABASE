using System.ComponentModel.DataAnnotations;

namespace EDUZAGO_PROJECT_DATABASE.Models
{
    public class Student : User
    {
        // USER_ID is inherited and acts as StudentID

        [Required]
        [Phone]
        public string PhoneNumber { get; set; } = string.Empty;

        public string Address { get; set; } = string.Empty;
    }
}
