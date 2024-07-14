using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;
using MakeVolunteerGreatAgain.Core.Entities;
using MakeVolunteerGreatAgain.Core.Services;

namespace MakeVolunteerGreatAgain.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SubscriptionController : ControllerBase
{
    private readonly ISubscriptionService _subscriptionService;

    public SubscriptionController(ISubscriptionService subscriptionService)
    {
        _subscriptionService = subscriptionService;
    }

    [Authorize(Roles = "Volunteer")]
    [HttpPost("Subscribe")]
    public async Task<IActionResult> Subscribe(int organizationId)
    {
        var volunteerId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

        var subscription = await _subscriptionService.SubscribeAsync(volunteerId, organizationId);

        return Ok(new { Message = "You have successfully subscribed." });
    }

    [Authorize(Roles = "Volunteer")]
    [HttpPost("AcceptInvitation/{invitationId:int}")]
    public async Task<IActionResult> SubscribeByInvitation(int invitationId)
    {
        var volunteerId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        var subscription = await _subscriptionService.SubscribeByInvitationAsync(invitationId, volunteerId);

        if (subscription == null)
        {
            return NotFound(new { Message = "Invitation not found or already accepted." });
        }

        return Ok(new { Message = "You have successfully subscribed." });
    }

    // Выводит все подписки, а надо только конкретной организации!!!!
    // Выводить тут надо подписчиков, я так понимаю??? 
    [Authorize(Roles = "Organization")]
    [HttpGet("GetSubscriptions")]
    public async Task<IActionResult> GetSubscriptions()
    {
        var organizationId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

        var subscriptions = await _subscriptionService.GetSubscriptionsAsync(organizationId);

        var subscriptionsToReturn = subscriptions.Select(s => new
        {
            SubscriptionId = s.Id, //Получаем айдишник подписки
            VolunteerName = s.Volunteer.FirstName + " " + s.Volunteer.LastName, //Получаем имя фвмилию волонтера
            Status = s.Status //получаем статус подписки (активна или протухла)
        }).ToList();
        return Ok(subscriptionsToReturn);
    }

    [Authorize(Roles = "Volunteer")]
    [HttpGet("MySubscriptions")]
    public async Task<IActionResult> GetSubscriptionsForVolunteer()
    {
        var volunteerCommonUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

        // Получаем подписки для волонтера по его CommonUserId
        var subscriptions = await _subscriptionService.GetSubscriptionsByVolunteerAsync(volunteerCommonUserId);

        // Извлекаем названия организаций, на которые подписан волонтер
        var organizationNames = subscriptions.Select(s => new 
        {
            SubscriptionId = s.Id, //Получаем айдишник подписки
            OrganizationName = s.Organization.Name, //Получаем название организации
            Status = s.Status //Получаем статус подписки (активна или протухла)
        }).ToList();

        return Ok(organizationNames);
    }
}