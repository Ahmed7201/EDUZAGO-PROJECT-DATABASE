using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EDUZAGO_PROJECT_DATABASE.Models
{
    public class Category
    {
        public int CategoryID { get; set; }

        [Required]
        public string CategoryName { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        // Foreign Key for Admin
        public int Admin_ID { get; set; }
        [ForeignKey("Admin_ID")]
        public Admin? Admin { get; set; }
    }
}
