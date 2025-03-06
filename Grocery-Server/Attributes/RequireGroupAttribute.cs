using System.Threading.Tasks;
using Grocery_Server.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

public class RequireGroupAttribute : Attribute, IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(
        ActionExecutingContext context,
        ActionExecutionDelegate next
    )
    {
        UserManager<User>? userManager =
            context.HttpContext.RequestServices.GetService(typeof(UserManager<User>))
                as UserManager<User>
            ?? throw new Exception("Cannot get a usermanager apparently");

        // Get the current user
        User? user = await userManager.GetUserAsync(context.HttpContext.User);
        if (user == null)
        {
            // Return Unauthorized if user is null
            context.Result = new UnauthorizedResult();
            return;
        }

        if (user.Group == null)
        {
            // Return BadRequest if user.Group is null
            context.Result = new BadRequestObjectResult("You're not in a group");
            return;
        }

        // If everything is valid, proceed with the action
        await next();
    }
}
