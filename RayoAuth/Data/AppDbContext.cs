using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace RayoAuth.Data
{
    public class AppDbContext : IdentityDbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> opts):base(opts)
        {

        }

        public DbSet<StandingsModel>? Standings{ get; set; }
    }
}
