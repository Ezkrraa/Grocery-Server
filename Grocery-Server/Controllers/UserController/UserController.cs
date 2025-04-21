// manages users, lets them create accs, reset passwords, etc
//using Grocery_Server.ControllerModels;
using Grocery_Server.Models;
using Grocery_Server.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Grocery_Server.Controllers.UserController;

[ApiController]
[EnableRateLimiting(nameof(RateLimiters.Fast))]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[Route("api/user")]
public class UserController : ControllerBase
{
    private readonly DbContext _dbContext;
    private readonly UserManager<User> _userManager;
    private readonly ImageStorageService _imageStorageService;

    public UserController([FromServices] DbContext dbContext, UserManager<User> userManager, ImageStorageService imageStorageService)
    {
        _dbContext = dbContext;
        _userManager = userManager;
        _imageStorageService = imageStorageService;
    }

    [HttpGet]
    public async Task<IActionResult> GetMyInfo()
    {
        User? user = await GetUser();
        if (user == null)
            return Unauthorized(
#if DEBUG
                JsonSerializer.Serialize(User)
#endif
        );

        return Ok(new UserDisplayDTO(user));
    }

    [HttpGet("{query}")]
    public IActionResult GetInfoByName(string query)
    {
        IEnumerable<User>? users = _dbContext.Users.Where(user => user.UserName.ToLower().Contains(query.ToLower()));
        if (users != null)
            return Ok(users.Select(user => new UserDisplayDTO(user)));
        return NotFound("No such user");
    }

    [EnableRateLimiting(nameof(RateLimiters.ReallySlow))]
    [HttpDelete]
    public async Task<IActionResult> DeleteAccount()
    {
        // throws sometimes?
        // hypothesis: it doesn't work when there's images uploaded
        // TODO: implement deletion of related images
        // TODO: test if that fixed it
        User? user = await GetUser();
        if (user != null)
        {
            await _userManager.DeleteAsync(user);
            return Ok();
        }
        return NotFound();
    }

    [HttpGet("picture")]
    public ActionResult<IFormFile> GetPicture(string fileName)
    {
        ProfilePicture? pfp = _dbContext.ProfilePictures.FirstOrDefault(pfp => pfp.FileName == fileName);
        if (pfp == null)
            return NotFound("No such profile picture is known");
        else
        {
            try
            {
                FileStream image = _imageStorageService.GetImage(pfp.FileName);
                return Ok(image);

            }
            catch (FileNotFoundException)
            {
                return NotFound("Error loading from storage");
            }
        }
    }

    private async Task<User?> GetUser()
    {
        return await _userManager.GetUserAsync(User);
    }
}
