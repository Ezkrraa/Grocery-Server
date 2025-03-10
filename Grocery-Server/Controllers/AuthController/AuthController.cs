//using Grocery_Server.ControllerModels;
using Grocery_Server.Models;
using Grocery_Server.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Grocery_Server.Controllers.Auth;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly UserManager<User> _userManager;
    private readonly JwtService _jwtService;
    private readonly SignInManager<User> _signInManager;
    private readonly IConfiguration _config;

    public AuthController(
        UserManager<User> userManager,
        JwtService jwtService,
        SignInManager<User> signInManager,
        IConfiguration config
    )
    {
        _userManager = userManager;
        _jwtService = jwtService;
        _signInManager = signInManager;
        _config = config;
    }

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDTO model)
    {
        User? user = await _userManager.FindByNameAsync(model.UserName);
        user ??= await _userManager.FindByEmailAsync(model.UserName);

        if (user == null)
            return NotFound();
        Microsoft.AspNetCore.Identity.SignInResult result =
            await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);

        if (result.Succeeded)
        {
            string token = _jwtService.GenerateToken(user, _config);
            return Ok(token);
        }

        return NotFound();
    }

    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [HttpPost("change-password")]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDTO passwordCombo)
    {
        User? user = await _userManager.GetUserAsync(User);
        if (user == null)
            return NotFound();
        IdentityResult result = await _userManager.ChangePasswordAsync(
            user,
            passwordCombo.Password,
            passwordCombo.NewPassword
        );
        if (!result.Succeeded)
            return BadRequest();
        return Ok();
    }

    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [HttpGet("check-token")]
    public IActionResult CheckToken()
    {
        return Ok();
    }
}
