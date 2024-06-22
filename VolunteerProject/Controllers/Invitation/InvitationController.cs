using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using VolunteerProject.Models;
using VolunteerProject.Services;

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

            return CreatedAtAction(nameof(GetInvitationById), new { id = createdInvitation.IdInv }, createdInvitation);
        }

        [Authorize(Roles = "Organization")]
        [HttpGet("GetAllInvitations")]
        public async Task<IActionResult> GetAllInvitations()
        {
            var Invitations = await _invitationService.GetAllInvitationsAsync();
            return Ok(Invitations);
        }

        [Authorize(Roles = "Organization")]
        [HttpGet("GetInvitationById{id}")]
        public async Task<IActionResult> GetInvitationById(int id)
        {
            var Invitation = await _invitationService.GetInvitationByIdAsync(id);
            return Ok(Invitation);
        }

        [Authorize(Roles = "Organization")]
        [HttpPut("UpdateInvitation{id}")]
        public async Task<IActionResult> UpdateInvitation(int id, Invitation updatedInvitation)
        {
            var Invitation = await _invitationService.UpdateInvitationAsync(id, updatedInvitation);
            return Ok(Invitation);
        }

        [Authorize(Roles = "Organization")]
        [HttpDelete("DeleteInvitation{id}")]
        public async Task<IActionResult> DeleteInvitation(int id)
        {
            var Invitation = await _invitationService.DeleteInvitationAsync(id);
            return Ok(Invitation);
        }
    }
}
