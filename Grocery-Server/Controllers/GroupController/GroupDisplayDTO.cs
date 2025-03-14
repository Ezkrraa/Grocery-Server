using Grocery_Server.Controllers.UserController;

namespace Grocery_Server.Controllers.GroupController;

public record GroupDisplayDTO
{
    public Guid? Id { get; set; }
    public IEnumerable<UserDisplayDTO> Members { get; set; }
    public DateTime CreatedAt { get; set; }
    public string Owner { get; set; }

    public GroupDisplayDTO(Models.Group group)
    {
        Id = group.Id;
        Members = group.Members.Select(member => new UserDisplayDTO(member));
        CreatedAt = group.CreationTime;
        Owner = group.Owner.UserName ?? group.Owner.ToString();
    }

    public GroupDisplayDTO() { }
}
