using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace Grocery_Server.Models;

[PrimaryKey(nameof(ListId), nameof(ItemId))]
public class GroceryListItem
{
    public Guid ListId { get; set; }

    public Guid ItemId { get; set; }

    public ushort Quantity { get; set; }

    [AllowNull]
    public virtual GroceryList List { get; set; }

    [AllowNull]
    public virtual GroceryItem Item { get; set; }

    public GroceryListItem(Guid listId, Guid itemId, ushort quantity)
    {
        ListId = listId;
        ItemId = itemId;
        Quantity = quantity;
    }

    public GroceryListItem() { }
}
