﻿namespace Grocery_Server.Controllers.Auth;

public class LoginDTO
{
    public required string UserName { get; set; }
    public required string Password { get; set; }
}
