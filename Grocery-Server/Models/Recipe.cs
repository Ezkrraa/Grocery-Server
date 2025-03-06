using Microsoft.EntityFrameworkCore;

namespace Grocery_Server.Models
{
    [PrimaryKey(nameof(Id))]
    public class Recipe
    {
        public Guid Id { get; set; }
        public Guid GroupId { get; set; }
        public required string Name { get; set; }

        public virtual List<RecipeItem> RecipeItems { get; } = [];
        public virtual Group? Group { get; }
    }
}
