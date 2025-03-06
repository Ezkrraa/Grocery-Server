using Microsoft.AspNetCore.Identity;
using Microsoft.Identity.Client;

namespace Grocery_Server.Controllers.UserController;

public class NewUserDTO
{
    public required string UserName { get; set; }
    public required string Password { get; set; }
    public required string Email { get; set; }
}
