using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using VolunteerProject.Models;
using VolunteerProject.Services;
using VolunteerProject.Services.Invitation;

namespace VolunteerProject.Controllers
{
    [ApiController]
    [Route("api")]
    public class InvitationController : ControllerBase
    {
        private readonly IInvitationService _invitationService;
        public InvitationController(IInvitationService invitationService)
        {
            _invitationService = invitationService;
        }

        [Authorize(Roles = "Organization")]
        [HttpPost("CreateInvitation")]
        public async Task<IActionResult> CreateInvitation([FromBody] CreateInvitationModel invitation)
        {
            var organizationId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            // Создание объекта мероприятия с использованием идентификатора организации
            var invitationObj = new Invitation
            {
                VolunteerId = invitation.VolunteerId,
                OrganizationId = organizationId
            };

            var createdInvitation = await _invitationService.CreateInvitationAsync(invitationObj);

            return CreatedAtAction(nameof(GetInvitationById), new { id = createdInvitation.Id }, createdInvitation);
        }

        [Authorize(Roles = "Organization")]
        [HttpGet("GetAllInvitations")]
        public async Task<IActionResult> GetAllInvitations()
        {
            var invitations = await _invitationService.GetAllInvitationsAsync();
            return Ok(invitations);
        }

        [Authorize(Roles = "Organization")]
        [HttpGet("GetInvitationById/{id:int}")]
        public async Task<IActionResult> GetInvitationById(int id)
        {
            var invitation = await _invitationService.GetInvitationByIdAsync(id);
            return Ok(invitation);
        }

        [Authorize(Roles = "Organization")]
        [HttpPut("UpdateInvitation{id:int}")]
        public async Task<IActionResult> UpdateInvitation(int id, Invitation updatedInvitation)
        {
            var invitation = await _invitationService.UpdateInvitationAsync(id, updatedInvitation);
            return Ok(invitation);
        }

        [Authorize(Roles = "Organization")]
        [HttpDelete("DeleteInvitation/{id:int}")]
        public async Task<IActionResult> DeleteInvitation(int id)
        {
            var invitation = await _invitationService.DeleteInvitationAsync(id);
            return Ok(invitation);
        }
    }
}
