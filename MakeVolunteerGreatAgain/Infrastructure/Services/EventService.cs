using MakeVolunteerGreatAgain.Core.Services;
using MakeVolunteerGreatAgain.Persistence;
using MakeVolunteerGreatAgain.Core.Entities;
using MakeVolunteerGreatAgain.Core.Repositories.DTO;
using Microsoft.EntityFrameworkCore;

namespace MakeVolunteerGreatAgain.Infrastructure.Services;

public class EventService : IEventService
{
    private readonly ApplicationDbContext _context;

    public EventService(ApplicationDbContext context) 
    {
        _context = context;
    }

    public async Task<Event> CreateEventAsync(EventCreateDTO eventModel, int organizationCommonUserId)
    {
        // Проверка существования организации по CommonUserId
        var organization = await _context.Organizations
            .FirstOrDefaultAsync(o => o.CommonUserId == organizationCommonUserId);
        if (organization == null)
        {
            throw new Exception("Organization not found");
        }

        // Создание объекта мероприятия с использованием идентификатора организации
        var eventObj = new Event
        {
            Title = eventModel.Title,
            StartDate = eventModel.StartDate,
            EndDate = eventModel.EndDate,
            City = eventModel.City,
            Description = eventModel.Description,
            OrganizationId = organization.Id, // Установка OrganizationId как идентификатор организации
            Organization = organization
        };

        _context.Events.Add(eventObj);
        await _context.SaveChangesAsync();
        return eventObj;
    }


    public async Task<IEnumerable<Event>> GetAllEventsAsync()
    {
        return await _context.Events.ToListAsync();
    }

    public async Task<Event?> GetEventByIdAsync(int id)
    {
        return await _context.Events.FindAsync(id);
    }


    public async Task<Event> UpdateEventAsync(int id, Event updatedEvent)
    {
        var existingEvent = await _context.Events.FindAsync(id);
        if (existingEvent == null)
        {
            return null;
        }

        existingEvent.Title = updatedEvent.Title;
        existingEvent.PhotoPath = updatedEvent.PhotoPath;
        existingEvent.StartDate = updatedEvent.StartDate;
        existingEvent.EndDate = updatedEvent.EndDate;
        existingEvent.City = updatedEvent.City;
        existingEvent.Description = updatedEvent.Description;
        existingEvent.OrganizationId = updatedEvent.OrganizationId;

        await _context.SaveChangesAsync();
        return existingEvent;
    }

    public async Task<bool> DeleteEventAsync(int id)
    {
        var eventToDelete = await _context.Events.FindAsync(id);
        if (eventToDelete == null)
        {
            return false;
        }

        _context.Events.Remove(eventToDelete);
        await _context.SaveChangesAsync();
        return true;
    }
}
