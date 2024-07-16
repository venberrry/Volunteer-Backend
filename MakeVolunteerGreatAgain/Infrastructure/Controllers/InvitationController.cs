using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using MakeVolunteerGreatAgain.Core.DTOs;
using MakeVolunteerGreatAgain.Core.Entities;
using MakeVolunteerGreatAgain.Core.Services;

namespace MakeVolunteerGreatAgain.Infrastructure.Controllers;

[ApiController]
[Route("api/[controller]")]
public class InvitationController : ControllerBase
{
    private readonly IInvitationService _invitationService;

    public InvitationController(IInvitationService invitationService)
    {
        _invitationService = invitationService;
    }

    [Authorize(Roles = "Organization")]
    [HttpPost("CreateInvitation")]
    public async Task<IActionResult> CreateInvitation([FromBody] CreateInvitationDTO invitation)
    {
        var organizationId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

        var volunteerId = invitation.VolunteerId;

        var createdInvitation = await _invitationService.CreateInvitationAsync(volunteerId, organizationId);
        
        return Ok( new {Message = "Приглашение создано."});
    }

    [Authorize(Roles = "Organization")]
    [HttpGet("GetAllInvitations")]
    public async Task<IActionResult> GetAllInvitations()
    {
        var organizationId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

        var invitations = await _invitationService.GetAllInvitationsAsync(organizationId);

        var invitationsToReturn = invitations.Select(i => new
        {
            invitationId = i.Id,
            volunteerName = i.Volunteer.FirstName + " " + i.Volunteer.LastName,
            organizationName = i.Organization.Name,
            status = i.Status
        }).ToList();
        return Ok(invitationsToReturn);
    }


    //TODO: прикрутить отправку уведомления в профиль (или на почту)
    [Authorize(Roles = "Volunteer")]
    [HttpGet("GetAllInvitationsForVolunteer")]
    public async Task<IActionResult> GetAllInvitationsForVolunteer()
    {
        var volunteerId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        var invitations = await _invitationService.GetAllInvitationsForVolunteerAsync(volunteerId);

        var invitationsToReturn = invitations.Select(i => new 
        {
            invitationId = i.Id,
            organizationName = i.Organization.Name,
            status = i.Status
        }).ToList();
        return Ok(invitationsToReturn);
    }
    

    [Authorize(Roles = "Organization")]
    [HttpGet("GetInvitationById/{id:int}")]
    public async Task<IActionResult> GetInvitationById(int id)
    {
        var invitation = await _invitationService.GetInvitationByIdAsync(id);
        if (invitation == null)
        {
            return NotFound("Invitation not found.");
        }
        return Ok(invitation);
    }

    [Authorize(Roles = "Organization")]
    [HttpPut("UpdateInvitation/{id:int}")]
    public async Task<IActionResult> UpdateInvitation(int id, [FromBody] Invitation updatedInvitation)
    {
        var invitation = await _invitationService.UpdateInvitationAsync(id, updatedInvitation);
        if (invitation == null)
        {
            return NotFound("Invitation not found.");
        }

        return Ok(new { Message = "Invitation updated successfully." });
    }

    [Authorize(Roles = "Organization")]
    [HttpDelete("DeleteInvitation/{id:int}")]
    public async Task<IActionResult> DeleteInvitation(int id)
    {
        var result = await _invitationService.DeleteInvitationAsync(id);
        if (result == null)
        {
            return NotFound("Invitation not found.");
        }

        return Ok(new { Message = "Invitation deleted successfully." });
    }
}