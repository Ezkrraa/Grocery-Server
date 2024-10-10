namespace Grocery_Server.ControllerModels;

public class ItemDisplayDTO
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public ItemDisplayDTO(Models.GroceryItem item)
    {
        Id = item.Id;
        Name = item.ItemName;
    }
}
