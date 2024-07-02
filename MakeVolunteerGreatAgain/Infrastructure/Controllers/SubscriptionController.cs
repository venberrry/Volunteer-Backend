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

        [Authorize(Roles = "Organization")]
        [HttpGet("GetSubscriptions")]
        public async Task<IActionResult> GetSubscriptions()
        {
            var subscriptions = await _subscriptionService.GetSubscriptionsAsync();

            var volunteerId = subscriptions.Select(s => s.VolunteerId).ToList();

            return Ok(volunteerId);
        }

        [Authorize(Roles = "Volunteer")]
        [HttpGet("MySubscriptions")]
        public async Task<IActionResult> GetSubscriptionsForVolunteer()
        {
            var volunteerId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var subscriptions = await _subscriptionService.GetSubscriptionsByVolunteerAsync(volunteerId);

            return Ok(subscriptions);
        }
    }
}
