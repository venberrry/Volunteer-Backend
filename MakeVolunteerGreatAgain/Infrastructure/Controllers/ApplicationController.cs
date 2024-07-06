using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MakeVolunteerGreatAgain.Core.Entities;
using MakeVolunteerGreatAgain.Core.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using MakeVolunteerGreatAgain.Core.Repositories.DTO;

namespace MakeVolunteerGreatAgain.Infrastructure.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ApplicationController : ControllerBase 
    {

        private readonly IApplicationService _applicationService;
        //private readonly ApplicationDbContext _context;

        public ApplicationController(IApplicationService applicationService) {
            _applicationService = applicationService;
        }

        [HttpGet("GetById/{id:int}")]
        public async Task<ActionResult<Application>> GetApplicationById(int id)
        {
            var applicationItem = await _applicationService.GetApplicationByIdAsync(id);
            if (applicationItem == null)
            {
                return NotFound();
            }
            return Ok(applicationItem);
        }

        //Добавть отправление заявки от организации????????
        [Authorize(Roles = "Volunteer")]
        [HttpPost("Apply")]
        public async Task<IActionResult> Apply([FromBody] ApplicationCreateDTO applicationModel, [FromQuery(Name = "event")] int eventId) 
        {
            // Получение идентификатора текущего пользователя
            var volunteerId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            // Проверка, отправлял ли пользователь заявку на указанное мероприятие
            var hasApplied = await _applicationService.HasAppliedAsync(volunteerId, eventId);
            if (hasApplied)
            {
                return BadRequest("Вы уже отправили заявку на это мероприятие.");
            }

            // Создание объекта заявки с использованием идентификатора пользователя
            var createdApplication = await _applicationService.ApplyAsync(applicationModel, volunteerId, eventId);

            return Ok(createdApplication);
        }

        [Authorize(Roles = "Volunteer")]
        [HttpDelete("Unapply/{id:int}")]
        public async Task<IActionResult> Unapply(int id) 
        {
            var success = await _applicationService.UnapplyAsync(id);
            if (!success) 
            {
                return NotFound();
            }
            return NoContent();
        }


        // Поменять формат возвращаемых значений 
        //Объект волонтер и событие берутся некорректно и возвращаются null
        [Authorize(Roles = "Organization")] 
        [HttpGet("GetApplicationsByEventId/{id:int}")]
        public async Task<IActionResult> GetApplicationsByEventId(int id) 
        {
            var applicationsByEventId = await _applicationService.GetApplicationsByEventIdAsync(id);
            var applicationsToReturn = applicationsByEventId.Select(a => new 
            {
                EventTitle = a.Event.Title,
                VolunteerName = a.Volunteer.FirstName + " " + a.Volunteer.LastName,
                a.CoverLetter,
                a.CreatedAt,
                a.Status
            }).ToList();
            return Ok(applicationsToReturn);
        }

        // Поменять формат возвращаемых значений 
        //Объект волонтер и событие берутся некорректно и возвращаются null
        [Authorize(Roles = "Organization")]
        [HttpGet("GetAcceptedApplicationsByEventId/{id:int}")]
        public async Task<IActionResult> GetAcceptedApplicationsByEventId(int id) 
        {
            var applicationsByEventId = await _applicationService.GetAcceptedApplicationsByEventIdAsync(id);
            var applicationsToReturn = applicationsByEventId.Select(a => new 
            {
                EventTitle = a.Event.Title,
                VolunteerName = a.Volunteer.FirstName + " " + a.Volunteer.LastName,
                Volunteer = a.Volunteer,
                a.CoverLetter,
                a.CreatedAt,
                a.Status
            }).ToList();
            return Ok(applicationsToReturn);
        }

        [Authorize(Roles = "Organization")]
        [HttpPut("AcceptAplication/{id:int}")]
        public async Task<IActionResult> AcceptAplication(int id)
        {
            var applicationToAccept = await _applicationService.AcceptAplicationAsync(id);
            if (applicationToAccept == null)
            {
                return NotFound();
            }
            return NoContent();
        }

        [Authorize(Roles = "Organization")]
        [HttpPut("RejectAplication/{id:int}")]
        public async Task<IActionResult> RejectAplication(int id) 
        {
            var applicationToReject = await _applicationService.RejectAplicationAsync(id);
            if (applicationToReject == null)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}

