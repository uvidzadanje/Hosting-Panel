using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace server.Models
{
    public class Datacenter
    {
        [Key]
        [Column("id")]
        public int ID { get; set; }

        [Required]
        [StringLength(70)]
        [Column("name")]
        public string Name { get; set; }

        [Required]
        [Column("address")]
        public string Address { get; set; }

        [Required]
        [Column("city")]
        public string City { get; set; }

        [Required]
        [Column("country")]
        public string Country { get; set; }

        public List<Server> Servers { get; set; }
    }
}