using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace server.Models
{
    public class UserServer
    {
        [Key]
        [Column("id")]
        public int ID { get; set; }

        public Server Server { get; set; }

        public User User { get; set; }
    }
}