using System;
using ShortPaper_API.DTO;
using ShortPaper_API.Entities;
using ShortPaper_API.Helper;
using ShortPaper_API.Services.Files;
using ShortPaper_API.Services.Shortpapers;

namespace ShortPaper_API.Services.Articles
{
    public class ArticleService : IArticleService
    {
        private readonly ShortpaperDbContext _db;

        public ArticleService(ShortpaperDbContext db)
        {
            _db = db;
        }

        public ServiceResponse<List<ArticleDTO>> GetArticles()
        {
            try
            {
                var articles = (from a in _db.Articles
                            join b in _db.Subjects on a.SubjectId equals b.SubjectId
                            into subject
                            from subjectOfArticle in subject.DefaultIfEmpty()
                            select new ArticleDTO
                            {
                                ArticleId = a.ArticleId,
                                Topic = a.Topic,
                                Author = a.Author,
                                FileName = a.FileName,
                                FileType = a.FileType,
                                Year = a.Year,
                                Subjects = new SubjectDTO
                                {
                                    SubjectId = subjectOfArticle.SubjectId,
                                    SubjectName = subjectOfArticle.SubjectName
                                }
                            }).ToList();

                var result = new ServiceResponse<List<ArticleDTO>>
                {
                    Data = articles,
                    httpStatusCode = StatusCodes.Status200OK,
                };

                return result;
            }
            catch (Exception ex)
            {
                var result = new ServiceResponse<List<ArticleDTO>>()
                {
                    httpStatusCode = StatusCodes.Status400BadRequest,
                    ErrorMessage = ex.Message
                };

                return result;
            }
        }

        public void CreateArticlesFromShortpaperFiles()
        {
            var shortpaperFiles = _db.ShortpaperFiles.ToList();

            foreach (var shortpaperFile in shortpaperFiles)
            {
                var shortpaper = _db.Shortpapers.FirstOrDefault(s => s.ShortpaperId == shortpaperFile.ShortpaperId);
                var student = _db.Students.FirstOrDefault(s => s.StudentId == shortpaper.StudentId);
                var subject = _db.StudentsHasSubjects.FirstOrDefault(s => s.StudentId == student.StudentId);
                var fileType = _db.ShortpaperFileTypes.FirstOrDefault(s => s.TypeId == shortpaperFile.ShortpaperFileTypeId);
                var fullName = student.Firstname + " " + student.Lastname;

                var article = new Article
                {
                    Topic = shortpaper?.ShortpaperTopic, // Null conditional operator used to prevent NullReferenceException
                    Author = fullName,
                    FileName = shortpaperFile.FileName,
                    FileSize = shortpaperFile.FileSize,
                    FileType = fileType?.TypeName, // Null conditional operator used to prevent NullReferenceException
                    Year = student?.Year, // Null conditional operator used to prevent NullReferenceException
                    SubjectId = subject?.SubjectId // Null conditional operator used to prevent NullReferenceException
                };


                _db.Articles.Add(article);
            }

            _db.SaveChanges();
        }

        public ServiceResponse<StudentsHasArticle> AddArticleToStudent(string studentId, int articleId)
        {
            var response = new ServiceResponse<StudentsHasArticle>();

            try
            {
                var studentAndArticle = new StudentsHasArticle
                {
                    ArticleId = articleId,
                    StudentId = studentId
                };

                _db.StudentsHasArticles.Add(studentAndArticle);
                _db.SaveChanges();

                response.IsSuccess = true;
                response.Data = studentAndArticle;
            }
            catch (Exception ex)
            {
                response.ErrorMessage = "An unexpected error occurred";
                response.httpStatusCode = StatusCodes.Status500InternalServerError;
            }

            return response;
        }

        public ServiceResponse<List<ArticleDTO>> GetArticlesByFilter(string filterText)
        {
            try
            {
                var articles = new List<ArticleDTO>();
                if(filterText == "" || filterText == null)
                {
                    articles    = (from a in _db.Articles
                                join b in _db.Subjects on a.SubjectId equals b.SubjectId
                                into subject
                                from subjectOfArticle in subject.DefaultIfEmpty()
                                select new ArticleDTO
                                {
                                    ArticleId = a.ArticleId,
                                    Topic = a.Topic,
                                    Author = a.Author,
                                    FileName = a.FileName,
                                    FileType = a.FileType,
                                    Year = a.Year,
                                    Subjects = new SubjectDTO
                                    {
                                        SubjectId = subjectOfArticle.SubjectId,
                                        SubjectName = subjectOfArticle.SubjectName
                                    }
                                }).ToList();
                }
                else
                {
                 articles = (from a in _db.Articles
                            join b in _db.Subjects on a.SubjectId equals b.SubjectId
                            into subject
                            from subjectOfArticle in subject.DefaultIfEmpty()
                            where a.Author == filterText || a.FileName == filterText || a.Topic == filterText || a.Year == filterText || a.FileType == filterText
                            select new ArticleDTO
                            {
                                ArticleId = a.ArticleId,
                                Topic = a.Topic,
                                Author = a.Author,
                                FileName = a.FileName,
                                FileType = a.FileType,
                                Year = a.Year,
                                Subjects = new SubjectDTO
                                {
                                    SubjectId = subjectOfArticle.SubjectId,
                                    SubjectName = subjectOfArticle.SubjectName
                                }
                            }).ToList();
                }
                     

                var result = new ServiceResponse<List<ArticleDTO>>
                {
                    Data = articles,
                    httpStatusCode = StatusCodes.Status200OK,
                };

                return result;
            }
            catch (Exception ex)
            {
                var result = new ServiceResponse<List<ArticleDTO>>()
                {
                    httpStatusCode = StatusCodes.Status400BadRequest,
                    ErrorMessage = ex.Message
                };

                return result;
            }
        }

