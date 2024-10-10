using Microsoft.Identity.Client;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Grocery_Server.Models;

public class GroceryListItem
{
    public Guid ListId { get; set; }

    public Guid ItemId { get; set; }

    public short Quantity { get; set; }

    public virtual GroceryList List { get; set; }

    public virtual GroceryItem Item { get; set; }

    public GroceryListItem(Guid listId, Guid itemId)
    {
        ListId = listId;
        ItemId = itemId;
    }
    public GroceryListItem() { }
}
