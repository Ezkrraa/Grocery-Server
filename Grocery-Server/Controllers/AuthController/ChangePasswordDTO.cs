namespace Grocery_Server.Controllers.Auth;

public record ChangePasswordDTO(string Pw, string NewPw)
{
    public string Password { get; set; } = Pw;
    public string NewPassword { get; set; } = NewPw;
}
