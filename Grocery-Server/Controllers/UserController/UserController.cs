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
    public async Task<IActionResult> GetMyInfo()
    {
        User? user = await GetUser();
        if (user == null)
            throw new Exception("GetUser failed");
        return Ok(new UserDisplayDTO(user));
    }

    [HttpGet("{query}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public IActionResult GetInfoByName(string query)
    {
        IEnumerable<User>? users = _dbContext.Users.Where(user => user.UserName.Contains(query));
        if (users != null)
            return Ok(users.Select(user => new UserDisplayDTO(user)));
        return NotFound("No such user");
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreateAccount([FromBody] NewUserDTO newUser)
    {
        if (
            await _userManager.FindByEmailAsync(newUser.Email) == null
            && await _userManager.FindByNameAsync(newUser.UserName) == null
        )
        {
            foreach (IPasswordValidator<User> validator in _userManager.PasswordValidators)
            {
                IdentityResult result = await validator.ValidateAsync(_userManager, null, newUser.Password);
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
            return Ok();
        }
        return BadRequest("User already exists");
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
