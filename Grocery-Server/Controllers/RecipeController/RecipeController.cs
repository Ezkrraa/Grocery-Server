// manages users, lets them create accs, reset passwords, etc
//using Grocery_Server.ControllerModels;
using Grocery_Server.Controllers.GroceryListController;
using Grocery_Server.Models;
using Grocery_Server.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Reflection.Metadata.Ecma335;
using System.Text.Json;

namespace Grocery_Server.Controllers.RecipeController;

[ApiController]
[EnableRateLimiting(nameof(RateLimiters.Fast))]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[RequireGroup]
[Route("api/recipe")]
public class RecipeController : ControllerBase
{
    private readonly GroceryDbContext _dbContext;
    private readonly UserManager<User> _userManager;
    private readonly ImageStorageService _imageStorageService;

    public RecipeController([FromServices] GroceryDbContext dbContext, UserManager<User> userManager, ImageStorageService imageStorageService)
    {
        _dbContext = dbContext;
        _userManager = userManager;
        _imageStorageService = imageStorageService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateRecipe([FromForm] NewRecipeDTO recipe)
    {
        List<CreateListItemDTO> items;
        try
        {
            List<CreateListItemDTO>? maybeItems = JsonSerializer.Deserialize<List<CreateListItemDTO>>(recipe.Items);
            if (maybeItems == null || maybeItems.IsNullOrEmpty())
                return BadRequest("Recipes should contain items");
            else if (maybeItems.Any(item => item.ItemId.ToByteArray().Equals(new Guid("{00000000-0000-0000-0000-000000000000}"))))
                return BadRequest("Malformed items JSON");
            else items = maybeItems;
        }
        catch
        {
            return BadRequest("Invalid items JSON");
        }


        User user = await GetUser();
        Guid recipeId = Guid.NewGuid();
        Guid groupId = user.GroupId ?? throw new Exception();
        Recipe dbRecipe = new(recipeId, groupId, recipe.Name, recipe.Description, recipe.Steps);

        if (user.Group?.Recipes.Any(r => r.Name == recipe.Name) ?? true)
            return BadRequest("Your group already contains a recipe with that name");

        List<RecipeItem> recipeItems = items.Select(ri => new RecipeItem(recipeId, ri.ItemId, ri.Quantity)).ToList();


        if (recipe.Pictures != null)
        {
            for (int i = 0; i < recipe.Pictures.Count; i++)
            {
                if (!_imageStorageService.IsValidRecipePicture(recipe.Pictures[i]))
                    return BadRequest($"Image #{++i} was invalid");
            }
            for (int i = 0; i < recipe.Pictures.Count; i++)
            {
                string path = _imageStorageService.SaveImage(recipe.Pictures[i]);
                dbRecipe.RecipePictures.Add(new(Guid.NewGuid(), dbRecipe.Id, path));
            }
        }

        _dbContext.Recipes.Add(dbRecipe);
        _dbContext.RecipeItems.AddRange(recipeItems);
        await _dbContext.SaveChangesAsync();
        return Ok();
    }

    [HttpGet]
    public async Task<IActionResult> GetRecipeDisplays([FromQuery] string? query)
    {
        User user = await GetUser();
        Group group = user.Group ?? throw new Exception();
        IEnumerable<Recipe> recipes = query?.IsNullOrEmpty() ?? true
            ? group.Recipes
            : group.Recipes
            .Where(r => EF.Functions.ILike(r.Name, $"%{query}%") || EF.Functions.ILike(r.Description, $"%{query}%"));
        return Ok(recipes.Select(r => new RecipeInfoDTO(r.Name, r.Description, r.RecipePictures.FirstOrDefault()?.FileName ?? "")));
    }


    [HttpGet("picture/{fileName}")]
    public ActionResult<IFormFile> GetPicture(string fileName)
    {
        RecipePicture? pfp = _dbContext.RecipePictures.FirstOrDefault(pfp => pfp.FileName == fileName);
        if (pfp == null)
            return NotFound("No such profile picture is known");
        else
        {
            try
            {
                FileStream image = _imageStorageService.GetImage(pfp.FileName);
                return Ok(image);

            }
            catch (FileNotFoundException)
            {
                return NotFound("Error loading from storage");
            }
        }
    }

    [NonAction]
    private async Task<User> GetUser()
    {
        return await _userManager.GetUserAsync(User) ?? throw new Exception("Couldn't get user??");
    }
}
