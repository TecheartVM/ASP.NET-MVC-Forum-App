using Microsoft.EntityFrameworkCore;
using MVC_test.Models;

namespace MVC_test.Data
{
    public class ForumDbContext : DbContext
    {
        public DbSet<Topic> Topics { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<User> Users { get; set; }

        public ForumDbContext(DbContextOptions<ForumDbContext> options) : base(options)
        {
            /*Database.EnsureDeleted();*/
            Database.EnsureCreated();
        }

        /*protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity(typeof (Comment)).HasOne("MVC_test.Models.User").WithMany().OnDelete(DeleteBehavior.NoAction);

            base.OnModelCreating(builder);
        }*/
    }
}
