/*
 * Provides the database context for the SafeCommerce application.
 * This file configures the tables, relationships, and other database settings for the models in the application.
*/

using Microsoft.EntityFrameworkCore;
using SafeCommerce.DataAccess.Models;
using SafeCommerce.DataAccessLayer.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using SafeShare.DataAccessLayer.Models;

namespace SafeCommerce.DataAccess.Context;

/// <summary>
/// Represents the database context for the Safe Commerce.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="ApplicationDbContext"/> class.
/// </remarks>
/// <param name="options">The options for configuring the database context.</param>
public class ApplicationDbContext
(
    DbContextOptions<ApplicationDbContext> options
) : IdentityDbContext<ApplicationUser>(options)
{
    /// <summary>
    /// Gets or sets the database table for refresh tokens.
    /// </summary>
    public DbSet<RefreshToken> RefreshTokens { get; set; }
    /// <summary>
    /// Gets or sets the database table for Shops.
    /// </summary>
    public DbSet<Shop> Shops { get; set; }
    /// <summary>
    /// Gets or sets the database table for Items.
    /// </summary>
    public DbSet<Item> Items { get; set; }
    /// <summary>
    /// Gets or sets the database table for ShopShares.
    /// </summary>
    public DbSet<ShopShare> ShopShares { get; set; }
    /// <summary>
    /// Gets or sets the database table for ItemShares.
    /// </summary>
    public DbSet<ItemShare> ItemShares { get; set; }
    /// <summary>
    /// Gets or sets the database table for Moderations.
    /// </summary>
    public DbSet<ModerationHistory> ModerationHistories { get; set; }
    /// <summary>
    /// Gets or sets the database table for Shop invitations.
    /// </summary>
    public DbSet<ShopInvitation> ShopInvitations { get; set; }
    /// <summary>
    /// Gets or sets the database table for Item invitations.
    /// </summary>
    public DbSet<ItemInvitation> ItemInvitations { get; set; }

    protected override void OnModelCreating
    (
        ModelBuilder modelBuilder
    )
    {
        base.OnModelCreating(modelBuilder);

        // User to Shops relationship
        modelBuilder.Entity<ApplicationUser>()
            .HasMany(u => u.Shops)
            .WithOne(s => s.Owner)
            .HasForeignKey(s => s.OwnerId)
            .OnDelete(DeleteBehavior.Restrict);

        // User to Items relationship
        modelBuilder.Entity<ApplicationUser>()
            .HasMany(u => u.Items)
            .WithOne(i => i.Owner)
            .HasForeignKey(i => i.OwnerId)
            .OnDelete(DeleteBehavior.Restrict);

        // Shop to Items relationship
        modelBuilder.Entity<Shop>()
            .HasMany(s => s.Items)
            .WithOne(i => i.Shop)
            .HasForeignKey(i => i.ShopId)
            .OnDelete(DeleteBehavior.Cascade);

        // Shop to ShopShares relationship
        modelBuilder.Entity<ShopShare>()
            .HasKey(ss => new { ss.ShopId, ss.UserId });

        modelBuilder.Entity<ShopShare>()
            .HasOne(ss => ss.Shop)
            .WithMany(s => s.ShopShares)
            .HasForeignKey(ss => ss.ShopId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<ShopShare>()
            .HasOne(ss => ss.User)
            .WithMany()
            .HasForeignKey(ss => ss.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        // Item to ItemShares relationship
        modelBuilder.Entity<ItemShare>()
            .HasKey(ish => new { ish.ItemId, ish.UserId });

        modelBuilder.Entity<ItemShare>()
            .HasOne(ish => ish.Item)
            .WithMany(i => i.ItemShares)
            .HasForeignKey(ish => ish.ItemId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<ItemShare>()
            .HasOne(ish => ish.User)
            .WithMany()
            .HasForeignKey(ish => ish.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        // Shop Invitation to User relationship

        modelBuilder.Entity<ShopInvitation>()
           .HasOne(gi => gi.InvitingUser)
           .WithMany(u => u.SentInvitations)
           .HasForeignKey(gi => gi.InvitingUserId)
           .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<ShopInvitation>()
            .HasOne(gi => gi.InvitedUser)
            .WithMany(u => u.ReceivedInvitations)
            .HasForeignKey(gi => gi.InvitedUserId)
            .OnDelete(DeleteBehavior.Restrict);

        // Item Invitation to User relationship

        modelBuilder.Entity<ItemInvitation>()
           .HasOne(gi => gi.InvitingUser)
           .WithMany(u => u.SentItemInvitations)
           .HasForeignKey(gi => gi.InvitingUserId)
           .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<ItemInvitation>()
            .HasOne(gi => gi.InvitedUser)
            .WithMany(u => u.ReceivedItemInvitations)
            .HasForeignKey(gi => gi.InvitedUserId)
            .OnDelete(DeleteBehavior.Restrict);

        // Configure one-to-one relationship between Item and ModerationHistory
        modelBuilder.Entity<Item>()
            .HasOne(i => i.ModerationHistory)
            .WithOne(mh => mh.Item)
            .HasForeignKey<ModerationHistory>(mh => mh.ItemId)
            .OnDelete(DeleteBehavior.Restrict);

        // Configure one-to-one relationship between Shop and ModerationHistory
        modelBuilder.Entity<Shop>()
            .HasOne(s => s.ModerationHistory)
            .WithOne(mh => mh.Shop)
            .HasForeignKey<ModerationHistory>(mh => mh.ShopId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}