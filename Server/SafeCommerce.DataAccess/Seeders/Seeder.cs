/*
 * Defines the seeder for initializing the databases in the application.
*/

using Microsoft.AspNetCore.Builder;
using SafeCommerce.Utilities.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using SafeCommerce.DataAccess.Models;
using SafeCommerce.DataAccess.Context;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace SafeCommerce.DataAccessLayer.Seeders;

/// <summary>
/// Defines the role seeder for initializing default roles in the application.
/// Seeds/created the db of <see cref="ApplicationDbContext"/>
/// </summary>
public class Seeder
{
    /// <summary>
    /// Seeds roles into the application based on the configuration.
    /// Created the db for crypto keys also.
    /// </summary>
    /// <param name="applicationBuilder">The application builder.</param>
    /// <param name="configuration">The application configuration.</param>
    public static async Task
    Seed
    (
        IApplicationBuilder applicationBuilder,
        IConfiguration configuration
    )
    {
        using var serviceScope = applicationBuilder.ApplicationServices.CreateScope();

        var _context = serviceScope.ServiceProvider.GetService<ApplicationDbContext>();
        var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        if (_context is not null)
        {
            _context.Database.EnsureCreated();

            try
            {
                // Ensure roles exist
                if (!await roleManager.RoleExistsAsync(configuration["Roles:Moderator"]!))
                    await roleManager.CreateAsync(new IdentityRole(configuration["Roles:Moderator"]!));

                if (!await roleManager.RoleExistsAsync(configuration["Roles:User"]!))
                    await roleManager.CreateAsync(new IdentityRole(configuration["Roles:User"]!));

                var existingModerators = await userManager.GetUsersInRoleAsync(configuration["Roles:Moderator"]!);
                var existingUserNames = new HashSet<string>(existingModerators.Select(m => m.UserName.ToLower()));

                var moderatorsSection = configuration.GetSection("Moderators");
                foreach (var moderatorSection in moderatorsSection.GetChildren())
                {
                    var userName = moderatorSection["UserName"];

                    if (existingUserNames.Contains(userName.ToLower()))
                        continue;

                    var newModerator = new ApplicationUser
                    {
                        UserName = userName,
                        Age = int.Parse(moderatorSection["Age"]),
                        CreatedAt = DateTime.UtcNow,
                        Email = moderatorSection["Email"],
                        EmailConfirmed = true,
                        Gender = Enum.Parse<Gender>(moderatorSection["Gender"]),
                        FullName = moderatorSection["FullName"],
                        ImageData = null,
                        IsDeleted = false,
                        NormalizedEmail = moderatorSection["Email"].ToUpper(),
                        Id = Guid.NewGuid().ToString(),
                        RequireOTPDuringLogin = false,
                        TwoFactorEnabled = false,
                        PhoneNumberConfirmed = true,
                        PhoneNumber = moderatorSection["PhoneNumber"],
                        Birthday = DateTime.Parse(moderatorSection["Birthday"]),
                        NormalizedUserName = userName.ToUpper(),
                        LockoutEnabled = false
                    };

                    var password = configuration[$"ModeratorPasswords:{userName.Replace('.', '_')}"]
                        ?? throw new Exception($"Password for {userName} is missing.");

                    var result = await userManager.CreateAsync(newModerator, password);

                    if (result.Succeeded)
                        await userManager.AddToRoleAsync(newModerator, configuration["Roles:Moderator"]!);
                    else
                        foreach (var error in result.Errors)
                            Console.WriteLine($"Error creating moderator {userName}: {error.Description}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error during seeding: " + ex.Message);
                Console.WriteLine("Please ensure that your User Secrets are correctly configured. Here is the expected structure:");
                Console.WriteLine(@"
                User Secrets Structure Sample:
                {
                    ""ModeratorPasswords"": {
                    ""moderator_1"": ""ModeratorPassword123!"",
                    ""moderator_2"": ""AnotherStrongPassword!""
                    },
                    ""Moderators"": {
                    ""Moderator1"": {
                        ""UserName"": ""moderator.1"",
                        ""Age"": ""20"",
                        ""Email"": ""moderator1@gmail.com"",
                        ""Gender"": ""Male"",
                        ""FullName"": ""Moderator 1"",
                        ""PhoneNumber"": ""43534534534"",
                        ""Birthday"": ""2003-01-15""
                    },
                    ""Moderator2"": {
                        ""UserName"": ""moderator.2"",
                        ""Age"": ""25"",
                        ""Email"": ""moderator2@gmail.com"",
                        ""Gender"": ""Female"",
                        ""FullName"": ""Moderator 2"",
                        ""PhoneNumber"": ""9876543210"",
                        ""Birthday"": ""1998-03-22""
                    }
                    }
                }");
            }
        }
    }
}