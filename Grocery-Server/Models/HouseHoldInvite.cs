using Microsoft.EntityFrameworkCore;

namespace Grocery_Server.Models;

public class HouseHoldInvite
{
    public Guid HouseholdId { get; set; }
    public string UserId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime ExpirationTime { get; set; }

    public virtual User User { get; set; }
    public virtual HouseHold HouseHold { get; set; }

    public HouseHoldInvite() { }
    public HouseHoldInvite(string userId, Guid householdId)
    {
        UserId = userId;
        HouseholdId = householdId;
        CreatedAt = DateTime.UtcNow;
        ExpirationTime = DateTime.UtcNow.AddDays(14);
    }


    public bool IsExpired()
    {
        return ExpirationTime < DateTime.UtcNow;
    }
}
