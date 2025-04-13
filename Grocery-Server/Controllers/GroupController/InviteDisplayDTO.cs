using Grocery_Server.Models;

namespace Grocery_Server.Controllers.GroupController;

public class InviteDisplayDTO
{
    public Guid GroupId { get; set; }
    public string UserId { get; set; }
    public string UserName { get; set; }
    public string GroupName { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime ExpiresAt { get; set; }
    public int GroupMemberCount { get; set; }

    public InviteDisplayDTO(GroupInvite invite)
    {
        GroupId = invite.GroupId;
        UserId = invite.UserId;
        UserName = invite.User?.UserName ?? "";
        GroupName = invite.Group.Name;
        CreatedAt = invite.CreatedAt;
        ExpiresAt = invite.ExpirationTime;
        GroupMemberCount = invite.Group.Members.Count;
    }

    public InviteDisplayDTO() { }
}
