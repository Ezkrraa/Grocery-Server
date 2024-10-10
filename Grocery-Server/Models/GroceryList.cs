using System.ComponentModel.DataAnnotations.Schema;

namespace Grocery_Server.Models;

public class GroceryList
{
    public Guid Id { get; set; }

    public Guid HouseHoldId { get; set; }

    public DateTime CreationTime { get; set; }

    public ICollection<GroceryListItem>? GroceryListItems { get; set; }

    public HouseHold HouseHold { get; set; }

    public GroceryList() { }
}
