using System.Text.Json.Serialization;

namespace ToDoApp.WebApi.Authentication.User;

public class MakeUserResponse
{
    [JsonPropertyName("userName")]
    public string? UserName { get; set; }
}
