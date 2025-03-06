using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Grocery_Server.Services
{
    public class DbCleanupService
    {
        public void CleanupCycle(DbContext dbContext)
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
            dbContext.SaveChanges();
        }
    }
}
