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
            var token = await _authService.AuthenticateAsync(model.UserId, model.Password);
            if (token == null)
            {
                return Unauthorized(new { message = "Invalid user ID or password" });
            }

            return Ok(new { token });
        }
    }
}
