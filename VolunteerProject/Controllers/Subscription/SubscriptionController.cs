using Microsoft.EntityFrameworkCore;
using VolunteerProject.DataBase;

namespace VolunteerProject.Controllers.Subscription;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;
using VolunteerProject.Models;
using VolunteerProject.Services;
using VolunteerProject.Services.Subscription;

[ApiController]
[Route("api/[controller]")]
public class SubscriptionController : ControllerBase
{
    private readonly ISubscriptionService _subscriptionService;
    private readonly ApplicationDbContext _context;

    public SubscriptionController(ISubscriptionService subscriptionService, ApplicationDbContext context)
    {
        _subscriptionService = subscriptionService;
        _context = context;
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
        
        return Ok(subscriptions);
    }
    
    [Authorize(Roles = "Volunteer")]
    [HttpGet("MySubscriptions")]
    public async Task<IActionResult> GetSubscriptionsForVolunteer()
    {
        var volunteerId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        var subscriptions = await _context.Subscriptions
            .Where(s => s.VolunteerId == volunteerId)
            .ToListAsync();

        return Ok(subscriptions);
    }
}