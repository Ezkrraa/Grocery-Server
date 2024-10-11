using Grocery_Server.Models;

namespace Grocery_Server.ControllerModels;

public class GroceryListDisplayDTO
{
    public DateTime CreatedTime { get; set; }
    public int ItemsCount { get; set; }
    public GroceryListDisplayDTO(GroceryList list)
    {
        CreatedTime = list.CreationTime;
        ItemsCount = list.GroceryListItems.Count;
    }
}
