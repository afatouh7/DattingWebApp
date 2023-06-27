using APi.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace APi.Data
{
    public class DataContext :IdentityDbContext<AppUser,AppRole,int,
        IdentityUserClaim<int>,AppUserRole,IdentityUserLogin<int>,IdentityRoleClaim<int>,IdentityUserToken<int>>
    {
        public DataContext(DbContextOptions<DataContext> options):base(options)
        {
            
        }

        public DbSet<UserLike> Likes { get; set; }
        public DbSet<Message> Messages { get; set; }
        protected override void OnModelCreating (ModelBuilder builder)
        { 
            base.OnModelCreating (builder);

            builder.Entity<AppUser>()
                .HasMany(u => u.UserRoles)
                .WithOne(u => u.User)
                .HasForeignKey(u => u.UserId)
                .IsRequired();


            builder.Entity<AppRole>()
                .HasMany(u => u.UserRoles)
                .WithOne(u => u.Role)
                .HasForeignKey(u => u.RoleId)
                .IsRequired();

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

            builder.Entity<Message>()
                .HasOne(u => u.Recipient)
                .WithMany(u => u.MessagesRecived)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Message>()
              .HasOne(u => u.Sender)
              .WithMany(u => u.MessagesSent)
              .OnDelete(DeleteBehavior.Restrict);


        }



    }
}
