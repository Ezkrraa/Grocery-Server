using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;

namespace Grocery_Server.Models;

public class GroupInvite
{
    public Guid GroupId { get; set; }
    public string UserId { get; set; }
    public string InvitedBy { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime ExpirationTime { get; set; }

    [AllowNull]
    public virtual User User { get; set; }
    [AllowNull]
    public virtual Group Group { get; set; }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    public GroupInvite() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

    public GroupInvite(string userId, Guid groupId, string invitedBy)
    {
        UserId = userId;
        GroupId = groupId;
        CreatedAt = DateTime.UtcNow;
        ExpirationTime = DateTime.UtcNow.AddDays(14);
        InvitedBy = invitedBy;
    }

    public bool IsExpired()
    {
        return ExpirationTime < DateTime.UtcNow;
    }
}
