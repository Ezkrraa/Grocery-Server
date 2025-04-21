namespace Grocery_Server.Controllers.RecipeController;

public class NewRecipeDTO
{
    public string Name { get; set; }
    public string Description { get; set; }
    // encoded as JSON
    public string Items { get; set; }
    public string Steps { get; set; }
    public List<IFormFile>? Pictures { get; set; }

    public NewRecipeDTO(string name, string description, string ingredients, string steps, List<IFormFile>? pictures)
    {
        Name = name;
        Description = description;
        Items = ingredients;
        Steps = steps;
        Pictures = pictures;
    }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    public NewRecipeDTO() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
}
