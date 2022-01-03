using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

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
        
        [Column("processor")]
        public string Processor { get; set; }

        [Column("ram_capacity")]
        public int RAMCapacity { get; set; }

        [Column("ssd_capacity")]
        public int SSDCapacity { get; set; }

        [JsonIgnore]
        public List<Report> Reports { get; set; }

        [JsonIgnore]
        public List<UserServer> UserServer { get; set; }

        public Datacenter Datacenter { get; set; }
    }
}