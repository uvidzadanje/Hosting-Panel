using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace server.Models
{
    class ReportType
    {
        [Key]
        [Column("id")]
        public int ID { get; set; }

        [Required]
        [Column("name")]
        public string Name { get; set; }

        public List<Report> Reports { get; set; }
    }
}