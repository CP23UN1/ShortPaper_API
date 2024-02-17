using ShortPaper_API.Entities;

namespace ShortPaper_API.Services.Authentications
{
    public interface IAuthService
    {
        Task<string?> AuthenticateAsync(string email, string password);
    }
}
