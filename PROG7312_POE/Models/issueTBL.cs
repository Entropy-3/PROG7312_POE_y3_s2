using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PROG7312_POE.Models
{
    public class issueTBL
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IssueID { get; set; }

        //Ai assisted with foreign key
        [ForeignKey("User")]
        public int UserID { get; set; }
        public userTBL? User { get; set; }

        [Required]
        public string Location { get; set; }

        [Required]
        public string Category { get; set; }

        [Required]
        public string Description { get; set; }

        public byte[]? DocumentData { get; set; }
    }
}
