using Grocery_Server.ControllerModels;
using Grocery_Server.Models;
using Grocery_Server.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Grocery_Server.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly JwtService _jwtService;
        private readonly SignInManager<User> _signInManager;
        public AuthController(UserManager<User> userManager, JwtService jwtService, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _jwtService = jwtService;
            _signInManager = signInManager;
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO model)
        {
            User? user = await _userManager.FindByNameAsync(model.UserName);
            if (user == null)
                return NotFound();

            Microsoft.AspNetCore.Identity.SignInResult result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);

            if (result.Succeeded)
            {
                string token = _jwtService.GenerateToken(user);
                return Ok(new { token });
            }

            return NotFound();
        }


        [Authorize]
        [HttpPost("change-password")]
        public IActionResult ChangePassword([FromBody] string newPassword)
        {
            throw new NotImplementedException();
        }
    }
}
