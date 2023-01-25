using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ToDoApp.WebApi.Authentication.User;

namespace ToDoApp.WebApi.Authentication.Logout;

public interface ILogoutsController
{
    public Task<IActionResult> MakeLogoutAsync();
}

[ApiController]
[Route("{controller}")]
public class LogoutsController : ControllerBase, ILogoutsController
{
    private readonly ILogoutsRepository _LogoutsRepository;
    private readonly UserManager<UserEntity> _userManager;

    public LogoutsController(ILogoutsRepository LogoutsRepository, UserManager<UserEntity> userManager)
    {
        _LogoutsRepository = LogoutsRepository;
        _userManager = userManager;
    }

    [HttpPost]
    [Route("")]
    public async Task<IActionResult> MakeLogoutAsync()
    {
        var currentUser = await _userManager.GetUserAsync(HttpContext.User);

        var makeLogoutResponse = await _LogoutsRepository.MakeLogoutAsync(currentUser);

        if (makeLogoutResponse is null)
        {
            return BadRequest();
        }

        return Ok(makeLogoutResponse);
    }
}
