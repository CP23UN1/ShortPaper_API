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

        [HttpPost]
        [Route("article/student/{studentId}/{articleId}")]
        public ServiceResponse<StudentsHasArticle> AddArticleToStudent(string studentId, int articleId)
        {
            var result = _articleService.AddArticleToStudent(studentId, articleId);
            return result;
        }

        [HttpGet]
        [Route("article/filter")]
        public ServiceResponse<List<ArticleDTO>> AddArticleToStudent(string filterText)
        {
            var result = _articleService.GetArticlesByFilter(filterText);
            return result;
        }

        [HttpGet]
        [Route("article/favorite/{studentId}")]
        public ServiceResponse<List<ArticleDTO>> GetFavoriteArticles(string studentId)
        {
            var result = _articleService.GetFavoriteArticles(studentId);
            return result;
        }

        [HttpDelete]
        [Route("article/favorite/remove/{studentId}/{articleId}")]
        public string RemoveFromFavorites(string studentId, int articleId)
        {
            var result = _articleService.RemoveFromFavorites(studentId, articleId);
            return result;
        }

        [HttpPost]
        [Route("article/filter/many")]
        public ServiceResponse<List<ArticleDTO>> GetArticlesByManyFilter(FilterArticleDTO filter)
        {
            var result = _articleService.GetArticlesByManyFilter(filter);
            return result;
        }

        [HttpGet]
        [Route("articles/year")]
        public ServiceResponse<List<string>> GetArticleYears()
        {
            var result = _articleService.GetArticleYears();
            return result;
        }
    }
}

