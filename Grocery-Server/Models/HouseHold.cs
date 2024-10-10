using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Grocery_Server.Models;

[PrimaryKey(nameof(Id))]
public class HouseHold
{
    public Guid Id { get; set; }

    [Required]
    public string OwnerId { get; set; }

    public string Name { get; set; }

    public DateTime CreationTime { get; set; }

    public virtual User Owner { get; set; }

    // let's hope it understands this is suppposed to be a foreign key
    public virtual ICollection<User> Members { get; set; }

    public virtual ICollection<GroceryList>? GroceryLists { get; set; }

    public virtual ICollection<GroceryCategory>? CustomCategories { get; set; }

    public HouseHold(ControllerModels.NewHouseHoldDTO model)
    {
        Id = new();
        OwnerId = model.UserId;
        Name = model.Name;
        CreationTime = DateTime.UtcNow;
    }

    public HouseHold(User creator, string name)
    {
        Id = new();
        creator.HouseHoldId = Id;
        OwnerId = creator.Id;
        CreationTime = DateTime.UtcNow;
        Name = name;
    }

    public HouseHold() { }

    public string GetString()
    {
        List<User> members = Members.ToList();
        return $"Id: {Id}\nName: '{Name}'\nOwner Id: {OwnerId}\nMembers: {string.Join(",", members)}";
    }
}
