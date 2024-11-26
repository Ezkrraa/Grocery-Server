namespace Grocery_Server.ControllerModels
{
    public class AddToListItemDTO
    {
        public Guid ItemId { get; set; }
        public Guid ListId { get; set; }
        public ushort Quantity { get; set; }
    }
}
