using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;

namespace SecureApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SecureController : ControllerBase
{
    private readonly IConfiguration _configuration;

    public SecureController(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    /// <summary>
    /// Demonstrates secure secret handling - secrets come from configuration, not code
    /// </summary>
    [HttpGet("config")]
    public IActionResult GetSecureConfig()
    {
        // SECURE: Secrets are retrieved from configuration (Azure Key Vault, environment variables)
        // Never hardcoded in source code
        var hasApiKey = !string.IsNullOrEmpty(_configuration["ApiKey"]);
        var hasDbConnection = !string.IsNullOrEmpty(_configuration["ConnectionStrings:DefaultConnection"]);

        return Ok(new
        {
            message = "Secure configuration check",
            apiKeyConfigured = hasApiKey,
            databaseConfigured = hasDbConnection,
            securityNote = "Secrets are loaded from Azure Key Vault or environment variables"
        });
    }

    /// <summary>
    /// Example of secure input validation
    /// </summary>
    [HttpPost("validate")]
    public IActionResult ValidateInput([FromBody] SecureInputModel input)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        // Secure: Using parameterized operations, no string concatenation for queries
        return Ok(new
        {
            message = "Input validated securely",
            sanitizedName = System.Net.WebUtility.HtmlEncode(input.Name),
            timestamp = DateTime.UtcNow
        });
    }

    /// <summary>
    /// Security status endpoint
    /// </summary>
    [HttpGet("status")]
    public IActionResult GetSecurityStatus()
    {
        return Ok(new
        {
            pipeline = "Zero-Trust CI/CD",
            scansEnabled = new List<string>
            {
                "Secret Detection (Gitleaks)",
                "SAST (CodeQL)",
                "Dependency Scanning (Trivy)",
                "Container Scanning (Trivy)"
            },
            lastScan = DateTime.UtcNow.ToString("O"),
            status = "Protected"
        });
    }
}

public class SecureInputModel
{
    public required string Name { get; set; }
    public string? Description { get; set; }
}

