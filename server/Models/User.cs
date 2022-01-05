using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace server.Models
{
    public class User {
        [Key]
        [Column("id")]
        public int ID { get; set; }
        
        [StringLength(25)]
        [Column("username")]
        public string Username { get; set; }

        [StringLength(50)]
        [Column("full_name")]
        public string FullName { get; set; }


        [StringLength(72)]
        [Column("password")]
        public string Password { get; set; }

        [Column("priority")]
        public int Priority { get; set; }

        [JsonIgnore]
        public List<Report> Reports { get; set; }

        [JsonIgnore]
        public List<UserServer> UserServer { get; set; }
    }
}