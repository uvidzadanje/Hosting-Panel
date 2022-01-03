using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace server.Models
{
    public class ReportType
    {
        [Key]
        [Column("id")]
        public int ID { get; set; }

        [Required]
        [Column("name")]
        public string Name { get; set; }

        [JsonIgnore]
        public List<Report> Reports { get; set; }
    }
}