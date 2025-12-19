using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EDUZAGO_PROJECT_DATABASE.Models
{
    public class Payment
    {
        [Key]
        public string PaymentCode { get; set; } = string.Empty;

        public DateTime PaymentDate { get; set; }

        public decimal PaymentAmount { get; set; }

        public string PaymentStatus { get; set; } = "Pending";

        // Foreign Keys
        public int Student_ID { get; set; }
        [ForeignKey("Student_ID")]
        public Student? Student { get; set; }

        public string Course_Code { get; set; } = string.Empty;
        [ForeignKey("Course_Code")]
        public Course? Course { get; set; }
    }
}
