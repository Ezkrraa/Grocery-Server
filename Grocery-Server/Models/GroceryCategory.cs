using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Grocery_Server.Models;

[PrimaryKey(nameof(Id))]
public class GroceryCategory
{
    public Guid Id { get; set; }

    public Guid GroupId { get; set; }
    public string CategoryName { get; set; }

    public virtual Group Group { get; set; }

    public virtual ICollection<GroceryItem> Items { get; set; }

    public GroceryCategory(string name, Guid groupId)
    {
        CategoryName = name;
        GroupId = groupId;
    }

    public GroceryCategory() { }
}
