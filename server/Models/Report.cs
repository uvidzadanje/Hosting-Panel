using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace server.Models
{
    class Report
    {
        [Key]
        [Column("id")]
        public int ID { get; set; }
        
        [Required]
        [Column("created_at")]
        [DataType(DataType.Date)]
        public DateTime CreatedAt { get; set; }

        [Required]
        [Column("description")]
        public string Description { get; set; }

        public Server Server { get; set; }

        public User User { get; set; }

        public ReportType ReportType { get; set; }
    }
}