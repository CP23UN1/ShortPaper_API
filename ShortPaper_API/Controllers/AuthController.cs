using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol.Plugins;
using ShortPaper_API.DTO;
using ShortPaper_API.Services.Authentications;

namespace ShortPaper_API.Controllers
{
    [Route("api")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        [AllowAnonymous] // Allow anonymous access to login endpoint
        public async Task<IActionResult> Login(LoginDTO model)
        {
            var response = await _authService.AuthenticateAsync(model.Email, model.Password);

            if (!response.IsSuccess)
            {
                return Unauthorized(new { message = response.ErrorMessage });
            }

            return Ok(new { token = response.Data });
        }
    }
}
