using Grocery_Server.Models;

namespace Grocery_Server.Controllers.GroupController;

public class NewInviteDTO
{
    public Guid GroupId { get; set; }
    public string UserId { get; set; }

    public NewInviteDTO(Guid groupId, string userId)
    {
        GroupId = groupId;
        UserId = userId;
    }

    public NewInviteDTO(GroupInvite invite)
    {
        GroupId = invite.GroupId;
        UserId = invite.UserId;
    }

    public NewInviteDTO() { }

    public GroupInvite GetInvite()
    {
        return new GroupInvite
        {
            CreatedAt = DateTime.UtcNow,
            ExpirationTime = DateTime.UtcNow.AddDays(7),
            GroupId = GroupId,
            UserId = UserId,
        };
    }
}
