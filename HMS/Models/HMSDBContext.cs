using HMS.DTOs;
using HMS.Models.Admin;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HMS.Models
{
    public class HMSDBContext : IdentityDbContext<IdentityUser>
    {
        public HMSDBContext(DbContextOptions<HMSDBContext> options):base(options)
        {
                
        }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<AdminRoomImage> AdminRoomImages { get; set; }

        //Admin
        public DbSet<AdminRoom> AdminRooms { get; set; }
        public DbSet<AdminCategory> AdminCategories { get; set; }
        public DbSet<AdminCategoryValue> AdminCategoryValues { get; set; }
        public DbSet<CategoryValue> CategoryValues { get; set; }
        public DbSet<AdminServiceAddon> AdminServiceAddons { get; set; }
        public DbSet<AdminContact> AdminContacts { get; set; }
        public DbSet<AdminBlog> AdminBlogs { get; set; }
        public DbSet<AdminFAQ> AdminFAQs { get; set; }



        //https://learn.microsoft.com/en-us/ef/core/managing-schemas/migrations/?tabs=dotnet-core-cli
        //Excluding parts of your model
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Exclude Identity tables from migrations
            modelBuilder.Entity<IdentityUser>()
                .ToTable("AspNetUsers", t => t.ExcludeFromMigrations());
            modelBuilder.Entity<IdentityRole>()
                .ToTable("AspNetRoles", t => t.ExcludeFromMigrations());
            modelBuilder.Entity<IdentityUserRole<string>>()
                .ToTable("AspNetUserRoles", t => t.ExcludeFromMigrations());
            modelBuilder.Entity<IdentityUserClaim<string>>()
                .ToTable("AspNetUserClaims", t => t.ExcludeFromMigrations());
            modelBuilder.Entity<IdentityUserLogin<string>>()
                .ToTable("AspNetUserLogins", t => t.ExcludeFromMigrations());
            modelBuilder.Entity<IdentityUserToken<string>>()
                .ToTable("AspNetUserTokens", t => t.ExcludeFromMigrations());
            modelBuilder.Entity<IdentityRoleClaim<string>>()
                .ToTable("AspNetRoleClaims", t => t.ExcludeFromMigrations());

            modelBuilder.Entity<AdminRoom>()
                 .HasMany(ar => ar.CategoryValues)
                 .WithOne(ac => ac.AdminRoom)
                 .HasForeignKey(ac => ac.AdminRoomId)
                 .OnDelete(DeleteBehavior.Cascade); // Optional: define cascade delete behavior

            modelBuilder.Entity<AdminRoom>()
                .HasMany(ar => ar.ServiceAddons)
                .WithOne(sa => sa.AdminRoom)
                .HasForeignKey(sa => sa.AdminRoomId)
                .OnDelete(DeleteBehavior.Cascade); // Optional: define set null behavior

            //modelBuilder.Entity<AdminRoom>()
            //    .HasMany(ar => ar.AdditionalInfo)
            //    .WithOne(ai => ai.AdminRoom)
            //    .HasForeignKey(ai => ai.AdminRoomId)
            //    .OnDelete(DeleteBehavior.SetNull); // Optional: define set null behavior

            modelBuilder.Entity<AdminCategory>()
                .HasMany(c => c.AdminCategoryValues)
                .WithOne(cv => cv.AdminCategory)
                .HasForeignKey(cv => cv.AdminCategoryId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<AdminRoom>()
                .HasMany(r => r.AdminRoomImages)
                .WithOne(i => i.AdminRoom)
                .HasForeignKey(i => i.AdminRoomId);

            modelBuilder.Entity<AdminRoomImage>()
            .Property(b => b.IsCoverImage)
            .HasDefaultValue(false);
            //what happen to categoryvalues if AdminCategory values is deleted

            //mapping IS-A relationship TPT Table Per Type way
            modelBuilder.Entity<AdminRoomImage>().ToTable("AdminRoomImages");

        }



    }
}
