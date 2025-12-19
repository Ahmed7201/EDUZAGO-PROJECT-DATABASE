using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EDUZAGO_PROJECT_DATABASE.Models
{
    public class Resource
    {
        public int ResourceID { get; set; }

        [Required]
        public string ResourceType { get; set; } = string.Empty;

        [Required]
        public string URL { get; set; } = string.Empty;

        // Foreign Keys
        public string Course_Code { get; set; } = string.Empty;
        [ForeignKey("Course_Code")]
        public Course? Course { get; set; }

        public int Instructor_ID { get; set; }
        [ForeignKey("Instructor_ID")]
        public Instructor? Instructor { get; set; }
    }
}
