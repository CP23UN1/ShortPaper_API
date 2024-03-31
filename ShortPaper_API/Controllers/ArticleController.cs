using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ShortPaper_API.DTO;
using ShortPaper_API.Entities;
using ShortPaper_API.Helper;
using ShortPaper_API.Services.Articles;
using ShortPaper_API.Services.Files;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ShortPaper_API.Controllers
{
    [Route("api")]
    [ApiController]
    public class ArticleController : ControllerBase
    {
        private readonly IArticleService _articleService;
        private readonly ShortpaperDbContext _dbContext;

        public ArticleController(IArticleService articleService, ShortpaperDbContext dbContext)
        {
            _articleService = articleService;
            _dbContext = dbContext;
        }

        [HttpGet]
        [Route("articles")]
        public ServiceResponse<List<ArticleDTO>> GetArticles()
        {
            var getData = _articleService.GetArticles();
            return getData;
        }

        [HttpPost]
        [Route("article/create/from/files")]
        public IActionResult CreateArticlesFromShortpaperFiles()
        {
            try
            {
                _articleService.CreateArticlesFromShortpaperFiles();
                return Ok("Articles created successfully from ShortpaperFiles.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }
    }
}

