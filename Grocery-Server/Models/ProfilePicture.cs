using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace Grocery_Server.Models;

[PrimaryKey(nameof(UserId))]
public class ProfilePicture
{
    public string UserId { get; set; }
    public DateTime TakenAt { get; set; }
    public string FileName { get; set; }

    [AllowNull]
    public virtual User User { get; set; }

    public ProfilePicture(string userId, DateTime takenAt, string fileName)
    {
        UserId = userId;
        TakenAt = takenAt;
        FileName = fileName;
    }
}
