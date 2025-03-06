namespace Grocery_Server.Controllers.ItemController;

public class NewItemDTO
{
    public Guid CategoryId { get; set; }
    public required string Name { get; set; }
}
