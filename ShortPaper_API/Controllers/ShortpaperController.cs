using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShortPaper_API.DTO;
using ShortPaper_API.Entities;
using ShortPaper_API.Helper;
using ShortPaper_API.Services.Shortpapers;
using ShortPaper_API.Services.Students;

namespace ShortPaper_API.Controllers
{
    [Route("api")]
    [ApiController]
    public class ShortpaperController : ControllerBase
    {
        private readonly ShortpaperDbContext _dbContext;
        private readonly IShortpaperService _shortpaperService;

        public ShortpaperController(ShortpaperDbContext dbContext, IShortpaperService shortpaperService)

        {
            _dbContext = dbContext;
            _shortpaperService = shortpaperService;
        }

        [HttpGet]
        [Route("shortpapers")]
        public ServiceResponse<List<ShortpaperDTO>> GetShortpaper()
        {
            var shortpapers = _shortpaperService.GetShortpaper();
            return shortpapers;
        }

        [HttpGet]
        [Route("shortpapers/{filterText}")]
        public ServiceResponse<List<ShortpaperDTO>> GetStudentByFilter(string filterText)
        {
            var shortpaper = _shortpaperService.GetShortpaperByFilter(filterText);
            return shortpaper;
        }
    }
}
