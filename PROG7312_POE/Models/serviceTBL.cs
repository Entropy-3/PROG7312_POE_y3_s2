using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PROG7312_POE.Models
{
    //enums for priority and status
    public enum RequestPriority { Low = 1, Medium = 2, High = 3, Critical = 4 }
    public enum RequestStatus { New, Acknowledged, InProgress, OnHold, Resolved, Closed }


    public class serviceTBL
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ServiceID { get; set; }

        [ForeignKey("User")]
        public int UserID { get; set; }
        public userTBL? User { get; set; }

        [Required]
        public string Title{ get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public RequestPriority Priority { get; set; }

        [Required] 
        public RequestStatus Status { get; set; }

        public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;
    }
}
