using Microsoft.EntityFrameworkCore;

namespace server.Models
{
    public class HostingContext: DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Server> Servers { get; set; }
        public DbSet<Report> Reports { get; set; }
        public DbSet<UserServer> UserServers { get; set; }
        public DbSet<ReportType> ReportTypes { get; set; }

        public HostingContext(DbContextOptions options):base(options) { }

    }
}