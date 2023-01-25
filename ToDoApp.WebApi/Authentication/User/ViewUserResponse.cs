using System.Text.Json.Serialization;

namespace ToDoApp.WebApi.Authentication.User;

public class ViewUserResponse
{
    [JsonPropertyName("userName")]
    public string? UserName { get; set; }
}
