using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using AnnouncementApi.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace AnnouncementApi.Infrastructure.Data
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        public AppDbContext(DbContextOptions dbContextOptions) : base(dbContextOptions)
        {

        }

        public DbSet<Group> Groups { get; set; }
        public DbSet<GroupUser> GroupUsers { get; set; }
        public DbSet<Announcement> Announcements { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // GroupUser (ManyToMany Join Table) Composite Key
            builder.Entity<GroupUser>(x => x.HasKey(y => new { y.GroupId, y.UserId }));

            builder.Entity<GroupUser>()
                .HasOne(x => x.User)
                .WithMany(x => x.GroupUsers)
                .HasForeignKey(y => y.UserId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<GroupUser>()
                .HasOne(x => x.Group)
                .WithMany(x => x.GroupUsers)
                .HasForeignKey(y => y.GroupId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            // announcement foreign key
            builder.Entity<Announcement>()
                .HasOne(a => a.User)
                .WithMany(u => u.Announcements)
                .HasForeignKey(a => a.UserId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);


            // Adding Identity Roles
            List<IdentityRole> roles = new List<IdentityRole>
            {
                new IdentityRole
                {
                    Id = "76b04b1d-033e-4e67-8ff6-69a5d3ed1394", // generated from guidgenerator.com
                    Name = "Admin",
                    NormalizedName = "ADMIN"
                },
                new IdentityRole
                {
                    Id = "d381d4b1-f601-4b19-b935-896dfb36319c", // generated from guidgenerator.com
                    Name = "User",
                    NormalizedName = "USER"
                },
            };
            builder.Entity<IdentityRole>().HasData(roles);
        }
    }
}
