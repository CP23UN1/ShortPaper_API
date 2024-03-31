using System;
using ShortPaper_API.DTO;
using ShortPaper_API.Helper;

namespace ShortPaper_API.Services.Articles
{
	public interface IArticleService
	{
        ServiceResponse<List<ArticleDTO>> GetArticles();
        void CreateArticlesFromShortpaperFiles();

    }
}

