using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MakeVolunteerGreatAgain.Core.Entities;

namespace MakeVolunteerGreatAgain.Persistence
{
    // Контекст базы данных, наследуемый от IdentityDbContext для поддержки Identity
    public class ApplicationDbContext : IdentityDbContext<CommonUser, IdentityRole<int>, int>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        // DbSet для каждой сущности в базе данных
        public DbSet<Volunteer> Volunteers { get; set; }
        public DbSet<Organization> Organizations { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Application> Applications { get; set; }
        public DbSet<Subscription> Subscriptions { get; set; }
        public DbSet<Invitation> Invitations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Volunteer -> CommonUser: Один к одному
            // Volunteer имеет внешний ключ CommonUserId, указывающий на CommonUser
            modelBuilder.Entity<Volunteer>()
                .HasOne(v => v.CommonUser)
                .WithMany()
                .HasForeignKey(v => v.CommonUserId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            // Organization -> CommonUser: Один к одному
            // Organization имеет внешний ключ CommonUserId, указывающий на CommonUser
            modelBuilder.Entity<Organization>()
                .HasOne(o => o.CommonUser)
                .WithOne()
                .HasForeignKey<Organization>(o => o.CommonUserId)
                .IsRequired();

            // Volunteer -> Applications: Один ко многим
            // Один Volunteer может иметь много Applications
            modelBuilder.Entity<Volunteer>()
                .HasMany(v => v.Applications)
                .WithOne(a => a.Volunteer)
                .HasForeignKey(a => a.VolunteerId)
                .OnDelete(DeleteBehavior.Cascade);

            // Volunteer -> Subscriptions: Один ко многим
            // Один Volunteer может иметь много Subscriptions
            modelBuilder.Entity<Volunteer>()
                .HasMany(v => v.Subscriptions)
                .WithOne(s => s.Volunteer)
                .HasForeignKey(s => s.VolunteerId)
                .OnDelete(DeleteBehavior.Cascade);

            // Organization -> Events: Один ко многим
            // Одна Organization может иметь много Events
            modelBuilder.Entity<Organization>()
                .HasMany(o => o.Events)
                .WithOne(e => e.Organization)
                .HasForeignKey(e => e.OrganizationId)
                .OnDelete(DeleteBehavior.Cascade);

            // Organization -> Subscriptions: Один ко многим
            // Одна Organization может иметь много Subscriptions
            modelBuilder.Entity<Organization>()
                .HasMany(o => o.Subscriptions)
                .WithOne(s => s.Organization)
                .HasForeignKey(s => s.OrganizationId)
                .OnDelete(DeleteBehavior.Cascade);

            // Event -> Organization: Один ко многим
            // Один Event принадлежит одной Organization
            modelBuilder.Entity<Event>()
                .HasOne(e => e.Organization)
                .WithMany(o => o.Events)
                .HasForeignKey(e => e.OrganizationId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            // Event -> Applications: Один ко многим
            // Один Event может иметь много Applications
            modelBuilder.Entity<Event>()
                .HasMany(e => e.Applications)
                .WithOne(a => a.Event)
                .HasForeignKey(a => a.EventId)
                .OnDelete(DeleteBehavior.Cascade);

            // Application -> Volunteer: Один к одному
            // Один Application принадлежит одному Volunteer
            modelBuilder.Entity<Application>()
                .HasOne(a => a.Volunteer)
                .WithMany(v => v.Applications)
                .HasForeignKey(a => a.VolunteerId)
                .OnDelete(DeleteBehavior.Cascade);

            // Application -> Event: Один к одному
            // Один Application принадлежит одному Event
            modelBuilder.Entity<Application>()
                .HasOne(a => a.Event)
                .WithMany(e => e.Applications)
                .HasForeignKey(a => a.EventId)
                .OnDelete(DeleteBehavior.Cascade);

            // Invitation -> Organization: Один к одному
            // Один Invitation принадлежит одной Organization
            modelBuilder.Entity<Invitation>()
                .HasOne(i => i.Organization)
                .WithMany()
                .HasForeignKey(i => i.OrganizationId)
                .OnDelete(DeleteBehavior.Cascade);

            // Invitation -> Volunteer: Один к одному
            // Один Invitation принадлежит одному Volunteer
            modelBuilder.Entity<Invitation>()
                .HasOne(i => i.Volunteer)
                .WithMany()
                .HasForeignKey(i => i.VolunteerId)
                .OnDelete(DeleteBehavior.Cascade);

            // Subscription -> Volunteer: Один к одному
            // Один Subscription принадлежит одному Volunteer
            modelBuilder.Entity<Subscription>()
                .HasOne(s => s.Volunteer)
                .WithMany(v => v.Subscriptions)
                .HasForeignKey(s => s.VolunteerId)
                .OnDelete(DeleteBehavior.Cascade);

            // Subscription -> Organization: Один к одному
            // Один Subscription принадлежит одной Organization
            modelBuilder.Entity<Subscription>()
                .HasOne(s => s.Organization)
                .WithMany(o => o.Subscriptions)
                .HasForeignKey(s => s.OrganizationId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
