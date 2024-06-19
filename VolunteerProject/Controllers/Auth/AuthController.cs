using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using VolunteerProject.Models;
using VolunteerProject.Models.Auth;
using VolunteerProject.Services;

[ApiController]
[EnableCors("AllowSpecificOrigin")]
[Route("api")]
public class AccountController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly UserManager<User> _userManager;

    public AccountController(IAuthService authService, UserManager<User> userManager)
    {
        _authService = authService;
        _userManager = userManager;
    }

    [HttpPost("register-volunteer")]
    public async Task<IActionResult> RegisterVolunteer([FromBody] RegisterModelVolunteer model)
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
    public async Task<IActionResult> RegisterOrganization([FromBody] RegisterModelOrganization model)
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
    public async Task<IActionResult> Login([FromBody] LoginModel model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = await _authService.LoginAsync(model);

        if (result.Success)
        {
            return Ok(new { Message = "Login successful" });
        }

        foreach (var error in result.Errors)
        {
            ModelState.AddModelError(string.Empty, error);
        }

        return BadRequest(ModelState);
    }

}
