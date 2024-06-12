namespace VolunteerProject.DataBase;

using Microsoft.EntityFrameworkCore;
using Models;
public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<User> Users { get; set; }
    public DbSet<Organization> Organizations { get; set; }
    public DbSet<Event> Events { get; set; }
    public DbSet<JoinRequest> JoinRequests { get; set; }
    public DbSet<EventParticipants> EventParticipants { get; set; }
    public DbSet<OrganizationMembership> OrganizationMemberships { get; set; }
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        // Сделать
    }
}