using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MakeVolunteerGreatAgain.Core.Services;
using MakeVolunteerGreatAgain.Core.Repositories.DTO;
using MakeVolunteerGreatAgain.Core.Entities;
using Microsoft.AspNetCore.Authorization;

namespace MakeVolunteerGreatAgain.Infrastructure.Controllers
{
    [ApiController]
    [EnableCors("AllowSpecificOrigin")]
    [Route("api")]
    public class AccountController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly UserManager<CommonUser> _userManager;

        public AccountController(IAuthService authService, UserManager<CommonUser> userManager)
        {
            _authService = authService;
            _userManager = userManager;
        }

        [HttpPost("register-volunteer")]
        public async Task<IActionResult> RegisterVolunteer([FromBody] RegisterVolunteerDTO model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _authService.RegisterVolunteerAsync(model);

            if (result.Success)
            {
                return Ok(new { Message = "Volunteer registered successfully" });
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error);
            }

            return BadRequest(ModelState);
        }

        [HttpPost("register-organization")]
        public async Task<IActionResult> RegisterOrganization([FromBody] RegisterOrganizationDTO model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _authService.RegisterOrganizationAsync(model);

            if (result.Success)
            {
                return Ok(new { Message = "Organization registered successfully" });
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error);
            }

            return BadRequest(ModelState);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _authService.LoginAsync(model);

            if (result.Success)
            {
                // Сохранение токена в куки
                Response.Cookies.Append("jwt", result.Token, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict
                });

                return Ok(new { Message = $"Login successful. Token: {result.Token}" });
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error);
            }

            return BadRequest(ModelState);
        }

        [HttpPut("update-volunteer")]
        public async Task<IActionResult> UpdateVolunteer([FromBody] UpdateVolunteerDTO model, int volunteerCommonUserId)
        {
            var volunteer = await _authService.UpdateVolunteerAsync(model, volunteerCommonUserId);
            return Ok(volunteer);
        }
        
        [Authorize(Roles = "Organization")]
        [HttpPut("update-organization")]
        public async Task<IActionResult> UpdateOrganization([FromBody] UpdateOrganizationDTO model, int organizationCommonUserId)
        {
            var organization = await _authService.UpdateOrganizationAsync(model, organizationCommonUserId);
            return Ok(organization);
        }
        
    }
}
