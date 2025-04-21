using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace Grocery_Server.Models
{
    [PrimaryKey(nameof(RecipeId), nameof(ItemId))]
    public class RecipeItem
    {
        public Guid RecipeId { get; set; }
        public Guid ItemId { get; set; }
        public int Quantity { get; set; }

        [AllowNull]
        public virtual Recipe Recipe { get; }
        [AllowNull]
        public virtual GroceryItem? Item { get; }

        public RecipeItem(Guid recipeId, Guid itemId, int quantity)
        {
            RecipeId = recipeId;
            ItemId = itemId;
            Quantity = quantity;
        }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
        public RecipeItem() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    }
}
