using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EDUZAGO_PROJECT_DATABASE.Models
{
    public class Payment
    {
        [Key]
        public int PaymentCode { get; set; }

        public DateTime PaymentDate { get; set; }

        public decimal PaymentAmount { get; set; }

        public string PaymentStatus { get; set; } = "Pending";

        // Foreign Keys
        public int Student_ID { get; set; }
        [ForeignKey("Student_ID")]
        public Student? Student { get; set; }

        public int Course_Code { get; set; }
        [ForeignKey("Course_Code")]
        public Course? Course { get; set; }
    }
}
