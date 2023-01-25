using Microsoft.AspNetCore.Identity;
using ToDoApp.WebApi.Authentication.User;

namespace ToDoApp.WebApi.Authentication.Login;

public interface ILoginsRepository
{
    public Task<MakeLoginResponse?> MakeLoginAsync(MakeLoginRequest makeLoginRequest);
}

public class LoginsRepository : ILoginsRepository
{
    private readonly SignInManager<UserEntity> _signInManager;

    public LoginsRepository(SignInManager<UserEntity> signInManager)
    {
        _signInManager = signInManager;
    }

    public async Task<MakeLoginResponse?> MakeLoginAsync(MakeLoginRequest makeLoginRequest)
    {
        if (makeLoginRequest is null)
        {
            return null;
        }

        if (String.IsNullOrWhiteSpace(makeLoginRequest.Password))
        {
            return null;
        }

        if (String.IsNullOrWhiteSpace(makeLoginRequest.UserName))
        {
            return null;
        }

        var result = await _signInManager.PasswordSignInAsync(makeLoginRequest.UserName, makeLoginRequest.Password, makeLoginRequest.RememberMe, false);

        if (!result.Succeeded)
        {
            return null;
        }

        return new MakeLoginResponse
        {
            UserName = makeLoginRequest.UserName,
        };
    }
}
