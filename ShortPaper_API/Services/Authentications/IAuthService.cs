using ShortPaper_API.Entities;
using ShortPaper_API.Helper;

namespace ShortPaper_API.Services.Authentications
{
    public interface IAuthService
    {
        Task<ServiceResponse<string>> AuthenticateAsync(string email, string password);
    }
}
