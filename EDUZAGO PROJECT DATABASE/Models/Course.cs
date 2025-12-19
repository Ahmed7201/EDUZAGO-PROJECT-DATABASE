using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EDUZAGO_PROJECT_DATABASE.Models
{
    public class Course
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)] // String keys are usually not auto-generated
        public string CourseCode { get; set; } = string.Empty;

        [Required]
        public string Title { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public string Duration { get; set; } = string.Empty;

        public decimal Fees { get; set; }

        // Foreign Keys
        public int Instructor_ID { get; set; }
        [ForeignKey("Instructor_ID")]
        public Instructor? Instructor { get; set; }

        public int Category_ID { get; set; }
        [ForeignKey("Category_ID")]
        public Category? Category { get; set; }

        public int Admin_ID { get; set; }
        [ForeignKey("Admin_ID")]
        public Admin? Admin { get; set; }
    }
}
