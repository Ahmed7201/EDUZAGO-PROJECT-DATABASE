using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EDUZAGO_PROJECT_DATABASE.Models
{
    public class Grade
    {
        public int GradeID { get; set; }

        public string CompletionStatus { get; set; } = string.Empty;

        public string CertificateDetails { get; set; } = string.Empty; // Maps to Certificate

        public string Progress { get; set; } = string.Empty; // Added based on schema

        // Foreign Keys
        public int Student_ID { get; set; }
        [ForeignKey("Student_ID")]
        public Student? Student { get; set; }

        public string Course_Code { get; set; } = string.Empty;
        [ForeignKey("Course_Code")]
        public Course? Course { get; set; }

        public int Instructor_ID { get; set; }
        [ForeignKey("Instructor_ID")]
        public Instructor? Instructor { get; set; }
    }
}
