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
                var file = (from a in _db.Articles
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
                                    SubjectName = subjectOfArticle.SubjectName
                                }
                            }).ToList();

                var result = new ServiceResponse<List<ArticleDTO>>
                {
                    Data = file,
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
    }
}

