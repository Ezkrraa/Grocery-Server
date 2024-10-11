namespace Grocery_Server.ControllerModels;

public class NewGroceryItemDTO
{
    public Guid CategoryId { get; set; }
    public string Name { get; set; }
    public int Quantity { get; set; }
}
