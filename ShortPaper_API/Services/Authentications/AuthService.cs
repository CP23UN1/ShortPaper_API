using Konscious.Security.Cryptography;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ShortPaper_API.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ShortPaper_API.Services.Authentications
{
    public class AuthService : IAuthService
    {
        private readonly ShortpaperDbContext _dbContext;
        private readonly IConfiguration _configuration;

        public AuthService(ShortpaperDbContext dbContext, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _configuration = configuration;
        }

        public async Task<string?> AuthenticateAsync(string userId, string password)
        {
            // Retrieve user from the database based on userId
            var user = await _dbContext.Students.SingleOrDefaultAsync(u => u.StudentId == userId);

            if (user == null || !VerifyPassword(user.Password, userId, password))
            {
                return null; // Invalid user ID or password
            }

            // Generate JWT token
            var token = GenerateJwtToken(user);
            return token;
        }

        public static bool VerifyPassword(string encodedPassword, string userId, string password)
        {
            try
            {
                // Decode the encoded password
                var decodedPassword = DecodePassword(encodedPassword);

                // Compare the decoded password with the provided password
                return decodedPassword == password;
            }
            catch (Exception)
            {
                // Handle decoding errors
                return false;
            }
        }

        public static string DecodePassword(string encodedPassword)
        {
            // Decode the encoded password from Base64
            string decodedString = Encoding.UTF8.GetString(Convert.FromBase64String(encodedPassword));

            // Split the decoded string into user ID and password
            string[] parts = decodedString.Split(':');

            if (parts.Length == 2)
            {
                return parts[1]; // Return the password part
            }
            else
            {
                throw new FormatException("Invalid encoded password format");
            }
        }

        private string GenerateJwtToken(Student user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["JwtSettings:Secret"]); // Get secret key from configuration
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                new Claim(ClaimTypes.NameIdentifier, user.StudentId)
                    // You can add more claims here as needed
                }),
                Expires = DateTime.UtcNow.AddDays(7), // Token expiry time
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
