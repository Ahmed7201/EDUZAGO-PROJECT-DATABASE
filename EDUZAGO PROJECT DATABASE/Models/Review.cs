using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EDUZAGO_PROJECT_DATABASE.Models
{
    public class Review
    {
        public int ReviewID { get; set; }

        [Required]
        public string Content { get; set; } = string.Empty;

        [Range(1, 5)]
        public int Rating { get; set; }

        // Foreign Keys
        public int Course_Code { get; set; }
        [ForeignKey("Course_Code")]
        public Course? Course { get; set; }

        public int Student_ID { get; set; }
        [ForeignKey("Student_ID")]
        public Student? Student { get; set; }
    }
}
