using Grocery_Server.Controllers;

namespace Grocery_Server.Controllers.ItemController;

public class ItemDetailDisplayDTO
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public DateTime CreatedAt { get; set; }
    public string CategoryName { get; set; }
    public Guid CategoryId { get; set; }

    public ItemDetailDisplayDTO(Models.GroceryItem item)
    {
        Id = item.Id;
        Name = item.ItemName;
        CreatedAt = item.CreationTime;
        CategoryName = item.Category.CategoryName;
        CategoryId = item.CategoryId;
    }
}
