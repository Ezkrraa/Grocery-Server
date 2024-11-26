using Grocery_Server.ControllerModels;
using Grocery_Server.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;
using System.Net;
/*
namespace Grocery_Server.Controllers
{
    //[Authorize]
    [ApiController]
    [Route("api/test")]
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

        [HttpGet("get-categories")]
        public IActionResult GetCategories([FromQuery] Guid id)
        {
            Group? group = _dbContext.Groups.FirstOrDefault(h => h.Id == id);
            if (group == null)
                return NotFound();
            List<CategoryDisplayDTO> categories = group.CustomCategories.Select(cat => new CategoryDisplayDTO(cat)).ToList();
            return Ok(categories);
        }

        [HttpPost("create-account")]
        public async Task<IActionResult> CreateAccount([FromBody] NewUserDTO newUser)
        {
            if (_dbContext.Users.Any(user => user.NormalizedUserName == newUser.UserName.Normalize() || user.NormalizedEmail == newUser.Email.Normalize()))
                return Conflict();

            User user = new(newUser);
            _dbContext.Users.Add(user);
            _dbContext.SaveChanges();
            await _userManager.AddPasswordAsync(user, newUser.Password);
            return Ok(user);
        }

        [HttpPost("create-category")]
        public IActionResult CreateCategory([FromBody] NewCategoryDTO newCategory)
        {
            GroceryCategory category = new(newCategory);
            Group? group = _dbContext.Groups.FirstOrDefault(group => group.Id == newCategory.GroupId);
            if (group == null) return NotFound();
            _dbContext.GroceryCategories.Add(category);
            _dbContext.SaveChanges();
            return Ok();
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
            return Ok(category.Items.Select(item => new ItemDisplayDTO(item)).ToList());
        }

        [HttpGet("get-all-items")]
        public IActionResult GetAllItems()
        {
            return Ok(_dbContext.GroceryItems.Select(item => new ItemDisplayDTO(item)).ToList());
        }

        [HttpGet("get-all-users")]
        public IActionResult GetAllUsers()
        {
            return Ok(_dbContext.Users.Select(user => new UserDisplayDTO(user)));
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet("test-session")]
        public IActionResult TestSession()
        {
            return Ok(new UserDisplayDTO(GetUser().Result));
        }

        private async Task<User> GetUser()
        {
            User? user = await _userManager.GetUserAsync(User);
            return user ?? throw new Exception("No current user found!!");
        }
    }
}

 */