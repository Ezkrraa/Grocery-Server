namespace Grocery_Server.ControllerModels;

public class ItemDisplayDTO
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public Guid CategoryId { get; set; }
    public int Quantity { get; set; }
    public ItemDisplayDTO(Models.GroceryListItem item)
    {
        Id = item.Item.Id;
        Name = item.Item.ItemName;
        CategoryId = item.Item.CategoryId;
        Quantity = item.Quantity;
    }

    public ItemDisplayDTO(Models.GroceryItem item)
    {
        Id = item.Id;
        Name = item.ItemName;
        CategoryId = item.CategoryId;
        Quantity = 0;
    }
}
