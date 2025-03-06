namespace Grocery_Server.Controllers.GroceryListController
{
    public class ListItemDisplayDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public Guid CategoryId { get; set; }

        public ListItemDisplayDTO(Guid id, string name, int quantity, Guid categoryId)
        {
            Id = id;
            Name = name;
            Quantity = quantity;
            CategoryId = categoryId;
        }
    }
}
