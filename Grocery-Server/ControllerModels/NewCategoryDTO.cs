using Grocery_Server.Models;
using Microsoft.Identity.Client;

namespace Grocery_Server.ControllerModels;

public class NewCategoryDTO
{
    public Guid GroupId { get; set; }
    public string Name { get; set; }
}