        public ServiceResponse<List<ArticleDTO>> GetFavoriteArticles(string studentId)
        {
            try
            {
                var favoriteArticles = (from sa in _db.StudentsHasArticles
                                        join a in _db.Articles on sa.ArticleId equals a.ArticleId
                                        where sa.StudentId == studentId
                                        select new ArticleDTO
                                        {
                                            ArticleId = a.ArticleId,
                                            Topic = a.Topic,
                                            Author = a.Author,
                                            FileName = a.FileName,
                                            FileType = a.FileType,
                                            Year = a.Year,
                                            Subjects = new SubjectDTO
                                            {
                                                SubjectId = a.Subject.SubjectId,
                                                SubjectName = a.Subject.SubjectName
                                            }
                                        }).ToList();

                var result = new ServiceResponse<List<ArticleDTO>>
                {
                    Data = favoriteArticles,
                    httpStatusCode = StatusCodes.Status200OK,
                };

                return result;
            }
            catch (Exception ex)
            {
                var result = new ServiceResponse<List<ArticleDTO>>()
                {
                    httpStatusCode = StatusCodes.Status500InternalServerError,
                    ErrorMessage = ex.Message
                };

                return result;
            }
        }

        public string RemoveFromFavorites(string studentId, int articleId)
        {
            var response = new ServiceResponse<object>();

            try
            {
                var favoriteArticle = _db.StudentsHasArticles.FirstOrDefault(sa => sa.StudentId == studentId && sa.ArticleId == articleId);

                if (favoriteArticle != null)
                {
                    _db.StudentsHasArticles.Remove(favoriteArticle);
                    _db.SaveChanges();

                    return "Article removed from favorites successfully.";
                }
                else
                {
                    return "Article is not in favorites.";
                }
            }
            catch (Exception ex)
            {
                return "An unexpected error occurred while removing article from favorites.";
            }
        }

        public ServiceResponse<List<ArticleDTO>> GetArticlesByManyFilter(FilterArticleDTO filter)
        {
            try
            {
                var articlesQuery = _db.Articles
                    .Join(_db.Subjects, a => a.SubjectId, b => b.SubjectId, (a, b) => new { Article = a, Subject = b });

                if (!string.IsNullOrEmpty(filter.Year))
                {
                    articlesQuery = articlesQuery.Where(j => j.Article.Year.Contains(filter.Year));
                }

                if (!string.IsNullOrEmpty(filter.FileName))
                {
                    articlesQuery = articlesQuery.Where(j => j.Article.FileName.Contains(filter.FileName));
                }

                if (!string.IsNullOrEmpty(filter.TopicOrAuthor))
                {
                    articlesQuery = articlesQuery.Where(j => j.Article.Author.Contains(filter.TopicOrAuthor) || j.Article.Topic.Contains(filter.TopicOrAuthor));
                }

                if (!string.IsNullOrEmpty(filter.FileType))
                {
                    articlesQuery = articlesQuery.Where(j => j.Article.FileType.Contains(filter.FileType));
                }

                if(!string.IsNullOrEmpty(filter.Subject))
                {
                    articlesQuery = articlesQuery.Where(j => j.Subject.SubjectName.Contains(filter.Subject));
                }

                var articles = articlesQuery
                    .Select(j => new ArticleDTO
                    {
                        ArticleId = j.Article.ArticleId,
                        Topic = j.Article.Topic,
                        Author = j.Article.Author,
                        FileName = j.Article.FileName,
                        FileType = j.Article.FileType,
                        Year = j.Article.Year,
                        Subjects = new SubjectDTO
                        {
                            SubjectId = j.Subject.SubjectId,
                            SubjectName = j.Subject.SubjectName
                        }
                    })
                    .ToList();

                var result = new ServiceResponse<List<ArticleDTO>>
                {
                    Data = articles,
                    httpStatusCode = StatusCodes.Status200OK,
                };

                return result;
            }
            catch (Exception ex)
            {
                var result = new ServiceResponse<List<ArticleDTO>>()
                {
                    httpStatusCode = StatusCodes.Status400BadRequest,
                    ErrorMessage = ex.Message
                };

                return result;
            }
        }
    }
}

