using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EDUZAGO_PROJECT_DATABASE.Models
{
    public class Review
    {
        public int ReviewID { get; set; }

        [Required]
        public string Content { get; set; } = string.Empty; // Maps to "Reviews" column

        [Range(1, 5)]
        public int Rating { get; set; }

        // Foreign Keys
        public string Course_Code { get; set; } = string.Empty;
        [ForeignKey("Course_Code")]
        public Course? Course { get; set; }

        public int Student_ID { get; set; }
        [ForeignKey("Student_ID")]
        public Student? Student { get; set; }
    }
}
