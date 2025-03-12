// manages users, lets them create accs, reset passwords, etc
//using Grocery_Server.ControllerModels;
using Grocery_Server.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace Grocery_Server.Controllers.UserController;

[ApiController]
[EnableRateLimiting(nameof(RateLimiters.Fast))]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[Route("api/user")]
public class UserController : ControllerBase
{
    private readonly DbContext _dbContext;
    private readonly UserManager<User> _userManager;

    public UserController([FromServices] DbContext dbContext, UserManager<User> userManager)
    {
        _dbContext = dbContext;
        _userManager = userManager;
    }

    [HttpGet]
    public async Task<IActionResult> GetMyInfo()
    {
        User? user = await GetUser();
        if (user == null)
            throw new Exception("GetUser failed");
        return Ok(new UserDisplayDTO(user));
    }

    [HttpGet("{query}")]
    public IActionResult GetInfoByName(string query)
    {
        IEnumerable<User>? users = _dbContext.Users.Where(user => user.UserName.Contains(query));
        if (users != null)
            return Ok(users.Select(user => new UserDisplayDTO(user)));
        return NotFound("No such user");
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteAccount()
    {
        User? user = await GetUser();
        if (user != null)
        {
            await _userManager.DeleteAsync(user);
            return Ok();
        }
        return NotFound();
    }

    private async Task<User?> GetUser()
    {
        return await _userManager.GetUserAsync(User);
    }
}
