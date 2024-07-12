using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MakeVolunteerGreatAgain.Core.Entities;
using MakeVolunteerGreatAgain.Core.Services;
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
        //Здесь eventid берется как query-параметр,то есть то, что в адресной строке будет после слова event (?event=1)
        public async Task<IActionResult> Apply([FromBody] ApplicationCreateDTO applicationModel, [FromQuery(Name = "event")] int eventId) 
        {
            // Получение идентификатора текущего пользователя
            var volunteerId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            // Проверка, отправлял ли пользователь заявку на указанное мероприятие
            var hasApplied = await _applicationService.HasAppliedAsync(volunteerId, eventId);
            if (hasApplied)
            {
                return BadRequest(new { Message = "Вы уже отправили заявку на это мероприятие." });
            }

            // Создание объекта заявки с использованием идентификатора пользователя
            var createdApplication = await _applicationService.ApplyAsync(applicationModel, volunteerId, eventId);

            return Ok(new { Message = "Заявка успешно создана." }); // Возвращаем только код статуса и краткий комментарий
        }


        [Authorize(Roles = "Volunteer")]
        [HttpDelete("Unapply/{id:int}")]
        public async Task<IActionResult> Unapply(int id) 
        {
            var success = await _applicationService.UnapplyAsync(id);
            if (!success) 
            {
                return NotFound( new { Message = "Заявка не найдена." }); // Возвращаем только код статуса и краткий комментарий
            }
            return Ok(new { Message = "Заявка успешно удалена." }); // Возвращаем только код статуса и краткий комментарий
        }


        [Authorize(Roles = "Organization")] 
        [HttpGet("GetApplicationsByEventId/{id:int}")]
        public async Task<IActionResult> GetApplicationsByEventId(int id) 
        {
            // Получение идентификатора текущего пользователя (организации)
            var organizationId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var applicationsByEventId = await _applicationService.GetApplicationsByEventIdAsync(id, organizationId);

            if (applicationsByEventId == null)
            {
                return StatusCode(403, new { Message = "У вас нет прав для просмотра заявок на это мероприятие"});
            }
            
            if (!applicationsByEventId.Any())
            {
                return NotFound(new { Message = "Ни одна заявка ещё не была отправлена."});
            }

            var applicationsToReturn = applicationsByEventId.Select(a => new 
            {
                EventTitle = a.Event.Title,
                VolunteerName = a.Volunteer.FirstName + " " + a.Volunteer.LastName,
                a.CoverLetter,
                a.CreatedAt.Date,
                a.Status
            }).ToList();
            return Ok(applicationsToReturn);
        }


        [Authorize(Roles = "Organization")]
        [HttpGet("GetAcceptedApplicationsByEventId/{id:int}")]
        public async Task<IActionResult> GetAcceptedApplicationsByEventId(int id) 
        {
            // Получение идентификатора текущего пользователя (организации)
            var organizationId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var applicationsByEventId = await _applicationService.GetAcceptedApplicationsByEventIdAsync(id, organizationId);

            if (applicationsByEventId == null)
            {
                return StatusCode(403, new { Message = "У вас нет прав для просмотра заявок на это мероприятие"});
            }

            if (!applicationsByEventId.Any())
            {
                return BadRequest(new { Message = "Ни одна заявка ещё не была одобрена."});
            }

            var applicationsToReturn = applicationsByEventId.Select(a => new 
            {
                EventTitle = a.Event.Title,
                VolunteerName = a.Volunteer.FirstName + " " + a.Volunteer.LastName,
                a.CoverLetter,
                a.CreatedAt.Date,
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
                return NotFound(new { Message = "Заявка не найдена." });
            }
            return Ok(new { Message = "Заявка успешно одобрена." });
        }


        [Authorize(Roles = "Organization")]
        [HttpPut("RejectAplication/{id:int}")]
        public async Task<IActionResult> RejectAplication(int id) 
        {
            var applicationToReject = await _applicationService.RejectAplicationAsync(id);
            if (applicationToReject == null)
            {
                return NotFound(new { Message = "Заявка не найдена." });
            }
            return Ok(new { Message = "Заявка успешно отклонена." });
        }
    }
}

