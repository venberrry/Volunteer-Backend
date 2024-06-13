using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using VolunteerProject.Models;

namespace VolunteerProject.DataBase
{
    public class ApplicationDbContext : IdentityDbContext<User, IdentityRole<int>, int>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Volunteer> Volunteers { get; set; }
        public DbSet<Organization> Organizations { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Application> Applications { get; set; }
        public DbSet<Subscription> Subscriptions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Один волонтёр связан с одной записью в User через внешний ключ UserId
            modelBuilder.Entity<Volunteer>()
                .HasOne(v => v.User)
                .WithOne()
                .HasForeignKey<Volunteer>(v => v.UserId);

            // Одна организация связана с одной записью в User через внешний ключ UserId
            modelBuilder.Entity<Organization>()
                .HasOne(o => o.User)
                .WithOne()
                .HasForeignKey<Organization>(o => o.UserId);

            // Конфигурация entity Volunteer
            // "один ко многим" между волонтерами и заявками
            modelBuilder.Entity<Volunteer>()
                .HasMany(v => v.Applications) // У одного волонтера может быть много заявок
                .WithOne(a => a.Volunteer) // Каждая заявка связана с одним волонтером
                .HasForeignKey(a => a.VolunteerId); // Связываем внешний ключ в таблице Applications и VolunteerId

            // "один ко многим" между волонтерами и подписками
            modelBuilder.Entity<Volunteer>()
                .HasMany(v => v.Subscriptions) // У одного волонтера может быть много подписок
                .WithOne(s => s.Volunteer) // Каждая подписка связана с одним волонтером
                .HasForeignKey(s => s.VolunteerId); // Связываем внешний ключ в таблице Subscriptions и VolunteerId

            // Конфигурация entity Organization
            // "один ко многим" между организациями и мероприятиями
            modelBuilder.Entity<Organization>()
                .HasMany(o => o.Events) // У одной организации может быть много мероприятий
                .WithOne(e => e.Organization) // Каждое мероприятие связано с одной организацией
                .HasForeignKey(e => e.OrganizationId); // Связываем внешний ключ в таблице Events и OrganizationId
            
            // "один ко многим" между организациями и подписками
            modelBuilder.Entity<Organization>()
                .HasMany(o => o.Subscriptions) // У одной организации может быть много подписок
                .WithOne(s => s.Organization) // Каждое подписка связано с одной организацией
                .HasForeignKey(s => s.OrganizationId); // Связываем внешний ключ в таблице Subscriptions и OrganizationId

            // Конфигурация entity Event 
            // "один ко многим" между мероприятиями и заявками
            modelBuilder.Entity<Event>()
                .HasMany(e => e.Applications) // У одного мероприятия может быть много заявок
                .WithOne(a => a.Event) // Каждая заявка связана с одним мероприятием
                .HasForeignKey(a => a.EventId); // Ключ Applications с форейн кеем Event.EventId
        }
    }
}
