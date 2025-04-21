using Microsoft.EntityFrameworkCore;

namespace Grocery_Server.Models
{
    [PrimaryKey(nameof(Id))]
    public class Recipe
    {
        public Guid Id { get; set; }
        public Guid GroupId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Steps { get; set; }

        public virtual List<RecipeItem> RecipeItems { get; set; } = [];
        public virtual List<RecipePicture> RecipePictures { get; set; } = [];
        public virtual Group? Group { get; }

        public Recipe(Guid id, Guid groupId, string name, string description, string steps)
        {
            Id = id;
            GroupId = groupId;
            Name = name;
            Description = description;
            Steps = steps;
        }
    }
}
