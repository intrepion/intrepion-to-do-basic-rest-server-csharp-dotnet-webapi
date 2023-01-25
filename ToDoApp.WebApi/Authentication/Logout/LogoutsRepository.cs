using Microsoft.AspNetCore.Identity;
using ToDoApp.WebApi.Authentication.User;

namespace ToDoApp.WebApi.Authentication.Logout;

public interface ILogoutsRepository
{
    public Task<MakeLogoutResponse?> MakeLogoutAsync(UserEntity? currentUser);
}

public class LogoutsRepository : ILogoutsRepository
{
    private readonly SignInManager<UserEntity> _signInManager;

    public LogoutsRepository(SignInManager<UserEntity> signInManager)
    {
        _signInManager = signInManager;
    }

    public async Task<MakeLogoutResponse?> MakeLogoutAsync(UserEntity? currentUser)
    {
        if (currentUser is null)
        {
            return null;
        }

        await _signInManager.SignOutAsync();

        return new MakeLogoutResponse();
    }
}
