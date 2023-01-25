using Microsoft.AspNetCore.Mvc;

namespace ToDoApp.WebApi.Authentication.Login;

public interface ILoginsController
{
    public Task<IActionResult> MakeLoginAsync([FromBody] MakeLoginRequest makeLoginRequest);
}

[ApiController]
[Route("{controller}")]
public class LoginsController : ControllerBase, ILoginsController
{
    private readonly ILoginsRepository _loginsRepository;

    public LoginsController(ILoginsRepository loginsRepository)
    {
        _loginsRepository = loginsRepository;
    }

    [HttpPost]
    [Route("")]
    public async Task<IActionResult> MakeLoginAsync([FromBody] MakeLoginRequest makeLoginRequest)
    {
        var makeLoginResponse = await _loginsRepository.MakeLoginAsync(makeLoginRequest);

        if (makeLoginResponse is null)
        {
            return BadRequest();
        }

        return Ok(makeLoginResponse);
    }
}
