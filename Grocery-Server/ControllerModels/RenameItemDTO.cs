namespace Grocery_Server.ControllerModels;


// for renaming an item, has an ID and a new name
public class RenameItemDTO
{
    public Guid Id { get; set; }
    public string Name { get; set; }
}
