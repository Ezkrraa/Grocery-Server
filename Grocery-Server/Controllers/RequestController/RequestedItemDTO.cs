namespace Grocery_Server.Controllers.RequestController;

public record RequestedItemDTO
{
    public Guid ItemId { get; set; }
    public string Name { get; set; }
    public int Quantity { get; set; }

    public RequestedItemDTO(Guid itemId, string name, int quantity)
    {
        ItemId = itemId;
        Name = name;
        Quantity = quantity;
    }
}
