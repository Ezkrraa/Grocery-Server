using System.Diagnostics.CodeAnalysis;

namespace Grocery_Server.Models;

public class RecipePicture
{
    public Guid Id { get; set; }
    public Guid RecipeId { get; set; }
    public string FileName { get; set; }

    [AllowNull]
    public virtual Recipe Recipe { get; set; }

    public RecipePicture(Guid id, Guid recipeId, string fileName)
    {
        Id = id;
        RecipeId = recipeId;
        FileName = fileName;
    }
}
