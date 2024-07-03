using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;
using MakeVolunteerGreatAgain.Core.Entities;
using MakeVolunteerGreatAgain.Core.Services;

namespace MakeVolunteerGreatAgain.Web.Controllers
{
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

            return Ok(subscription);
        }

        [Authorize(Roles = "Volunteer")]
        [HttpPost("AcceptInvitation/{invitationId:int}")]
        public async Task<IActionResult> SubscribeByInvitation(int invitationId)
        {
            var volunteerId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var subscription = await _subscriptionService.SubscribeByInvitationAsync(invitationId, volunteerId);

            if (subscription == null)
            {
                return NotFound("Invitation not found or already accepted.");
            }

            return Ok(subscription);
        }

        // Выводит все подписки, а надо только конкретной организации!!!!
        [Authorize(Roles = "Organization")]
        [HttpGet("GetSubscriptions")]
        public async Task<IActionResult> GetSubscriptions()
        {
            var organizationId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            
            var subscriptions = await _subscriptionService.GetSubscriptionsAsync(organizationId);
            
            var subscriptionIds = subscriptions.Select(s => s.VolunteerId).ToList();

            return Ok(subscriptionIds);
        }
        
        [Authorize(Roles = "Volunteer")]
        [HttpGet("MySubscriptions")]
        public async Task<IActionResult> GetSubscriptionsForVolunteer()
        {
            var volunteerCommonUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            // Получаем подписки для волонтера по его CommonUserId
            var subscriptions = await _subscriptionService.GetSubscriptionsByVolunteerAsync(volunteerCommonUserId);

            // Извлекаем названия организаций, на которые подписан волонтер
            var organizationNames = subscriptions.Select(s => s.OrganizationId).ToList();

            return Ok(organizationNames);
        }

    }
}
