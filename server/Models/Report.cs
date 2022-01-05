using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace server.Models
{
    public class Report
    {
        [Key]
        [Column("id")]
        public int ID { get; set; }
        
        [Column("created_at")]
        [DataType(DataType.Date)]
        public DateTime CreatedAt { get; set; }

        [Required]
        [Column("description")]
        public string Description { get; set; }

        [Column("is_solved")]
        public bool IsSolved { get; set; }

        public Server Server { get; set; }

        public User User { get; set; }

        public ReportType ReportType { get; set; }
    }
}