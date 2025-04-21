using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace Grocery_Server.Models;

[PrimaryKey(nameof(Id))]
public class Group
{
    public Guid Id { get; set; }

    [Required]
    public string OwnerId { get; set; }

    public string Name { get; set; }

    public DateTime CreationTime { get; set; }

    [AllowNull]
    public virtual User Owner { get; set; }

    [AllowNull]
    public virtual ICollection<User> Members { get; set; }
    [AllowNull]
    public virtual ICollection<GroceryList>? GroceryLists { get; set; }
    [AllowNull]
    public virtual ICollection<GroceryCategory> CustomCategories { get; set; }
    [AllowNull]
    public virtual ICollection<GroupInvite> Invites { get; set; }
    [AllowNull]
    public virtual ICollection<Recipe> Recipes { get; set; }

    public Group(User creator, string name)
    {
        Id = new();
        creator.GroupId = Id;
        OwnerId = creator.Id;
        CreationTime = DateTime.UtcNow;
        Name = name;
    }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    public Group() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

    public string GetString()
    {
        List<User> members = Members.ToList();
        return $"Id: {Id}\nName: '{Name}'\nOwner Id: {OwnerId}\nMembers: {string.Join(",", members)}";
    }
}
