using Grocery_Server.Models;
using Grocery_Server.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace Grocery_Server.Controllers.AuthController;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly UserManager<User> _userManager;
    private readonly JwtService _jwtService;
    private readonly SignInManager<User> _signInManager;
    private readonly ImageStorageService _imageStorageService;
    private readonly DbContext _dbContext;
    private readonly IConfiguration _config;

    private static readonly string[] validImageFileExtension = new string[] { "image/png", "image/jpeg", "image/jpg" };

    public AuthController(
        UserManager<User> userManager,
        JwtService jwtService,
        SignInManager<User> signInManager,
        ImageStorageService imageStorageService,
        DbContext dbContext,
        IConfiguration config
    )
    {
        _userManager = userManager;
        _jwtService = jwtService;
        _signInManager = signInManager;
        _imageStorageService = imageStorageService;
        _dbContext = dbContext;
        _config = config;
    }

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpPost("login")]
    [EnableRateLimiting(nameof(RateLimiters.Slow))]
    public async Task<IActionResult> Login([FromBody] LoginDTO model)
    {
        User? user = await _userManager.FindByNameAsync(model.UserName);
        user ??= await _userManager.FindByEmailAsync(model.UserName);

        if (user == null)
            return NotFound();
        Microsoft.AspNetCore.Identity.SignInResult result =
            await _signInManager.CheckPasswordSignInAsync(user, model.Password, true);

        if (result.Succeeded)
        {
            string token = _jwtService.GenerateToken(user, _config);
            return Ok(token);
        }

        return NotFound();
    }

    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [EnableRateLimiting(nameof(RateLimiters.Slow))]
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

    [EnableRateLimiting(nameof(RateLimiters.ReallySlow))]
    [HttpPost("create")]
    public async Task<IActionResult> CreateAccount([FromForm] NewUserDTO newUser)
    {
        if (newUser.ProfilePicture == null)
            return BadRequest("Invalid picture");
        else if (!validImageFileExtension.Any(imageExtension => newUser.ProfilePicture.ContentType == imageExtension))
            return BadRequest($"Invalid picture type: '{newUser.ProfilePicture.ContentType}'");

        if (
            await _userManager.FindByEmailAsync(newUser.Email) != null
            || await _userManager.FindByNameAsync(newUser.UserName) != null
        )
            return BadRequest("User already exists");
        if (!_imageStorageService.IsValidProfilePicture(newUser.ProfilePicture))
            return BadRequest("Posted an invalid profile picture");

        foreach (IPasswordValidator<User> validator in _userManager.PasswordValidators)
        {
            // using Identity methods to verify if a password meets our criteria
            IdentityResult result = await validator.ValidateAsync(_userManager, null!, newUser.Password);
            if (!result.Succeeded)
                return BadRequest(result.Errors.First().Description);
        }
        User user = new(newUser);
        IdentityResult createResult = await _userManager.CreateAsync(user);
        if (!createResult.Succeeded)
            return BadRequest(createResult.Errors.First());
        IdentityResult passwordResult = await _userManager.AddPasswordAsync(user, newUser.Password);

        if (!passwordResult.Succeeded)
            return BadRequest(createResult.Errors.First());

        string profilePictureFileName = _imageStorageService.SaveImage(newUser.ProfilePicture);
        _dbContext.ProfilePictures.Add(new ProfilePicture(user.Id, DateTime.UtcNow, profilePictureFileName));
        _dbContext.SaveChanges();

        return Ok();
    }

    [EnableRateLimiting(nameof(RateLimiters.Fast))]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [HttpGet("check-token")]
    public async Task<IActionResult> CheckToken()
    {
        return await _userManager.GetUserAsync(User) != null ? Ok() : Unauthorized();
    }
}
