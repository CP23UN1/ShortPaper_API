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
        public async Task<IActionResult> Login([FromBody] LoginDTO model)
        {
            try
            {
                var result = await _authService.AuthenticateAsync(model.Username, model.Password);

                if (result.IsSuccess)
                {
                    return Ok(new { Token = result.AccessToken, DecodedToken = result.DecodedToken });
                }
                else
                {
                    return BadRequest(new { Message = result.ErrorMessage });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Internal server error" });
            }
        }
    }
}
