using Grocery_Server.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;

namespace Grocery_Server;

public class DbContext : IdentityDbContext<User>
{
    // explicitly override it to show all my columns here
    public override DbSet<User> Users { get; set; }
    public DbSet<Group> Groups { get; set; }
    public DbSet<GroceryList> GroceryLists { get; set; }
    public DbSet<GroceryListItem> GroceryListItems { get; set; }
    public DbSet<GroceryItem> GroceryItems { get; set; }
    public DbSet<GroceryCategory> GroceryCategories { get; set; }
    public DbSet<GroupInvite> GroupInvites { get; set; }

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
        modelBuilder.Entity<GroupInvite>()
            .HasKey(invite => new { invite.UserId, invite.GroupId });

        // specify the Group's owner
        modelBuilder.Entity<Group>()
            .HasOne(h => h.Owner)
            .WithOne() // skip navigation back
            .HasForeignKey<Group>(h => h.OwnerId)
            .OnDelete(DeleteBehavior.SetNull);
        modelBuilder.Entity<User>()
            .HasOne(u => u.Group)
            .WithMany(h => h.Members)
            .HasForeignKey(u => u.GroupId)
            .OnDelete(DeleteBehavior.SetNull)
            .IsRequired(false);

        modelBuilder.Entity<Group>()
            .HasMany(h => h.GroceryLists)
            .WithOne(gl => gl.Group)
            .HasForeignKey(gl => gl.GroupId)
            .OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<Group>()
            .HasMany(h => h.CustomCategories)
            .WithOne(c => c.Group)
            .HasForeignKey(c => c.GroupId)
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
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<GroceryItem>()
            .HasOne(i => i.Category)
            .WithMany(c => c.Items)
            .HasForeignKey(i => i.CategoryId)
            .HasPrincipalKey(c => c.Id)
            .OnDelete(DeleteBehavior.NoAction);

        // delete invite if group or user gets deleted
        modelBuilder.Entity<User>()
            .HasMany(e => e.Invites)
            .WithOne(invite => invite.User)
            .HasForeignKey(e => e.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<Group>()
            .HasMany(e => e.Invites)
            .WithOne(e => e.Group)
            .HasForeignKey(e => e.GroupId)
            .OnDelete(DeleteBehavior.Cascade);

        base.OnModelCreating(modelBuilder);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseLazyLoadingProxies()
                .UseSqlite($"Data Source={DbPath}");
}
