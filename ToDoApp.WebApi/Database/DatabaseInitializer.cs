using Microsoft.AspNetCore.Identity;
using ToDoApp.WebApi.Authentication.Role;
using ToDoApp.WebApi.Authentication.User;

namespace ToDoApp.WebApi.Database;

public static class DatabaseInitializer
{
    public static void Initialize(ApplicationDatabaseContext context)
    {
        var email = "intrepion@gmail.com";
        var adminRoleName = "Admin";
        var adminRole = context.Roles.SingleOrDefault(role => role.Name == adminRoleName);
        if (adminRole is null)
        {
            adminRole = new RoleEntity
            {
                Id = Guid.NewGuid(),
                Name = adminRoleName,
                NormalizedName = adminRoleName.ToUpper(),
            };

            context.Roles.Add(adminRole);
        }

        var adminUserName = "admin";
        var adminUser = context.Users.SingleOrDefault(user => user.UserName == adminUserName);
        if (adminUser is null)
        {
            adminUser = new UserEntity
            {
                ConcurrencyStamp = "b580c17a-4891-4907-a289-896cfe626059",
                Email = email,
                Id = new Guid("0f22ead4-c2dc-47b6-bfa7-53b71524a123"),
                NormalizedEmail = email.ToUpper(),
                NormalizedUserName = adminUserName.ToUpper(),
                PasswordHash = "AQAAAAIAAYagAAAAEPlC3spp0sF663crmvWsH44fEgHdynasZEhBYjpU33qVayBbqo13yhf7nc53TVeXFQ==",
                SecurityStamp = "5VBDAQB4FP22JE6R6TUSZQEG5FK5U346",
                UserName = adminUserName,
            };

            context.Users.Add(adminUser);
        }

        var adminUserRole = context.UserRoles.SingleOrDefault(userRole => userRole.UserId == adminUser.Id && userRole.RoleId == adminRole.Id);
        if (adminUserRole is null)
        {
            context.UserRoles.Add(new IdentityUserRole<Guid>
            {
                RoleId = adminRole.Id,
                UserId = adminUser.Id,
            });
        }

        var regularRoleName = "Regular";
        var regularRole = context.Roles.SingleOrDefault(role => role.Name == regularRoleName);
        if (regularRole is null)
        {
            regularRole = new RoleEntity
            {
                Id = Guid.NewGuid(),
                Name = regularRoleName,
                NormalizedName = regularRoleName.ToUpper(),
            };

            context.Roles.Add(regularRole);
        }

        var regularUserName = "user";
        var regularUser = context.Users.SingleOrDefault(user => user.UserName == regularUserName);
        if (regularUser is null)
        {
            regularUser = new UserEntity
            {
                ConcurrencyStamp = "29b261a3-8854-47aa-b5db-39d1af4d16b4",
                Email = email,
                Id = new Guid("91e3682b-735a-4da0-8bce-956714313878"),
                NormalizedEmail = email.ToUpper(),
                NormalizedUserName = regularUserName.ToUpper(),
                PasswordHash = "AQAAAAIAAYagAAAAEGjTqPCH6FvDgteBVlUpNmyRuWaNHdwnAls3ATX1IvjGMSQonXFeFvMMo785JsA/4g==",
                SecurityStamp = "7F2WYPUIFN55SQY4LYMC2G56C4MZAUOG",
                UserName = regularUserName,
            };

            context.Users.Add(regularUser);
        }

        var regularUserRole = context.UserRoles.SingleOrDefault(userRole => userRole.UserId == regularUser.Id && userRole.RoleId == regularRole.Id);
        if (regularUserRole is null)
        {
            context.UserRoles.Add(new IdentityUserRole<Guid>
            {
                RoleId = regularRole.Id,
                UserId = regularUser.Id,
            });
        }

        context.SaveChanges();
    }
}
