using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace server.Models
{
    public class User {
        [Key]
        [Column("id")]
        public int ID { get; set; }

        [Required]
        [StringLength(25)]
        [Column("username")]
        public string Username { get; set; }

        [Required]
        [StringLength(50)]
        [Column("full_name")]
        public string FullName { get; set; }

        [Required]
        [StringLength(72)]
        [Column("password")]
        public string Password { get; set; }

        [Required]
        [Column("priority")]
        public int Priority { get; set; }

        public List<Report> Reports { get; set; }

        public List<UserServer> UserServer { get; set; }
    }
}