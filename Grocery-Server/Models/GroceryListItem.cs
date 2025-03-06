using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.Identity.Client;

namespace Grocery_Server.Models;

public class GroceryListItem
{
    public Guid ListId { get; set; }

    public Guid ItemId { get; set; }

    public ushort Quantity { get; set; }

    public virtual GroceryList List { get; set; }

    public virtual GroceryItem Item { get; set; }

    public GroceryListItem(Guid listId, Guid itemId, ushort quantity)
    {
        ListId = listId;
        ItemId = itemId;
        Quantity = quantity;
    }

    public GroceryListItem() { }
}
