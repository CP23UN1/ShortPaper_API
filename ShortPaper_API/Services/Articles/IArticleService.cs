﻿using System;
using ShortPaper_API.DTO;
using ShortPaper_API.Entities;
using ShortPaper_API.Helper;

namespace ShortPaper_API.Services.Articles
{
	public interface IArticleService
	{
        ServiceResponse<List<ArticleDTO>> GetArticles();
        void CreateArticlesFromShortpaperFiles();
        ServiceResponse<StudentsHasArticle> AddArticleToStudent(string studentId, int articleId);
        ServiceResponse<List<ArticleDTO>> GetArticlesByFilter(string filterText);

    }
}

