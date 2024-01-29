using ShortPaper_API.DTO;
using ShortPaper_API.Entities;
using ShortPaper_API.Helper;
using System.Globalization;

namespace ShortPaper_API.Services.Subjects
{
    public class SubjectService : ISubjectService
    {
        private readonly ShortpaperDbContext _db;

        public SubjectService(ShortpaperDbContext db)
        {
            _db = db;
        }
        public ServiceResponse<List<SubjectDTO>> GetSubjects()
        {
            try
            {
                var subjects = (from a in _db.Subjects
                                     select new SubjectDTO
                                     {
                                         SubjectId = a.SubjectId,
                                         SubjectName = a.SubjectName,
                                     }).ToList();
                var result = new ServiceResponse<List<SubjectDTO>>()
                {
                    httpStatusCode = StatusCodes.Status200OK,
                    Data = subjects
                };

                return result;

            }
            catch (Exception ex)
            {

                var result = new ServiceResponse<List<SubjectDTO>>()
                {
                    httpStatusCode = StatusCodes.Status400BadRequest,
                    ErrorMessage = ex.Message
                };

                return result;
            }
        }

        public ServiceResponse<List<SubjectDTO>> GetSubjectByFilter(string text)
        {
            try
            {
                var subjects = new List<SubjectDTO>();

                if (text == "" || text == null)
                {
                    subjects = (from a in _db.Subjects
                                    select new SubjectDTO
                                    {
                                        SubjectId = a.SubjectId,
                                        SubjectName = a.SubjectName,
                                    }).ToList();
                }
                else
                {
                    subjects = (from a in _db.Subjects
                                where a.SubjectId == text || a.SubjectName == text
                                select new SubjectDTO
                                {
                                    SubjectId = a.SubjectId,
                                    SubjectName = a.SubjectName,
                                }).ToList();
                }

                   
                var result = new ServiceResponse<List<SubjectDTO>>()
                {
                    httpStatusCode = StatusCodes.Status200OK,
                    Data = subjects
                };

                return result;

            }
            catch (Exception ex)
            {

                var result = new ServiceResponse<List<SubjectDTO>>()
                {
                    httpStatusCode = StatusCodes.Status400BadRequest,
                    ErrorMessage = ex.Message
                };

                return result;
            }
        }
    }
}

 
