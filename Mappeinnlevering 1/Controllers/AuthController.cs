using Mappeinnlevering_1.DTOs;
using Mappeinnlevering_1.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Mappeinnlevering_1.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;

    public AuthController(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    // GET /api/auth/me
    [Authorize]
    [HttpGet("me")]
    public async Task<ActionResult<UserDto>> Me()
    {
        ApplicationUser? user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return Unauthorized();
        }

        IList<string> roles = await _userManager.GetRolesAsync(user);

        UserDto dto = new UserDto();
        dto.Id = user.Id;
        dto.Email = user.Email ?? "";
        dto.FullName = user.FullName;
        dto.Roles = roles.ToList();

        return Ok(dto);
    }
}