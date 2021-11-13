using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace server.Models
{
    public class Server
    {
        [Key]
        [Column("id")]
        public int ID { get; set; }

        [Required]
        [RegularExpression(@"^(?:[0-9]{1,3}\.){3}[0-9]{1,3}$")]
        [Column("ip_address")]
        public string IPAdress { get; set; }
        
        [Required]
        [Column("processor")]
        public string Processor { get; set; }

        [Required]
        [Column("ram_capacity")]
        public int RAMCapacity { get; set; }

        [Required]
        [Column("ssd_capacity")]
        public int SSDCapacity { get; set; }

        public List<Report> Reports { get; set; }

        public List<UserServer> UserServer { get; set; }

        public Datacenter Datacenter { get; set; }
    }
}