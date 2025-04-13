namespace Grocery_Server.Controllers.GroceryListController;

public class CreateListItemDTO
{
    public Guid ItemId { get; set; }
    public ushort Quantity { get; set; }
}
