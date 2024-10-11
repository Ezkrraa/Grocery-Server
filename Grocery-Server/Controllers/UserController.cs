// manages users, lets them create accs, reset passwords, etc
using Grocery_Server.ControllerModels;
using Grocery_Server.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;
using System.IdentityModel.Tokens.Jwt;
using System.Net;

namespace Grocery_Server.Controllers
{
    [ApiController]
    [Route("api/user")]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class UserController : ControllerBase
    {
        private readonly DbContext _dbContext;
        public UserController([FromServices] DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public IActionResult GetInfo([FromQuery] Guid id)
        {
            string idStr = id.ToString();
            return Ok(_dbContext.Users.FirstOrDefault(user => user.Id == idStr));
        }

        [HttpPost("create")]
        public IActionResult CreateAccount([FromBody] NewUserDTO newUserDTO)
        {
            return Ok();
        }

        [HttpDelete]
        public IActionResult DeleteAccount([FromBody] string id)
        {
            User? user = _dbContext.Users.FirstOrDefault(user => user.Id == id);
            if (user == null)
                return NotFound();
            _dbContext.Users.Remove(user);
            _dbContext.SaveChanges();
            return Ok();
        }
    }
}