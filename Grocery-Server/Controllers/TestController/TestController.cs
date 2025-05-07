using Grocery_Server.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace Grocery_Server.Controllers.TestController;

#if DEBUG
[Route("api/[controller]")]
[ApiController]

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class TestController : ControllerBase
{
    [HttpGet()]
    public async Task<IActionResult> GetClaims()
    {
        return Ok(JsonSerializer.Serialize(User.Claims.Select(claim => claim.ToString())));
    }
}


#endif