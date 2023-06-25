using APi.Entities;
using Microsoft.EntityFrameworkCore;

namespace APi.Data
{
    public class DataContext :DbContext
    {
        public DataContext(DbContextOptions<DataContext> options):base(options)
        {
            
        }
        public DbSet<AppUser> Users { get; set; }
        public DbSet<UserLike> Likes { get; set; }
        protected override void OnModelCreating (ModelBuilder builder)
        {
            base.OnModelCreating (builder);
            builder.Entity<UserLike>()
                .HasKey(x => new { x.SorceUserId, x.LikeUserId });
            builder.Entity<UserLike>()
                .HasOne(s => s.SorceUser)
                .WithMany(d => d.LikedUser)
                .HasForeignKey(s=>s.SorceUserId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.Entity<UserLike>()
               .HasOne(s => s.LikeUser)
               .WithMany(d => d.LikeByUser)
               .HasForeignKey(s => s.LikeUserId)
               .OnDelete(DeleteBehavior.Restrict);


        }



    }
}
