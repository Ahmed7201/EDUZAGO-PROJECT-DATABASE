using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EDUZAGO_PROJECT_DATABASE.Models
{
    public class Grade
    {
        public int GradeID { get; set; }

        public string CompletionStatus { get; set; } = string.Empty; // e.g., "Completed", "In Progress"

        public string CertificateDetails { get; set; } = string.Empty;

        // Foreign Keys
        public int Student_ID { get; set; }
        [ForeignKey("Student_ID")]
        public Student? Student { get; set; }

        public int Course_Code { get; set; }
        [ForeignKey("Course_Code")]
        public Course? Course { get; set; }

        public int Instructor_ID { get; set; }
        [ForeignKey("Instructor_ID")]
        public Instructor? Instructor { get; set; }
    }
}
