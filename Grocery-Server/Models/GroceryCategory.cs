using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Grocery_Server.Models;

[PrimaryKey(nameof(Id))]
public class GroceryCategory
{
    public Guid Id { get; set; }

    public Guid HouseHoldId { get; set; }
    public string CategoryName { get; set; }

    public HouseHold HouseHold { get; set; }

    public ICollection<GroceryItem> Items { get; set; }

    public GroceryCategory(ControllerModels.NewCategoryDTO newCategoryDTO)
        : this(newCategoryDTO.Name, newCategoryDTO.HouseHoldId) { }

    public GroceryCategory(string name, Guid houseHoldId)
    {
        CategoryName = name;
        HouseHoldId = houseHoldId;
    }
    public GroceryCategory() { }
}
