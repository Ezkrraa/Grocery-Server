using Grocery_Server.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;

namespace Grocery_Server;

public class DbContext : IdentityDbContext<User>
{
    public DbSet<User> Users { get; set; }
    public DbSet<HouseHold> HouseHolds { get; set; }
    public DbSet<GroceryList> GroceryLists { get; set; }
    public DbSet<GroceryListItem> GroceryListItems { get; set; }
    public DbSet<GroceryItem> GroceryItems { get; set; }
    public DbSet<GroceryCategory> GroceryCategories { get; set; }

    public string DbPath { get; }

    public DbContext()
    {
        Environment.SpecialFolder folder = Environment.SpecialFolder.LocalApplicationData;
        string path = Path.Join(Environment.GetFolderPath(folder), "Groceries-Server");
        DbPath = Path.Join(path, "Database.db");
        if (!Directory.Exists(path))
            Directory.CreateDirectory(path);
        if (!File.Exists(DbPath))
            File.Create(DbPath);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // gli has a composite pk
        modelBuilder.Entity<GroceryListItem>()
            .HasKey(gli => new { gli.ItemId, gli.ListId });

        // specify the HouseHold's owner
        modelBuilder.Entity<HouseHold>()
            .HasOne(h => h.Owner)
            .WithOne() // skip navigation back
            .HasForeignKey<HouseHold>(h => h.OwnerId);
        modelBuilder.Entity<User>()
            .HasOne(u => u.HouseHold)
            .WithMany(h => h.Members)
            .HasForeignKey(u => u.HouseHoldId)
            .IsRequired(false);

        modelBuilder.Entity<HouseHold>()
            .HasMany(h => h.GroceryLists)
            .WithOne(gl => gl.HouseHold)
            .HasForeignKey(gl => gl.HouseHoldId);
        modelBuilder.Entity<HouseHold>()
            .HasMany(h => h.CustomCategories)
            .WithOne(c => c.HouseHold)
            .HasForeignKey(c => c.HouseHoldId)
            .IsRequired(false);

        modelBuilder.Entity<GroceryList>()
            .HasMany(h => h.GroceryListItems)
            .WithOne(gli => gli.List)
            .HasForeignKey(gli => gli.ListId);

        modelBuilder.Entity<GroceryListItem>()
            .HasOne(gli => gli.Item)
            .WithMany()
            .HasForeignKey(gli => gli.ItemId)
            .HasPrincipalKey(i => i.Id);

        modelBuilder.Entity<GroceryItem>()
            .HasOne(i => i.Category)
            .WithMany(c => c.Items)
            .HasForeignKey(i => i.CategoryId)
            .HasPrincipalKey(c => c.Id);
        base.OnModelCreating(modelBuilder);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite($"Data Source={DbPath}");
}
