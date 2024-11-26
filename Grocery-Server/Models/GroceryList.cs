using System.ComponentModel.DataAnnotations.Schema;

namespace Grocery_Server.Models;

public class GroceryList
{
    public Guid Id { get; set; }

    public Guid GroupId { get; set; }

    public DateTime CreationTime { get; set; }

    public virtual ICollection<GroceryListItem>? GroceryListItems { get; set; }

    public virtual Group Group { get; set; }

    public GroceryList(ControllerModels.CreateListDTO createList, Group group)
    {
        Id = new();
        GroupId = group.Id;
        CreationTime = DateTime.UtcNow;
    }

    public GroceryList() { }
}
