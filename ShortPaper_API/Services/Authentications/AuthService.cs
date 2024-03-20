using Konscious.Security.Cryptography;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ShortPaper_API.Entities;
using ShortPaper_API.Helper;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Text.Json;


namespace ShortPaper_API.Services.Authentications
{
    public class AuthService : IAuthService
    {
        private readonly HttpClient _httpClient;
        private readonly ShortpaperDbContext _dbContext;
        private readonly IConfiguration _configuration;

        public AuthService(ShortpaperDbContext dbContext, IConfiguration configuration, HttpClient httpClient)
        {
            _dbContext = dbContext;
            _configuration = configuration;
            _httpClient = httpClient;
        }
        public async Task<AuthResponse> AuthenticateAsync(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                return new AuthResponse
                {
                    IsSuccess = false,
                    ErrorMessage = "Invalid username or password"
                };
            }

            var formData = new FormUrlEncodedContent(new[]
            {
        new KeyValuePair<string, string>("username", username),
        new KeyValuePair<string, string>("password", password),
        new KeyValuePair<string, string>("client_secret", "Inz9aNsu1zJNDkbt0T1uHM75hMaSnQgm"),
        new KeyValuePair<string, string>("grant_type", "password"),
        new KeyValuePair<string, string>("client_id", "auth-cp23un1")
    });

            var response = await _httpClient.PostAsync("https://login.sit.kmutt.ac.th/realms/student-project/protocol/openid-connect/token", formData);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var tokenResponse = JsonSerializer.Deserialize<TokenReponse>(content);

                return new AuthResponse
                {
                    AccessToken = tokenResponse?.access_token,
                    DecodedToken = DecodeToken(tokenResponse?.access_token)
                };
            }
            else
            {
                return new AuthResponse
                {
                    IsSuccess = false,
                    ErrorMessage = "Authentication failed"
                };
            }
        }


        public class TokenReponse
        {
            public string access_token { get; set; }
        }

        public class DecodedToken
        {
            public string Name { get; set; }
            public string PreferredUsername { get; set; }
            public string GroupId { get; set; }
            public string Email { get; set; }
        }

        private DecodedToken DecodeToken(string accessToken)
        {
            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(accessToken);

            return new DecodedToken
            {
                Name = token.Claims.FirstOrDefault(c => c.Type == "name")?.Value,
                PreferredUsername = token.Claims.FirstOrDefault(c => c.Type == "preferred_username")?.Value,
                GroupId = token.Claims.FirstOrDefault(c => c.Type == "group_id")?.Value,
                Email = token.Claims.FirstOrDefault(c => c.Type == "email")?.Value
                // Add more properties as needed
            };
        }

        public class AuthResponse
        {
            public string AccessToken { get; set; }
            public DecodedToken DecodedToken { get; set; }
            public bool IsSuccess { get; set; }
            public string ErrorMessage { get; set; }

            // Constructor to initialize IsSuccess and ErrorMessage properties
            public AuthResponse()
            {
                IsSuccess = true; // Default value assuming success
            }
        }

    }
}
