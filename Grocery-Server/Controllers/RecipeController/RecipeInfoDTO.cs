using Grocery_Server.Models;

namespace Grocery_Server.Controllers.RecipeController;

public class RecipeInfoDTO
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string RecipePictureName { get; set; }
    public string Steps { get; set; }
    public RecipeInfoDTO(Guid id, string name, string description, string recipePictureName, string steps)
    {
        Id = id;
        Name = name;
        Description = description;
        RecipePictureName = recipePictureName;
        Steps = steps;
    }

    public RecipeInfoDTO(Recipe recipe)
        : this(recipe.Id, recipe.Name, recipe.Description, recipe.RecipePictures.FirstOrDefault()?.FileName ?? "", recipe.Steps) { }
}
