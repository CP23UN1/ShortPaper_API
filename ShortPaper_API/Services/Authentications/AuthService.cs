using Konscious.Security.Cryptography;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ShortPaper_API.Entities;
using ShortPaper_API.Helper;
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

        public async Task<ServiceResponse<string>> AuthenticateAsync(string email, string password)
        {
            try
            {
                // Attempt to retrieve a user from the database based on email
                var student = await _dbContext.Students.SingleOrDefaultAsync(u => u.Email == email);
                var admin = await _dbContext.Admins.SingleOrDefaultAsync(a => a.Email == email);
                var committee = await _dbContext.Committees.SingleOrDefaultAsync(c => c.Email == email);

                // Check if a student, admin, or committee with the given email exists
                if (student != null && VerifyPassword(student.Password, email, password))
                {
                    // Generate JWT token for student
                    var token = GenerateJwtToken(student);
                    return new ServiceResponse<string>
                    {
                        IsSuccess = true,
                        Data = token,
                        httpStatusCode = StatusCodes.Status200OK
                    };
                }
                else if (admin != null && VerifyPassword(admin.Password, email, password))
                {
                    // Generate JWT token for admin
                    var token = GenerateJwtToken(admin);
                    return new ServiceResponse<string>
                    {
                        IsSuccess = true,
                        Data = token,
                        httpStatusCode = StatusCodes.Status200OK
                    };
                }
                else if (committee != null && VerifyPassword(committee.Password, email, password))
                {
                    var token = GenerateJwtToken(committee);
                    return new ServiceResponse<string>
                    {
                        IsSuccess = true,
                        Data = token,
                        httpStatusCode = StatusCodes.Status200OK
                    };
                }
                else
                {
                    // No matching user found, or password doesn't match
                    return new ServiceResponse<string>
                    {
                        IsSuccess = false,
                        ErrorMessage = "Invalid email or password.",
                        httpStatusCode = StatusCodes.Status400BadRequest
                    };
                }
            }
            catch (Exception ex)
            {
                return new ServiceResponse<string>
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message,
                    httpStatusCode = StatusCodes.Status500InternalServerError
                };
            }
        }

        public static bool VerifyPassword(string encodedPassword, string email, string password)
        {
            try
            {
                // Decode the encoded password
                var decodedPassword = DecodePassword(encodedPassword, email);

                // Compare the decoded password with the provided password
                return decodedPassword == password;
            }
            catch (Exception)
            {
                // Handle decoding errors
                return false;
            }
        }

        public static string DecodePassword(string encodedPassword, string email)
        {
            // Decode the encoded password from Base64
            string decodedString = Encoding.UTF8.GetString(Convert.FromBase64String(encodedPassword));

            // Split the decoded string into email and password
            string[] parts = decodedString.Split(':');

            if (parts.Length == 2 && parts[0] == email)
            {
                return parts[1]; // Return the password part
            }
            else
            {
                throw new FormatException("Invalid encoded password format");
            }
        }

        private string GenerateJwtToken(Student student)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["JwtSettings:Secret"]); // Get secret key from configuration
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, student.StudentId)
                    // You can add more claims here as needed
                }),
                Expires = DateTime.UtcNow.AddDays(7), // Token expiry time
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        private string GenerateJwtToken(Admin user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["JwtSettings:Secret"]); // Get secret key from configuration
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.AdminId.ToString()),
                    // Add more claims as needed
                }),
                Expires = DateTime.UtcNow.AddDays(7), // Token expiry time
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        private string GenerateJwtToken(Committee user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["JwtSettings:Secret"]); // Get secret key from configuration
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.CommitteeId.ToString()),
                    // Add more claims as needed
                }),
                Expires = DateTime.UtcNow.AddDays(7), // Token expiry time
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
