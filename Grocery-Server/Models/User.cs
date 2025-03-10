using Grocery_Server.Controllers.UserController;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace Grocery_Server.Models;

public class User : IdentityUser
{
    [Required]
    public DateTime JoinTime { get; set; }

    public Guid? GroupId { get; set; }

    public virtual Group? Group { get; set; }
    public virtual ICollection<GroupInvite> Invites { get; set; }

    public User(NewUserDTO dto)
    {
        JoinTime = DateTime.UtcNow;
        UserName = dto.UserName;
        NormalizedUserName = dto.UserName.Normalize();
        GroupId = null;
        Email = dto.Email;
        NormalizedEmail = dto.Email.Normalize();
    }

    // returns whether these have any overlapping unique fields (there is no constraint but they should be in practice)
    public bool CanCoexist(User other)
    {
        return other.UserName == UserName || other.Email == Email;
    }
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    public User() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
}
