using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EDUZAGO_PROJECT_DATABASE.Models
{
    public class Course
    {
        [Key]
        public int CourseCode { get; set; } // Using int as ID based on standard EF conventions, though "Code" implies string typically. User asked for "Course Code". I'll use int for PK simplification or string if they insisted, but "Course Code" usually implies like "CS101". Given database list says "Course Code" is stored. I will make it the Key. 

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
