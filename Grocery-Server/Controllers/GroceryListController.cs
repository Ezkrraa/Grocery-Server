using Grocery_Server.ControllerModels;
using Grocery_Server.Models;
using Microsoft.AspNetCore.Mvc;

namespace Grocery_Server.Controllers
{
    [ApiController]
    [Route("api/grocery-list-controller")]
    public class GroceryListController : ControllerBase
    {
        private readonly DbContext _dbContext;
        public GroceryListController(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet("get-list")]
        public IActionResult GetList([FromQuery] Guid id)
        {
            Models.GroceryList? list = _dbContext.GroceryLists.FirstOrDefault(list => list.Id == id);
            if (list == null)
                return NotFound();
            _dbContext.Entry(list).Collection(list => list.GroceryListItems).Load();
            List<Models.GroceryListItem> items = list.GroceryListItems.ToList();
            return Ok(list.GroceryListItems.Select(item => new ItemDisplayDTO(item)));
        }

        [HttpPost("create-list")]
        public IActionResult CreateList([FromBody] CreateListDTO itemsList)
        {
            HouseHold? houseHold = _dbContext.HouseHolds.FirstOrDefault(house => house.Id == itemsList.HouseId);
            if (houseHold == null)
                return NotFound();

            GroceryList newList = new(itemsList.HouseId);
            newList.GroceryListItems = itemsList.Items.Select(item => new GroceryListItem(newList.Id, item.ItemId)).ToList();
            _dbContext.GroceryLists.Add(newList);
            _dbContext.SaveChanges();
            return Ok();
        }
    }
}
