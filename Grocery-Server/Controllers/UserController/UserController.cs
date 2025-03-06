// manages users, lets them create accs, reset passwords, etc
//using Grocery_Server.ControllerModels;
using Grocery_Server.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Grocery_Server.Controllers.UserController;

[ApiController]
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
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public IActionResult GetInfo([FromQuery] Guid id)
    {
        User? user = _dbContext.Users.FirstOrDefault(user => user.Id == id.ToString());
        if (user != null)
            return Ok(new UserDisplayDTO(user));
        return NotFound();
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreateAccount([FromBody] NewUserDTO newUser)
    {
        if (
            await _userManager.FindByEmailAsync(newUser.Email) == null
            && await _userManager.FindByNameAsync(newUser.UserName) == null
        )
        {
            User user = new(newUser);
            await _userManager.CreateAsync(user);
            await _userManager.AddPasswordAsync(user, newUser.Password);
            return Ok();
        }
        return BadRequest();
    }

    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
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
