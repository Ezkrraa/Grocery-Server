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
    public DbSet<HouseHoldInvite> HouseholdInvites { get; set; }

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
        // hhi has a composite pk
        modelBuilder.Entity<HouseHoldInvite>()
            .HasKey(invite => new { invite.UserId, invite.HouseholdId });

        // specify the HouseHold's owner
        modelBuilder.Entity<HouseHold>()
            .HasOne(h => h.Owner)
            .WithOne() // skip navigation back
            .HasForeignKey<HouseHold>(h => h.OwnerId)
            .OnDelete(DeleteBehavior.SetNull);
        modelBuilder.Entity<User>()
            .HasOne(u => u.HouseHold)
            .WithMany(h => h.Members)
            .HasForeignKey(u => u.HouseHoldId)
            .OnDelete(DeleteBehavior.SetNull)
            .IsRequired(false);

        modelBuilder.Entity<HouseHold>()
            .HasMany(h => h.GroceryLists)
            .WithOne(gl => gl.HouseHold)
            .HasForeignKey(gl => gl.HouseHoldId)
            .OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<HouseHold>()
            .HasMany(h => h.CustomCategories)
            .WithOne(c => c.HouseHold)
            .HasForeignKey(c => c.HouseHoldId)
            .IsRequired(false);

        modelBuilder.Entity<GroceryList>()
            .HasMany(h => h.GroceryListItems)
            .WithOne(gli => gli.List)
            .HasForeignKey(gli => gli.ListId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<GroceryListItem>()
            .HasOne(gli => gli.Item)
            .WithMany()
            .HasForeignKey(gli => gli.ItemId)
            .HasPrincipalKey(i => i.Id)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<GroceryItem>()
            .HasOne(i => i.Category)
            .WithMany(c => c.Items)
            .HasForeignKey(i => i.CategoryId)
            .HasPrincipalKey(c => c.Id)
            .OnDelete(DeleteBehavior.NoAction);

        // delete invite if household or user gets deleted
        modelBuilder.Entity<User>()
            .HasMany(e => e.Invites)
            .WithOne(invite => invite.User)
            .HasForeignKey(e => e.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<HouseHold>()
            .HasMany(e => e.Invites)
            .WithOne(e => e.HouseHold)
            .HasForeignKey(e => e.HouseholdId)
            .OnDelete(DeleteBehavior.Cascade);

        base.OnModelCreating(modelBuilder);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite($"Data Source={DbPath}");
}
