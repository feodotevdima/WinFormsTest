using Core;
using Microsoft.EntityFrameworkCore;

namespace Persistence
{
    public class AppDbContext : DbContext
    {
        public DbSet<Status> Statuses { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Post> Posts { get; set; }

        public virtual DbSet<StatisticsItem> StatisticsItems { get; set; }
        public virtual DbSet<Person> Persons { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Status>(entity => entity.ToTable("status", schema: "dbo"));
            modelBuilder.Entity<Department>(entity => entity.ToTable("deps", schema: "dbo"));
            modelBuilder.Entity<Post>(entity => entity.ToTable("posts", schema: "dbo"));

            modelBuilder.Entity<StatisticsItem>().HasNoKey();
            modelBuilder.Entity<Person>().HasNoKey();
        }
    }
}
