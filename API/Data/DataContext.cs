using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base (options) {}

        public DbSet<Node> Nodes { get; set; }
        public DbSet<Network> Networks { get; set; }
        public DbSet<Network_MN_Nodes> Network_MN_Nodes { get; set; }
    }
}