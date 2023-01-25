using System.Text.Json.Serialization;

namespace ToDoApp.WebApi.Authentication.Login;

public class MakeLoginResponse
{
    [JsonPropertyName("userName")]
    public string? UserName { get; set; }
}
