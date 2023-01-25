using Microsoft.AspNetCore.Mvc;

namespace ToDoApp.WebApi.HealthCheck;

public class HealthCheckController : ControllerBase
{
    public IActionResult Get()
    {
        return Ok("");
    }
}
