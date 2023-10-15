using Microsoft.EntityFrameworkCore;
using Reddit.Models; 

namespace Reddit.Data
{
    public class RedditDBContext : DbContext
    {
        public RedditDBContext(DbContextOptions<RedditDBContext> options) : base(options) {}

        public DbSet<ForumUser> ForumUsers { get; set; }
        public DbSet<BlogPost> BlogPosts { get; set; }
        public DbSet<Comment> Comments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<BlogPost>()
                .HasMany(b => b.Comments) // One BlogPost can have many Comments
                .WithOne(c => c.BlogPost) // Each Comment belongs to one BlogPost
                .HasForeignKey(c => c.BlogPostId) // The foreign key on Comment pointing to BlogPost
                .OnDelete(DeleteBehavior.SetNull); // If BlogPost is deleted, set the foreign key on Comment to null

            modelBuilder.Entity<Comment>()
                .HasOne(c => c.User) // Each Comment has one User
                .WithMany(u => u.Comments) // A User can have many Comments
                .HasForeignKey(c => c.UserId) // The foreign key on Comment pointing to User
                .OnDelete(DeleteBehavior.Restrict); // If User is deleted, prevent the deletion if there are Comments associated
        }

    }
}
