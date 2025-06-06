using Grocery_Server.Controllers.UserController;
using Grocery_Server.Services;

namespace Grocery_Server.Controllers.GroupController;

public record GroupDisplayDTO
{
    public Guid? Id { get; set; }
    public IEnumerable<UserDisplayDTO> Members { get; set; }
    public DateTime CreatedAt { get; set; }
    public string Owner { get; set; }

    public GroupDisplayDTO(Models.Group group, ImageStorageService imageStorageService)
    {
        Id = group.Id;
        Members = group.Members.Select(member => new UserDisplayDTO(member));
        CreatedAt = group.CreationTime;
        Owner = group.Owner?.UserName ?? group.Owner?.ToString() ?? "No owner";
    }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    public GroupDisplayDTO() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
}
