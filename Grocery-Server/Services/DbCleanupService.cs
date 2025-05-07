using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Grocery_Server.Services
{
    public class DbCleanupService
    {
        public async Task CleanupCycle(GroceryDbContext dbContext)
        {
            dbContext.Database.Migrate();
            dbContext.RemoveRange(
                dbContext.GroupInvites.Where(invite => invite.ExpirationTime < DateTime.UtcNow)
            );
            dbContext.RemoveRange(
                dbContext.GroceryLists.Where(list =>
                    list.CreationTime < DateTime.UtcNow.AddDays(-365)
                )
            );
            await dbContext.SaveChangesAsync();
        }
    }
}
