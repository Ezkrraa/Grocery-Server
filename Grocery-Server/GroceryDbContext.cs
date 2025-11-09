using Grocery_Server.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Grocery_Server;

public class GroceryDbContext : IdentityDbContext<User>
{
    // explicitly override it to show all my columns here
    public override DbSet<User> Users { get; set; }
    public DbSet<Group> Groups { get; set; }
    public DbSet<GroceryList> GroceryLists { get; set; }
    public DbSet<GroceryListItem> GroceryListItems { get; set; }
    public DbSet<GroceryItem> GroceryItems { get; set; }
    public DbSet<GroceryCategory> GroceryCategories { get; set; }
    public DbSet<GroupInvite> GroupInvites { get; set; }
    public DbSet<Recipe> Recipes { get; set; }
    public DbSet<RecipeItem> RecipeItems { get; set; }
    public DbSet<RecipePicture> RecipePictures { get; set; }
    public DbSet<ProfilePicture> ProfilePictures { get; set; }
    public string DbPath { get; }

    public GroceryDbContext(DbContextOptions<GroceryDbContext> options) : base(options)
    {
        Environment.SpecialFolder folder = Environment.SpecialFolder.LocalApplicationData;
        string path = Path.Join(Environment.GetFolderPath(folder), "Groceries-Server");
        DbPath = Path.Join(path, "Database.db");
        if (!Directory.Exists(path))
            Directory.CreateDirectory(path);
        //base.Database.Migrate();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // gli has a composite pk
        modelBuilder.Entity<GroceryListItem>().HasKey(gli => new { gli.ItemId, gli.ListId });
        // hhi has a composite pk
        modelBuilder.Entity<GroupInvite>().HasKey(invite => new { invite.UserId, invite.GroupId });

        // specify the Group's owner
        modelBuilder
            .Entity<Group>()
            .HasOne(h => h.Owner)
            .WithOne() // skip navigation back
            .HasForeignKey<Group>(h => h.OwnerId)
            .OnDelete(DeleteBehavior.NoAction);
        modelBuilder
            .Entity<User>()
            .HasOne(u => u.Group)
            .WithMany(h => h.Members)
            .HasForeignKey(u => u.GroupId)
            .OnDelete(DeleteBehavior.SetNull)
            .IsRequired(false);

        modelBuilder
            .Entity<Group>()
            .HasMany(h => h.GroceryLists)
            .WithOne(gl => gl.Group)
            .HasForeignKey(gl => gl.GroupId)
            .OnDelete(DeleteBehavior.Cascade);
        modelBuilder
            .Entity<Group>()
            .HasMany(h => h.CustomCategories)
            .WithOne(c => c.Group)
            .HasForeignKey(c => c.GroupId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired(false);

        modelBuilder.Entity<Group>()
            .HasMany(group => group.Recipes)
            .WithOne(recipe => recipe.Group)
            .HasPrincipalKey(g => g.Id)
            .HasForeignKey(r => r.GroupId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder
            .Entity<GroceryList>()
            .HasMany(h => h.GroceryListItems)
            .WithOne(gli => gli.List)
            .HasForeignKey(gli => gli.ListId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder
            .Entity<GroceryListItem>()
            .HasOne(gli => gli.Item)
            .WithMany()
            .HasForeignKey(gli => gli.ItemId)
            .HasPrincipalKey(i => i.Id)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder
            .Entity<GroceryItem>()
            .HasOne(i => i.Category)
            .WithMany(c => c.Items)
            .HasForeignKey(i => i.CategoryId)
            .HasPrincipalKey(c => c.Id)
            .OnDelete(DeleteBehavior.NoAction);

        // delete invite if group or user gets deleted
        modelBuilder
            .Entity<User>()
            .HasMany(e => e.Invites)
            .WithOne(invite => invite.User)
            .HasForeignKey(e => e.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        modelBuilder
            .Entity<Group>()
            .HasMany(e => e.Invites)
            .WithOne(e => e.Group)
            .HasForeignKey(e => e.GroupId)
            .OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<Recipe>()
            .HasMany(r => r.RecipeItems)
            .WithOne(ri => ri.Recipe)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<GroceryItem>()
            .HasMany(gi => gi.RecipeItems)
            .WithOne(ri => ri.Item)
            .HasPrincipalKey(gi => gi.Id)
            .HasForeignKey(ri => ri.ItemId);

        modelBuilder.Entity<Recipe>()
            .HasMany(r => r.RecipePictures)
            .WithOne(rp => rp.Recipe)
            .OnDelete(DeleteBehavior.Cascade)
            .HasPrincipalKey(r => r.Id)
            .HasForeignKey(rp => rp.RecipeId);

        modelBuilder.Entity<User>()
            .HasOne(u => u.ProfilePicture)
            .WithOne(pfp => pfp.User)
            .OnDelete(DeleteBehavior.SetNull);



        base.OnModelCreating(modelBuilder);
    }
}
