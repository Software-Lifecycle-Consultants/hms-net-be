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
        

        //https://learn.microsoft.com/en-us/ef/core/managing-schemas/migrations/?tabs=dotnet-core-cli
        //Excluding parts of your model
        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<IdentityUser>()
        //        .ToTable("AspNetUsers", t => t.ExcludeFromMigrations());
        //}
    }
}
