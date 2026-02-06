using Microsoft.EntityFrameworkCore;
using To_do_teste.src.Entities;
using Task = To_do_teste.src.Entities.Task;

namespace To_do_teste.src.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users => Set<User>();
        public DbSet<Task> Tasks => Set<Task>();
        public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().ToTable("users");
            modelBuilder.Entity<Task>().ToTable("tasks");
            modelBuilder.Entity<RefreshToken>().ToTable("refreshtokens");

            base.OnModelCreating(modelBuilder);
        }
    }
}
