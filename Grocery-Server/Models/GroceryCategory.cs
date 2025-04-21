using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace Grocery_Server.Models;

[PrimaryKey(nameof(Id))]
public class GroceryCategory
{
    public Guid Id { get; set; }

    public Guid GroupId { get; set; }
    public string CategoryName { get; set; }

    [AllowNull]
    public virtual Group Group { get; set; }

    [AllowNull]
    public virtual ICollection<GroceryItem> Items { get; set; }

    public GroceryCategory(string name, Guid groupId)
    {
        CategoryName = name;
        GroupId = groupId;
    }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    public GroceryCategory() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
}
