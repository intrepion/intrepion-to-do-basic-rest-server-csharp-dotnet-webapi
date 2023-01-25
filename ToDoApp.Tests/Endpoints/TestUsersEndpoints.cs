using System.Net;
using System.Text;
using System.Text.Json;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using ToDoApp.WebApi.Authentication.Login;
using ToDoApp.WebApi.Authentication.User;

namespace ToDoApp.Tests.Endpoints;

public class TestUsersEndpoints : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public TestUsersEndpoints(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task Users_Endpoints()
    {
        // Arrange
        var client = _factory.CreateClient();
        var emptyContent = new StringContent("", Encoding.UTF8, "application/json");

        var adminMakeLoginRequest = new MakeLoginRequest
        {
            Password = "adminP4$$w0rd",
            RememberMe = true,
            UserName = "admin",
        };
        var adminMakeLoginRequestString = JsonSerializer.Serialize(adminMakeLoginRequest);
        var adminMakeLoginRequestContent = new StringContent(adminMakeLoginRequestString, Encoding.UTF8, "application/json");

        var editUserName = Guid.NewGuid().ToString();
        var editUserRequest = new EditUserRequest
        {
            EditUserName = editUserName,
        };
        var editUserRequestString = JsonSerializer.Serialize(editUserRequest);
        var editUserRequestContent = new StringContent(editUserRequestString, Encoding.UTF8, "application/json");

        var makePassword = "makeP4$$w0rd";
        var makeUserName = "makeUserName" + String.Join("", Guid.NewGuid().ToString().Split("-"));
        var makeEmail = $"makeEmail@makeEmail.com";
        var makeUserRequest = new MakeUserRequest
        {
            Confirm = makePassword,
            Email = makeEmail,
            Password = makePassword,
            UserName = makeUserName,
        };
        var makeUserRequestString = JsonSerializer.Serialize(makeUserRequest);
        var makeUserRequestContent = new StringContent(makeUserRequestString, Encoding.UTF8, "application/json");

        var userMakeLoginRequest = new MakeLoginRequest
        {
            Password = "userP4$$w0rd",
            RememberMe = true,
            UserName = "user",
        };
        var userMakeLoginRequestString = JsonSerializer.Serialize(userMakeLoginRequest);
        var userMakeLoginRequestContent = new StringContent(userMakeLoginRequestString, Encoding.UTF8, "application/json");

        var makeMakeLoginRequest = new MakeLoginRequest
        {
            Password = makePassword,
            RememberMe = true,
            UserName = makeUserName,
        };
        var makeMakeLoginRequestString = JsonSerializer.Serialize(makeMakeLoginRequest);
        var makeMakeLoginRequestContent = new StringContent(makeMakeLoginRequestString, Encoding.UTF8, "application/json");

        // Act
        var response = await client.PostAsync("/Logouts", emptyContent);

        // Assert
        Assert.NotNull(response);
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        // Act
        response = await client.GetAsync("/Users");

        // Assert
        Assert.NotNull(response);
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var responseContent = await response.Content.ReadAsStringAsync();
        Assert.NotNull(responseContent);
        var allUsersResponse = JsonSerializer.Deserialize<AllUsersResponse>(responseContent);
        Assert.NotNull(allUsersResponse);
        Assert.NotNull(allUsersResponse.Users);
        var previousUsersCount = allUsersResponse.Users.Count;

        // Act
        response = await client.GetAsync($"/Users/{makeUserName}");

        // Assert
        Assert.NotNull(response);
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        // Act
        response = await client.PutAsync($"/Users/{makeUserName}", editUserRequestContent);

        // Assert
        Assert.NotNull(response);
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);

        // Act
        response = await client.PostAsync("/Users", makeUserRequestContent);

        // Assert
        Assert.NotNull(response);
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        // Act
        response = await client.PostAsync("/Users", makeUserRequestContent);

        // Assert
        Assert.NotNull(response);
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        // Act
        response = await client.GetAsync("/Users");

        // Assert
        Assert.NotNull(response);
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        responseContent = await response.Content.ReadAsStringAsync();
        Assert.NotNull(responseContent);
        allUsersResponse = JsonSerializer.Deserialize<AllUsersResponse>(responseContent);
        Assert.NotNull(allUsersResponse);
        Assert.NotNull(allUsersResponse.Users);
        var nextUsersCount = allUsersResponse.Users.Count;
        Assert.Equal(previousUsersCount + 1, nextUsersCount);

        // Act
        response = await client.GetAsync($"/Users/{makeUserName}");

        // Assert
        Assert.NotNull(response);
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        // Act
        response = await client.PutAsync($"/Users/{makeUserName}", editUserRequestContent);

        // Assert
        Assert.NotNull(response);
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);

        // Act
        response = await client.DeleteAsync($"/Users/{makeUserName}");

        // Assert
        Assert.NotNull(response);
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);

        // Act
        response = await client.PostAsync("/Logins", adminMakeLoginRequestContent);

        // Assert
        Assert.NotNull(response);
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        // Act
        response = await client.DeleteAsync($"/Users/{makeUserName}");

        // Assert
        Assert.NotNull(response);
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        // Act
        response = await client.PostAsync("/Logouts", emptyContent);

        // Assert
        Assert.NotNull(response);
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        // Act
        response = await client.PostAsync("/Logins", userMakeLoginRequestContent);

        // Assert
        Assert.NotNull(response);
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        // Act
        response = await client.GetAsync("/Users");

        // Assert
        Assert.NotNull(response);
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        responseContent = await response.Content.ReadAsStringAsync();
        Assert.NotNull(responseContent);
        allUsersResponse = JsonSerializer.Deserialize<AllUsersResponse>(responseContent);
        Assert.NotNull(allUsersResponse);
        Assert.NotNull(allUsersResponse.Users);
        previousUsersCount = allUsersResponse.Users.Count;
        Assert.Equal(previousUsersCount, nextUsersCount - 1);

        // Act
        response = await client.GetAsync($"/Users/{makeUserName}");

        // Assert
        Assert.NotNull(response);
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        // Act
        response = await client.PutAsync($"/Users/{makeUserName}", editUserRequestContent);

        // Assert
        Assert.NotNull(response);
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        // Act
        response = await client.PostAsync("/Users", makeUserRequestContent);

        // Assert
        Assert.NotNull(response);
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        // Act
        response = await client.PostAsync("/Users", makeUserRequestContent);

        // Assert
        Assert.NotNull(response);
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        // Act
        response = await client.GetAsync($"/Users/{makeUserName}");

        // Assert
        Assert.NotNull(response);
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        // Act
        response = await client.PutAsync($"/Users/{makeUserName}", editUserRequestContent);

        // Assert
        Assert.NotNull(response);
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        // Act
        response = await client.GetAsync($"/Users/{makeUserName}");

        // Assert
        Assert.NotNull(response);
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        // Act
        response = await client.GetAsync($"/Users/{editUserName}");

        // Assert
        Assert.NotNull(response);
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        // Act
        response = await client.PostAsync("/Logouts", emptyContent);

        // Assert
        Assert.NotNull(response);
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        // Act
        response = await client.PostAsync("/Logins", makeMakeLoginRequestContent);
        responseContent = await response.Content.ReadAsStringAsync();

        // Assert
        Assert.NotNull(response);
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        // Act
        response = await client.GetAsync("/Users");

        // Assert
        Assert.NotNull(response);
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        responseContent = await response.Content.ReadAsStringAsync();
        Assert.NotNull(responseContent);
        allUsersResponse = JsonSerializer.Deserialize<AllUsersResponse>(responseContent);
        Assert.NotNull(allUsersResponse);
        Assert.NotNull(allUsersResponse.Users);
        nextUsersCount = allUsersResponse.Users.Count;
        Assert.Equal(previousUsersCount + 1, nextUsersCount);

        // Act
        response = await client.GetAsync($"/Users/{makeUserName}");

        // Assert
        Assert.NotNull(response);
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        // Act
        response = await client.PutAsync($"/Users/{makeUserName}", editUserRequestContent);

        // Assert
        Assert.NotNull(response);
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        // Act
        response = await client.GetAsync($"/Users/{makeUserName}");

        // Assert
        Assert.NotNull(response);
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        // Act
        response = await client.GetAsync($"/Users/{editUserName}");

        // Assert
        Assert.NotNull(response);
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        // Act
        response = await client.DeleteAsync($"/Users/{makeUserName}");

        // Assert
        Assert.NotNull(response);
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);

        // Act
        response = await client.PostAsync("/Logouts", emptyContent);

        // Assert
        Assert.NotNull(response);
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        // Act
        response = await client.PostAsync("/Logins", adminMakeLoginRequestContent);

        // Assert
        Assert.NotNull(response);
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        // Act
        response = await client.DeleteAsync($"/Users/{editUserName}");

        // Assert
        Assert.NotNull(response);
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        // Act
        response = await client.GetAsync("/Users");

        // Assert
        Assert.NotNull(response);
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        responseContent = await response.Content.ReadAsStringAsync();
        Assert.NotNull(responseContent);
        allUsersResponse = JsonSerializer.Deserialize<AllUsersResponse>(responseContent);
        Assert.NotNull(allUsersResponse);
        Assert.NotNull(allUsersResponse.Users);
        previousUsersCount = allUsersResponse.Users.Count;
        Assert.Equal(previousUsersCount, nextUsersCount - 1);

        // Act
        response = await client.GetAsync($"/Users/{makeUserName}");

        // Assert
        Assert.NotNull(response);
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        // Act
        response = await client.PutAsync($"/Users/{makeUserName}", editUserRequestContent);

        // Assert
        Assert.NotNull(response);
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        // Act
        response = await client.PostAsync("/Users", makeUserRequestContent);

        // Assert
        Assert.NotNull(response);
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        // Act
        response = await client.PostAsync("/Users", makeUserRequestContent);

        // Assert
        Assert.NotNull(response);
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        // Act
        response = await client.GetAsync($"/Users/{makeUserName}");

        // Assert
        Assert.NotNull(response);
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        // Act
        response = await client.GetAsync("/Users");

        // Assert
        Assert.NotNull(response);
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        responseContent = await response.Content.ReadAsStringAsync();
        Assert.NotNull(responseContent);
        allUsersResponse = JsonSerializer.Deserialize<AllUsersResponse>(responseContent);
        Assert.NotNull(allUsersResponse);
        Assert.NotNull(allUsersResponse.Users);
        nextUsersCount = allUsersResponse.Users.Count;
        Assert.Equal(previousUsersCount + 1, nextUsersCount);

        // Act
        response = await client.GetAsync($"/Users/{makeUserName}");

        // Assert
        Assert.NotNull(response);
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        // Act
        response = await client.PutAsync($"/Users/{makeUserName}", editUserRequestContent);

        // Assert
        Assert.NotNull(response);
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        // Act
        response = await client.GetAsync($"/Users/{makeUserName}");

        // Assert
        Assert.NotNull(response);
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        // Act
        response = await client.GetAsync($"/Users/{editUserName}");

        // Assert
        Assert.NotNull(response);
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        // Act
        response = await client.DeleteAsync($"/Users/{makeUserName}");

        // Assert
        Assert.NotNull(response);
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        // Act
        response = await client.DeleteAsync($"/Users/{editUserName}");

        // Assert
        Assert.NotNull(response);
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        // Act
        response = await client.DeleteAsync($"/Users/{editUserName}");

        // Assert
        Assert.NotNull(response);
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        // Act
        response = await client.PostAsync("/Logouts", emptyContent);

        // Assert
        Assert.NotNull(response);
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}
