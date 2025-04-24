// manages users, lets them create accs, reset passwords, etc
//using Grocery_Server.ControllerModels;
using Grocery_Server.Models;
using Grocery_Server.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using System.Transactions;

namespace Grocery_Server.Controllers.UserController;

[ApiController]
[EnableRateLimiting(nameof(RateLimiters.Fast))]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[Route("api/user")]
public class UserController : ControllerBase
{
    private readonly ILogger<UserController> _logger;
    private readonly DbContext _dbContext;
    private readonly UserManager<User> _userManager;
    private readonly ImageStorageService _imageStorageService;

    public UserController([FromServices] ILogger<UserController> logger, DbContext dbContext, UserManager<User> userManager, ImageStorageService imageStorageService)
    {
        _logger = logger;
        _dbContext = dbContext;
        _userManager = userManager;
        _imageStorageService = imageStorageService;
    }

    [HttpGet]
    public async Task<IActionResult> GetMyInfo()
    {
        User? user = await GetUser();
        if (user == null)
            return Unauthorized();

        return Ok(new UserDisplayDTO(user));
    }

    [HttpGet("{query}")]
    public IActionResult GetInfoByName(string query)
    {
        query = query.Normalize().ToUpper();
        IEnumerable<User>? users = _dbContext.Users.Where(user => user.NormalizedUserName!.Contains(query));
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
            if (user.Group != null && user.Group.Owner == user)
            {
                if (user.Group.Members.Count >= 2)
                {
                    // assign a new owner
                    user.Group.Owner = user.Group.Members.First(groupUser => groupUser.Id != user.Id);
                    _dbContext.Update(user.Group);
                }
                else
                {
                    // delete group
                    user.Group.Owner = null;
                    _dbContext.Update(user.Group);
                    _dbContext.SaveChanges();
                    _dbContext.Remove(user.Group);
                    _dbContext.SaveChanges();
                }
            }
            _dbContext.Remove(user);
            _dbContext.SaveChanges();
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
