using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EDUZAGO_PROJECT_DATABASE.Models
{
    public class Schedule
    {
        public int ScheduleID { get; set; }

        public DateTime SessionTime { get; set; }

        public string SessionDetails { get; set; } = string.Empty;

        // Foreign Keys
        public int Course_Code { get; set; }
        [ForeignKey("Course_Code")]
        public Course? Course { get; set; }

        public int Instructor_ID { get; set; }
        [ForeignKey("Instructor_ID")]
        public Instructor? Instructor { get; set; }
    }
}
