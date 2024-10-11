namespace Grocery_Server.ControllerModels;

public class CreateListDTO
{
    public Guid HouseId { get; set; }

    public List<NewGroceryListItemDTO> Items { get; set; }
}
