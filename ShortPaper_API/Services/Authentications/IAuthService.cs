using ShortPaper_API.Entities;
using ShortPaper_API.Helper;
using static ShortPaper_API.Services.Authentications.AuthService;

namespace ShortPaper_API.Services.Authentications
{
    public interface IAuthService
    {
        Task<AuthResponse> AuthenticateAsync(string username, string password);
    }
}
