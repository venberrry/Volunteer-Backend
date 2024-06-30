using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;
using MakeVolunteerGreatAgain.Core.DTOs;
using MakeVolunteerGreatAgain.Core.Entities;
using MakeVolunteerGreatAgain.Core.Services;

namespace MakeVolunteerGreatAgain.Infrastructure.Controllers
{
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
            return Ok(invitation);
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
            return Ok(result);
        }
    }
}
