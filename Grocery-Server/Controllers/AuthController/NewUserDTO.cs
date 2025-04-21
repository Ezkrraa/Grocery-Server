using Microsoft.AspNetCore.Identity;
using Microsoft.Identity.Client;

namespace Grocery_Server.Controllers.AuthController;

public class NewUserDTO
{
    public required string UserName { get; set; }
    public required string Password { get; set; }
    public required string Email { get; set; }
    public required IFormFile? ProfilePicture { get; set; }
}
