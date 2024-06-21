using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using VolunteerProject.Models;

namespace VolunteerProject.DataBase
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser<int>, IdentityRole<int>, int>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        
        public DbSet<User> Users { get; set; }
        public DbSet<Volunteer> Volunteers { get; set; }
        public DbSet<Organization> Organizations { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Application> Applications { get; set; }
        public DbSet<Subscription> Subscriptions { get; set; }
        public DbSet<Invitation> Invitation { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
                .HasDiscriminator<string>("UserType")
                .HasValue<Volunteer>("Volunteer")
                .HasValue<Organization>("Organization");

            modelBuilder.Entity<Volunteer>()
                .HasMany(v => v.Applications)
                .WithOne(a => a.Volunteer)
                .HasForeignKey(a => a.VolunteerId);

            modelBuilder.Entity<Volunteer>()
                .HasMany(v => v.Subscriptions)
                .WithOne(s => s.Volunteer)
                .HasForeignKey(s => s.VolunteerId);

            modelBuilder.Entity<Organization>()
                .HasMany(o => o.Events)
                .WithOne(e => e.Organization)
                .HasForeignKey(e => e.OrganizationId);

            modelBuilder.Entity<Organization>()
                .HasMany(o => o.Subscriptions)
                .WithOne(s => s.Organization)
                .HasForeignKey(s => s.OrganizationId);

            modelBuilder.Entity<Event>()
                .HasOne(e => e.Organization)
                .WithMany(o => o.Events)
                .HasForeignKey(e => e.OrganizationId)
                .IsRequired();

        }
    }
}
