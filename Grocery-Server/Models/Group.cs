using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Grocery_Server.Models;

[PrimaryKey(nameof(Id))]
public class Group
{
    public Guid Id { get; set; }

    [Required]
    public string OwnerId { get; set; }

    public string Name { get; set; }

    public DateTime CreationTime { get; set; }

    public virtual User Owner { get; set; }

    public virtual ICollection<User> Members { get; set; }
    public virtual ICollection<GroceryList>? GroceryLists { get; set; }
    public virtual ICollection<GroceryCategory> CustomCategories { get; set; }
    public virtual ICollection<GroupInvite> Invites { get; set; }
    public virtual ICollection<Recipe> Recipes { get; set; }

    public Group(User creator, string name)
    {
        Id = new();
        creator.GroupId = Id;
        OwnerId = creator.Id;
        CreationTime = DateTime.UtcNow;
        Name = name;
    }

    public Group() { }

    public string GetString()
    {
        List<User> members = Members.ToList();
        return $"Id: {Id}\nName: '{Name}'\nOwner Id: {OwnerId}\nMembers: {string.Join(",", members)}";
    }
}
