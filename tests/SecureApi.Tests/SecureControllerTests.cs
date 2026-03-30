using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace SecureApi.Tests;

public class SecureControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public SecureControllerTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetRoot_ReturnsWelcomeMessage()
    {
        // Act
        var response = await _client.GetAsync("/");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var content = await response.Content.ReadAsStringAsync();
        Assert.Contains("Welcome to Secure API", content);
    }

    [Fact]
    public async Task GetHealth_ReturnsHealthy()
    {
        // Act
        var response = await _client.GetAsync("/health");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task GetSecurityStatus_ReturnsProtectedStatus()
    {
        // Act
        var response = await _client.GetAsync("/api/secure/status");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var content = await response.Content.ReadAsStringAsync();
        Assert.Contains("Zero-Trust CI/CD", content);
        Assert.Contains("Protected", content);
    }

    [Fact]
    public async Task GetSecureConfig_ReturnsConfigStatus()
    {
        // Act
        var response = await _client.GetAsync("/api/secure/config");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task PostValidate_WithValidInput_ReturnsSuccess()
    {
        // Arrange
        var input = new { Name = "Test User", Description = "Test description" };

        // Act
        var response = await _client.PostAsJsonAsync("/api/secure/validate", input);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task PostValidate_WithMaliciousInput_SanitizesOutput()
    {
        // Arrange - XSS attempt
        var input = new { Name = "<script>alert('xss')</script>", Description = "Test" };

        // Act
        var response = await _client.PostAsJsonAsync("/api/secure/validate", input);
        var content = await response.Content.ReadAsStringAsync();

        // Assert - Input should be HTML encoded
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.DoesNotContain("<script>", content);
    }
}

