using Grocery_Server.Models;

namespace Grocery_Server.Controllers.RecipeController;

public class RecipeInfoDTO
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string RecipePictureName { get; set; }
    public string Steps { get; set; }
    public RecipeInfoDTO(string name, string description, string recipePictureName, string steps)
    {
        Name = name;
        Description = description;
        RecipePictureName = recipePictureName;
        Steps = steps;
    }

    public RecipeInfoDTO(Recipe recipe)
        : this(recipe.Name, recipe.Description, recipe.RecipePictures.FirstOrDefault()?.FileName ?? "", recipe.Steps) { }
}
