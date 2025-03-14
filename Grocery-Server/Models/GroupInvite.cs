﻿using Microsoft.EntityFrameworkCore;

namespace Grocery_Server.Models;

public class GroupInvite
{
    public Guid GroupId { get; set; }
    public string UserId { get; set; }
    public string InvitedBy { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime ExpirationTime { get; set; }

    public virtual User User { get; set; }
    public virtual Group Group { get; set; }

    public GroupInvite() { }

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
