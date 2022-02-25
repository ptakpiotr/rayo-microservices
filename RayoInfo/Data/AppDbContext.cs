using Microsoft.EntityFrameworkCore;

namespace RayoInfo.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> opts):base(opts)
        {

        }

        public DbSet<NewsModel>? News{ get; set; }
        public DbSet<CommentModel>? Comments{ get; set; }
        public DbSet<StandingsModel>? Standings{ get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<NewsModel>()
                 .HasMany(n => n.Comments)
                 .WithOne(c => c.News)
                 .HasForeignKey(c => c.NewsId);

            modelBuilder.Entity<CommentModel>()
                .HasOne(c => c.News)
                .WithMany(n => n.Comments)
                .HasForeignKey(c => c.NewsId);
        }
    }
}
