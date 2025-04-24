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
        Name = user.UserName ?? "Name not found";
        JoinedAt = user.JoinTime;
        IsInGroup = user.GroupId != null;
        ProfilePictureName = user.ProfilePicture?.FileName ?? "";
    }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    public UserDisplayDTO() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
}
