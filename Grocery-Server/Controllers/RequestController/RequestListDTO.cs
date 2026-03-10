using Grocery_Server.Models;

namespace Grocery_Server.Controllers.RequestController;

public record RequestListItemDTO
{
    public Guid ItemId { get; set; }
    public ushort Quantity { get; set; }

    public RequestListItemDTO(Guid itemId, ushort quantity)
    {
        this.ItemId = itemId;
        this.Quantity = quantity;
    }
    
    public RequestListItemDTO(RequestListItem item)
    {
        ItemId = item.ItemId;
        Quantity = item.Quantity;
    }

    public RequestListItemDTO() { }
}
