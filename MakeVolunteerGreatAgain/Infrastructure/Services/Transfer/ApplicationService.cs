using MakeVolunteerGreatAgain.Core.Services;
using MakeVolunteerGreatAgain.Persistence;
using MakeVolunteerGreatAgain.Core.Entities;
using MakeVolunteerGreatAgain.Core.Repositories.DTO;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.HttpResults;

namespace MakeVolunteerGreatAgain.Infrastructure.Services.Transfer; 
 
public class ApplicationService : IApplicationService 
{
    private readonly ApplicationDbContext _context;

    public ApplicationService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Application?> GetApplicationByIdAsync(int id)
    {
        return await _context.Applications.FindAsync(id);
    }

    public async Task<Application> ApplyAsync(ApplicationCreateDTO applicationModel, int volunteerCommonUserId, int eventId)
    {
        // Проверка существования волонтера по CommonUserId
        var volunteer = await _context.Volunteers
            .FirstOrDefaultAsync(v => v.CommonUserId == volunteerCommonUserId) ?? throw new Exception("Volunteer not found");

        // Проверка существования мероприятия по eventId
        var eventt = await _context.Events
            .FirstOrDefaultAsync(e => e.Id == eventId) ?? throw new Exception("Event not found");

        // Создание объекта заявки с использованием идентификатора волонтера
        var applicationObj = new Application
        {
            CoverLetter = applicationModel.CoverLetter,
            Status = ApplicationStatus.UnderСonsideration.ToString(),
            VolunteerId = volunteer.Id, // Установка VolunteerId как идентификатор волонтера
            EventId = eventId,
            Volunteer = volunteer,
            Event = eventt,
            CreatedAt = DateTime.Now.ToUniversalTime(),
            UpdatedAt = DateTime.Now.ToUniversalTime()
        };
        _context.Applications.Add(applicationObj);
        await _context.SaveChangesAsync();
        return applicationObj;
    }

    public async Task<bool> UnapplyAsync(int applicationId)
    {
        var applicationToUnapply = await _context.Applications.FindAsync(applicationId);
        if (applicationToUnapply == null)
        {
            return false;
        }
        _context.Applications.Remove(applicationToUnapply);
        await _context.SaveChangesAsync();
        return true;  
    }


    public async Task<IEnumerable<Application>?> GetApplicationsByEventIdAsync(int eventId, int organizationCommonUserId)
    {
        //Поиск организации в бд
        var organization = await _context.Organizations
            .FirstOrDefaultAsync(o => o.CommonUserId == organizationCommonUserId) ?? throw new Exception("Organization not found");

        //проверяем, не пытается ли какая-то левая организация запросить заявки на чужое мероприятие
        if (_context.Applications.Where(a => a.EventId == eventId).Any(a => a.Event.OrganizationId != organization.Id))
        {
            return null; 
        }

        //Выводим только заявки с совпадающим EventId и чтобы принадлежали нашей организации, которая пытается запросить
        var applicationsByEventId = _context.Applications
        .Where(a => a.EventId == eventId && a.Event.OrganizationId == organization.Id);
        
        // Отложенная загрузка связанных объектов Volunteer и Event
        //(потому что при проекции ef core автоматически не подгружает связанные объекты)
        await applicationsByEventId
            .Include(a => a.Volunteer)
            .Include(a => a.Event)
            .LoadAsync();

        return applicationsByEventId;
    }


    public async Task<IEnumerable<Application>?> GetAcceptedApplicationsByEventIdAsync(int eventId,  int organizationCommonUserId)
    {
        //Поиск организации в бд
        var organization = await _context.Organizations
            .FirstOrDefaultAsync(o => o.CommonUserId == organizationCommonUserId) ?? throw new Exception("Organization not found");

        //проверяем, не пытается ли какая-то левая организация запросить заявки на чужое мероприятие
        if (_context.Applications.Where(a => a.EventId == eventId).Any(a => a.Event.OrganizationId != organization.Id))
        {
            return null; 
        }

        //Выводим только заявки с совпадающим EventId и чтобы принадлежали нашей организации, которая пытается запросить
        //Со статусом одобрено
         var acceptedApplicationsByEventId = _context.Applications
        .Where(a => a.EventId == eventId && a.Event.OrganizationId == organization.Id && a.Status == ApplicationStatus.Accepted.ToString());

        // Отложенная загрузка связанных объектов Volunteer и Event 
        //(потому что при проекции ef core автоматически не подгружает связанные объекты)
        await acceptedApplicationsByEventId
            .Include(a => a.Volunteer)
            .Include(a => a.Event)
            .LoadAsync();

        return acceptedApplicationsByEventId;
    }


    public async Task<Application?> AcceptAplicationAsync(int id)
    {
        var existingApplication = await _context.Applications.FindAsync(id);
        if (existingApplication == null)
        {
            return null;
        }
        existingApplication.Status = ApplicationStatus.Accepted.ToString();
        await _context.SaveChangesAsync();
        return existingApplication;
    }

    public async Task<Application?> RejectAplicationAsync(int id)
    {
        var existingApplication = await _context.Applications.FindAsync(id);
        if (existingApplication == null)
        {
            return null;
        }
        existingApplication.Status = ApplicationStatus.Rejected.ToString();
        await _context.SaveChangesAsync();
        return existingApplication;
    }


    public async Task<bool> HasAppliedAsync(int volunteerCommonUserId, int eventId)
    {
        var volunteer = await _context.Volunteers.FirstOrDefaultAsync(v => v.CommonUserId == volunteerCommonUserId) ?? throw new Exception("Volunteer not found");
        var application = await _context.Applications.FirstOrDefaultAsync(a => a.VolunteerId == volunteer.Id && a.EventId == eventId);
        return application != null;
    }
}