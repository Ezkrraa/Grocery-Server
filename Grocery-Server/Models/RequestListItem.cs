using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace Grocery_Server.Models;

[PrimaryKey(nameof(GroupId), nameof(ItemId))]
public class RequestListItem
{
    public Guid GroupId { get; set; }

    public Guid ItemId { get; set; }

    public ushort Quantity { get; set; }

    [AllowNull]
    public virtual Group Group { get; set; }

    [AllowNull]
    public virtual GroceryItem Item { get; set; }

    public RequestListItem(Guid groupId, Guid itemId, ushort quantity)
    {
        GroupId = groupId;
        ItemId = itemId;
        Quantity = quantity;
    }

    public RequestListItem() { }
}
