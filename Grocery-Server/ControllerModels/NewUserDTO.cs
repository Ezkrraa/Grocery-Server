using Microsoft.AspNetCore.Identity;
using Microsoft.Identity.Client;

namespace Grocery_Server.ControllerModels;

public class NewUserDTO
{
    public string UserName { get; set; }
    public string Password { get; set; }
    public string Email { get; set; }
}
