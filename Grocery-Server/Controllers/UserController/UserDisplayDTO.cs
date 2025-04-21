using Grocery_Server.Services;
using Microsoft.Identity.Client;

namespace Grocery_Server.Controllers.UserController;

public record UserDisplayDTO
{
    public string Id { get; set; }
    public string Name { get; set; }
    public DateTime JoinedAt { get; set; }
    public bool IsInGroup { get; set; }
    public string ProfilePictureName { get; set; }

    public UserDisplayDTO(Models.User user)
    {
        Id = user.Id;
        Name = user.UserName;
        JoinedAt = user.JoinTime;
        IsInGroup = user.GroupId != null;
        ProfilePictureName = user.ProfilePicture.FileName;
    }

    public UserDisplayDTO() { }
}
