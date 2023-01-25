using System.Text.Json.Serialization;

namespace ToDoApp.WebApi.Authentication.User;

public class EditUserRequest
{
    [JsonPropertyName("currentPassword")]
    public string? CurrentPassword { get; set; }

    [JsonPropertyName("editConfirm")]
    public string? EditConfirm { get; set; }

    [JsonPropertyName("editEmail")]
    public string? EditEmail { get; set; }

    [JsonPropertyName("editPassword")]
    public string? EditPassword { get; set; }

    [JsonPropertyName("editUserName")]
    public string? EditUserName { get; set; }
}
