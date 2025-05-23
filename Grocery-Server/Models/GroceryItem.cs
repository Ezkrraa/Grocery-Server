using Grocery_Server.Controllers.ItemController;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace Grocery_Server.Models;

[PrimaryKey(nameof(Id))]
public class GroceryItem
{
    public Guid Id { get; set; }

    // ItemName does not need to be unique, multiple families can have their custom cheeses
    public string ItemName { get; set; }
    public Guid CategoryId { get; set; }
    public DateTime CreationTime { get; set; }
    public DateTime LastUsed { get; set; }

    [AllowNull]
    public virtual GroceryCategory Category { get; set; }
    [AllowNull]
    public virtual List<RecipeItem> RecipeItems { get; set; }

    public GroceryItem(NewItemDTO newItem)
    {
        Id = new();
        ItemName = newItem.Name;
        CategoryId = newItem.CategoryId;
        CreationTime = DateTime.UtcNow;
        LastUsed = DateTime.UtcNow;
    }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    public GroceryItem() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
}
