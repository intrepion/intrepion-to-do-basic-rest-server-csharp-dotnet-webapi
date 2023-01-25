using System.Text.Json.Serialization;

namespace ToDoApp.WebApi.Authentication.Login;

public class MakeLoginRequest
{
    [JsonPropertyName("password")]
    public string? Password { get; set; }

    [JsonPropertyName("rememberMe")]
    public bool RememberMe { get; set; }

    [JsonPropertyName("userName")]
    public string? UserName { get; set; }
}
