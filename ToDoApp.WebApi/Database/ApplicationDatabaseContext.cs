using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ToDoApp.WebApi.Authentication.Role;
using ToDoApp.WebApi.Authentication.User;

namespace ToDoApp.WebApi.Database;

public class ApplicationDatabaseContext : IdentityDbContext<UserEntity, RoleEntity, Guid>
{
    public ApplicationDatabaseContext(DbContextOptions<ApplicationDatabaseContext> options) : base(options)
    {
        Database.EnsureCreated();
        DatabaseInitializer.Initialize(this);
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
    }
}
