namespace Grocery_Server.Controllers.GroceryListController
{
    public class AddToListItemDTO
    {
        public Guid ItemId { get; set; }
        public Guid ListId { get; set; }
        public ushort Quantity { get; set; }
    }
}
