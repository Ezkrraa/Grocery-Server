using Grocery_Server.Models;

namespace Grocery_Server.Controllers.GroceryListController;

public class GroceryListDisplayDTO
{
    public Guid ListId { get; set; }
    public DateTime CreatedTime { get; set; }
    public int ItemsCount { get; set; }

    public GroceryListDisplayDTO(GroceryList list)
    {
        ListId = list.Id;
        CreatedTime = list.CreationTime;
        ItemsCount = list.GroceryListItems?.Count ?? 0;
    }
}
