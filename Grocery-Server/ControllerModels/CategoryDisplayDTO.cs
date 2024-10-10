namespace Grocery_Server.ControllerModels;

public record CategoryDisplayDTO
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public CategoryDisplayDTO(Models.GroceryCategory category)
    {
        Id = category.Id;
        Name = category.CategoryName;
    }
    public CategoryDisplayDTO() { }
}
