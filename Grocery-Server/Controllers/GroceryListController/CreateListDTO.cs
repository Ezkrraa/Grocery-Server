namespace Grocery_Server.Controllers.GroceryListController;

public class CreateListDTO
{
    public required List<AddToListItemDTO> Items { get; set; }
}
