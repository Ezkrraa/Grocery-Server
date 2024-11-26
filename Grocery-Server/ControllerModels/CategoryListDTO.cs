namespace Grocery_Server.ControllerModels;

public class CategoryListDTO
{
    public Guid Id { get; set; }
    public string CategoryName { get; set; }
    public ICollection<ItemDisplayDTO> Items { get; set; }
    public CategoryListDTO(Models.GroceryCategory category)
    {
        Id = category.Id;
        CategoryName = category.CategoryName;
        Items = category.Items.Select(item => new ItemDisplayDTO(item)).ToList();
    }
    public CategoryListDTO() { }
}
