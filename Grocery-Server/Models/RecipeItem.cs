using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.Pkcs;

namespace Grocery_Server.Models
{
    [PrimaryKey(nameof(RecipeId), nameof(ItemId))]
    public class RecipeItem
    {
        public Guid RecipeId { get; set; }
        public Guid ItemId { get; set; }
        public int Quantity { get; set; }

        public virtual Recipe? Recipe { get; }
        public virtual GroceryItem? Item { get; }

        public RecipeItem(Guid recipeId, Guid itemId, int quantity)
        {
            RecipeId = recipeId;
            ItemId = itemId;
            Quantity = quantity;
        }

        public RecipeItem() { }
    }
}
