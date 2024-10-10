using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace Grocery_Server.Models;

public class User : IdentityUser
{

    [Required]
    public DateTime? JoinTime { get; set; }

    public Guid? HouseHoldId { get; set; }

    public virtual HouseHold? HouseHold { get; set; }

    public User(ControllerModels.NewUserDTO dto)
    {
        JoinTime = DateTime.UtcNow;
        UserName = dto.UserName;
        NormalizedUserName = dto.UserName.Normalize();
        HouseHoldId = null;
        Email = dto.Email;
        NormalizedEmail = dto.Email.Normalize();
    }

    // returns whether these have any overlapping unique fields (there is no constraint but they should be in practice)
    public bool CanCoexist(User other)
    {
        return other.UserName == UserName || other.Email == Email;
    }

    public User() { }
}
