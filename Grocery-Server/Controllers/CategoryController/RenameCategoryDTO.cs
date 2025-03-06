namespace Grocery_Server.Controllers.Category;

// for renaming an item or category, has an ID and a new name
public class RenameCategoryDTO
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
}
