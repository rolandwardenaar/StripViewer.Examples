using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.IO;

namespace Voorbeeld.WebApplication.Services
{
    public class TokenService : ITokenService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly ILogger<TokenService> _logger;
        private readonly string _configPath;

        public TokenService(IHttpClientFactory httpClientFactory, IConfiguration configuration, ILogger<TokenService> logger)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            _logger = logger;
            _configPath = Path.Combine(AppContext.BaseDirectory, "appsettings.json");
        }

        public async Task<string> GetValidTokenAsync()
        {
            var currentToken = _configuration["Yaro:Token"];
            
            if (string.IsNullOrEmpty(currentToken))
            {
                _logger.LogWarning("No token found in configuration");
                return await RefreshTokenAsync();
            }

            if (await IsTokenValidAsync(currentToken))
            {
                return currentToken;
            }

            _logger.LogInformation("Token is expired, refreshing...");
            return await RefreshTokenAsync();
        }

        public Task<bool> IsTokenValidAsync(string token)
        {
            try
            {
                if (string.IsNullOrEmpty(token))
                    return Task.FromResult(false);

                var handler = new JwtSecurityTokenHandler();
                var jsonToken = handler.ReadJwtToken(token);

                // Check if token is expired (with 5 minute buffer)
                var expiryTime = jsonToken.ValidTo;
                var bufferTime = TimeSpan.FromMinutes(5);
                
                return Task.FromResult(DateTime.UtcNow.Add(bufferTime) < expiryTime);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error validating token");
                return Task.FromResult(false);
            }
        }

        public async Task<string> RefreshTokenAsync()
        {
            try
            {
                var httpClient = _httpClientFactory.CreateClient();
                var tokenUrl = "https://api.yarodataservices.com/api/token";

                _logger.LogInformation("Requesting new token from {TokenUrl}", tokenUrl);

                // Set timeout for the request
                httpClient.Timeout = TimeSpan.FromSeconds(30);

                // Add the current token as Bearer token if available
                var currentToken = _configuration["Yaro:Token"];
                if (!string.IsNullOrEmpty(currentToken))
                {
                    httpClient.DefaultRequestHeaders.Authorization = 
                        new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", currentToken);
                    _logger.LogInformation("Including current token in Authorization header for refresh");
                }

                var response = await httpClient.PostAsync(tokenUrl, null);
                
                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    _logger.LogError("Failed to refresh token. Status: {StatusCode}, Content: {Content}", 
                        response.StatusCode, errorContent);
                    throw new Exception($"Failed to refresh token: {response.StatusCode} - {errorContent}");
                }

                var responseContent = await response.Content.ReadAsStringAsync();
                
                if (string.IsNullOrWhiteSpace(responseContent))
                {
                    _logger.LogError("Received empty response from token API");
                    throw new Exception("Received empty response from token API");
                }

                var tokenResponse = JsonSerializer.Deserialize<TokenResponse>(responseContent, new JsonSerializerOptions 
                { 
                    PropertyNameCaseInsensitive = true 
                });

                if (string.IsNullOrEmpty(tokenResponse?.Token))
                {
                    _logger.LogError("Received empty or invalid token from API. Response: {Response}", responseContent);
                    throw new Exception("Received empty or invalid token from API");
                }

                // Validate the new token before saving
                if (!await IsTokenValidAsync(tokenResponse.Token))
                {
                    _logger.LogError("Received invalid token from API");
                    throw new Exception("Received invalid token from API");
                }

                // Update appsettings.json
                await UpdateAppSettingsAsync(tokenResponse.Token);

                _logger.LogInformation("Token successfully refreshed and saved. Valid until: {ExpiryTime}", 
                    GetTokenExpiryTime(tokenResponse.Token));
                return tokenResponse.Token;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Network error while refreshing token");
                throw new Exception("Network error while refreshing token", ex);
            }
            catch (TaskCanceledException ex)
            {
                _logger.LogError(ex, "Timeout while refreshing token");
                throw new Exception("Timeout while refreshing token", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error refreshing token");
                throw;
            }
        }

        private DateTime? GetTokenExpiryTime(string token)
        {
            try
            {
                var handler = new JwtSecurityTokenHandler();
                var jsonToken = handler.ReadJwtToken(token);
                return jsonToken.ValidTo;
            }
            catch
            {
                return null;
            }
        }

        private async Task UpdateAppSettingsAsync(string newToken)
        {
            try
            {
                if (!File.Exists(_configPath))
                {
                    _logger.LogWarning("appsettings.json not found at {ConfigPath}", _configPath);
                    return;
                }

                var json = await File.ReadAllTextAsync(_configPath);
                var configDocument = JsonDocument.Parse(json);
                var root = configDocument.RootElement;

                // Build the updated configuration while preserving existing structure
                var updatedConfig = new Dictionary<string, object>();

                // Copy all existing sections
                foreach (var property in root.EnumerateObject())
                {
                    if (property.Name == "Yaro")
                    {
                        // Update Yaro section with new token
                        var yaroConfig = new Dictionary<string, object>
                        {
                            ["Token"] = newToken,
                            ["BaseUrlApi"] = _configuration["Yaro:BaseUrlApi"] ?? "https://api.yarodataservices.com"
                        };
                        
                        // Preserve any other Yaro properties
                        if (property.Value.ValueKind == JsonValueKind.Object)
                        {
                            foreach (var yaroProperty in property.Value.EnumerateObject())
                            {
                                if (yaroProperty.Name != "Token" && yaroProperty.Name != "BaseUrlApi")
                                {
                                    yaroConfig[yaroProperty.Name] = JsonSerializer.Deserialize<object>(yaroProperty.Value.GetRawText());
                                }
                            }
                        }
                        
                        updatedConfig[property.Name] = yaroConfig;
                    }
                    else
                    {
                        // Copy other sections as-is
                        updatedConfig[property.Name] = JsonSerializer.Deserialize<object>(property.Value.GetRawText());
                    }
                }

                // Ensure Yaro section exists even if it wasn't in the original config
                if (!updatedConfig.ContainsKey("Yaro"))
                {
                    updatedConfig["Yaro"] = new Dictionary<string, object>
                    {
                        ["Token"] = newToken,
                        ["BaseUrlApi"] = "https://api.yarodataservices.com"
                    };
                }

                var options = new JsonSerializerOptions 
                { 
                    WriteIndented = true,
                    PropertyNamingPolicy = null
                };

                var updatedJson = JsonSerializer.Serialize(updatedConfig, options);
                await File.WriteAllTextAsync(_configPath, updatedJson);

                _logger.LogInformation("Successfully updated appsettings.json with new token at {ConfigPath}", _configPath);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating appsettings.json at {ConfigPath}", _configPath);
                throw;
            }
        }

        private class TokenResponse
        {
            public string Token { get; set; }
        }
    }
}
