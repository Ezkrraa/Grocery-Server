namespace Grocery_Server.Controllers.RecipeController;

public class RecipeInfoDTO
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string RecipePictureName { get; set; }
    public RecipeInfoDTO(string name, string description, string recipePictureName)
    {
        Name = name;
        Description = description;
        RecipePictureName = recipePictureName;
    }
}
