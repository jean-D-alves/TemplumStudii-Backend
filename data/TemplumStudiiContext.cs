using Microsoft.EntityFrameworkCore;
using templumStudii.models;
namespace TemplumStudii.data
{
	public class TemplumStudiiContext : DbContext
	{
		public TemplumStudiiContext(DbContextOptions<TemplumStudiiContext> options) : base(options)
		{
		}
		public DbSet<User> Users { get; set; }
		public DbSet<Time> Times { get; set; }
		public DbSet<Sequence> Sequences { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasIndex(u => u.email)
                .IsUnique();
        }
    }
}