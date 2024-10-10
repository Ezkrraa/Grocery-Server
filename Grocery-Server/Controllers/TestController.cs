using Grocery_Server.ControllerModels;
using Grocery_Server.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;
using System.Net;

namespace Grocery_Server.Controllers
{
    //[Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class TestController : ControllerBase
    {
        private readonly ILogger<TestController> _logger;
        private readonly DbContext _dbContext;
        private readonly UserManager<User> _userManager;

        public TestController(ILogger<TestController> logger, DbContext context, UserManager<User> userManager)
        {
            _dbContext = context;
            _logger = logger;
            _userManager = userManager;
        }

        [HttpGet("get-user")]
        public IActionResult GetUser([FromQuery] Guid id)
        {
            string strId = id.ToString();
            return Ok(_dbContext.Users.FirstOrDefault(user => user.Id == strId));
        }

        [HttpGet("get-household")]
        public IActionResult GetHouseHold([FromQuery] Guid userId)
        {
            string strId = userId.ToString();
            User? user = _dbContext.Users.FirstOrDefault(user => user.Id == strId);
            if (user == null)
                return NotFound();
            _dbContext.Entry(user).Reference(user => user.HouseHold).Load();
            if (user.HouseHold == null)
                return NotFound("Household was null");
            return Ok(user.HouseHold.GetString());
        }

        [HttpGet("get-categories")]
        public IActionResult GetCategories([FromQuery] Guid id)
        {
            HouseHold? household = _dbContext.HouseHolds.FirstOrDefault(h => h.Id == id);
            if (household == null)
                return NotFound();
            _dbContext.Entry(household).Collection(household => household.CustomCategories).Load();
            List<CategoryDisplayDTO> categories = household.CustomCategories.Select(cat => new CategoryDisplayDTO(cat)).ToList();
            return Ok(categories);
        }

        [HttpPost("create-account")]
        public async Task<IActionResult> CreateAccount([FromBody] NewUserDTO newUser)
        {
            if (!newUser.IsValid())
                return BadRequest();

            if (_dbContext.Users.Any(user => user.NormalizedUserName == newUser.UserName.Normalize() || user.NormalizedEmail == newUser.Email.Normalize()))
                return Conflict();

            User user = new(newUser);
            _dbContext.Users.Add(user);
            _dbContext.SaveChanges();
            await _userManager.AddPasswordAsync(user, newUser.Password);
            return Ok(user);
            //return new StatusCodeResult(StatusCodes.Status500InternalServerError);
        }

        [HttpPost("create-category")]
        public IActionResult CreateCategory([FromBody] NewCategoryDTO newCategory)
        {
            GroceryCategory category = new(newCategory);
            HouseHold? houseHold = _dbContext.HouseHolds.FirstOrDefault(household => household.Id == newCategory.HouseHoldId);
            if (houseHold == null) return NotFound();
            _dbContext.GroceryCategories.Add(category);
            _dbContext.SaveChanges();
            return Ok();
        }

        [HttpPost("create-household")]
        public IActionResult CreateHouseHold([FromBody] NewHouseHoldDTO creationModel)
        {
            HouseHold houseHold = new(creationModel);
            User? user = _dbContext.Users.FirstOrDefault(user => user.Id == creationModel.UserId);
            if (user == null)
                return BadRequest();
            if (_dbContext.HouseHolds.Any(house => house.OwnerId == creationModel.UserId))
                return Conflict(); // can't create new group if already owner of a group
            _dbContext.HouseHolds.Add(houseHold);
            user.HouseHoldId = houseHold.Id;
            _dbContext.SaveChanges();
            return Ok(houseHold.GetString());
        }

        [HttpPost("create-item")]
        public IActionResult CreateItem([FromBody] NewItemDTO itemDTO)
        {
            if (!_dbContext.GroceryCategories.Any(category => category.Id == itemDTO.CategoryId))
                return BadRequest();
            GroceryItem newItem = new(itemDTO);
            _dbContext.GroceryItems.Add(newItem);
            _dbContext.SaveChanges();
            return Ok(newItem);
        }

        [HttpGet("get-items")]
        public IActionResult GetItemsInCategory(Guid id)
        {
            GroceryCategory? category = _dbContext.GroceryCategories.FirstOrDefault(category => category.Id == id);
            if (category == null)
                return NotFound();
            _dbContext.Entry(category).Collection(category => category.Items).Load();
            return Ok(category.Items.Select(item => new ItemDisplayDTO(item)).ToList());
        }
    }
}
