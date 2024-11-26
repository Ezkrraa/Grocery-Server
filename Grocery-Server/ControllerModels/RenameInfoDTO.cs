namespace Grocery_Server.ControllerModels;


// for renaming an item or category, has an ID and a new name
public class RenameInfoDTO
{
    public Guid Id { get; set; }
    public string Name { get; set; }
}
