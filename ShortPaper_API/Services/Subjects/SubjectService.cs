using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

                if (subjects.Count == 0)
                {
                    var noresult = new ServiceResponse<List<SubjectDTO>>
                    {
                        httpStatusCode = StatusCodes.Status404NotFound,
                        ErrorMessage = "no subjects found."
                    };
                    return noresult;
                }
                else
                {
                    var result = new ServiceResponse<List<SubjectDTO>>
                    {
                        httpStatusCode = StatusCodes.Status200OK,
                        Data = subjects
                    };

                    return result;
                }

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


                if (subjects.Count == 0)
                {
                    var noresult = new ServiceResponse<List<SubjectDTO>>
                    {
                        httpStatusCode = StatusCodes.Status404NotFound,
                        ErrorMessage = "no subjects found."
                    };
                    return noresult;
                }
                else
                {
                    var result = new ServiceResponse<List<SubjectDTO>>
                    {
                        httpStatusCode = StatusCodes.Status200OK,
                        Data = subjects
                    };

                    return result;
                }

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

        public ServiceResponse<UpdateSubjectDTO> UpdateStudentSubject(string studentId, UpdateSubjectDTO subject)
        {
            var response = new ServiceResponse<UpdateSubjectDTO>();

            try
            {
                // Retrieve the student from the database
                var student = _db.Students.Include(s => s.StudentsHasSubjects)
                                          .SingleOrDefault(a => a.StudentId == studentId);

                if (student == null)
                {
                    response.ErrorMessage = "Student not found";
                    response.httpStatusCode = StatusCodes.Status404NotFound;
                }

                // Find the subject in the student's list of subjects
                var existingSubject = student.StudentsHasSubjects.FirstOrDefault(s => s.SubjectId == subject.SubjectId);

                if (existingSubject == null)
                {
                    // If subject does not exist, create a new one
                    var newSubject = new StudentsHasSubject
                    {
                        StudentId = studentId,
                        SubjectId = subject.SubjectId,
                        IsRegisteredSubject = subject.IsRegisteredSubject ? 1UL : 0UL,
                        IsPaperSubject = subject.IsPaperSubject ? 1UL : 0UL
                    };
                    student.StudentsHasSubjects.Add(newSubject);

                    // Remove other subjects if existingSubject is both registered and paper subject
                    if (subject.IsRegisteredSubject && subject.IsPaperSubject)
                    {
                        var otherSubjects = student.StudentsHasSubjects.Where(s => s.SubjectId != subject.SubjectId).ToList();
                        foreach (var otherSubject in otherSubjects)
                        {
                            _db.Remove(otherSubject);
                        }
                    }
                }
                else
                {
                    // Update the properties of the existing subject
                    existingSubject.IsRegisteredSubject = subject.IsRegisteredSubject ? 1UL : 0UL;
                    existingSubject.IsPaperSubject = subject.IsPaperSubject ? 1UL : 0UL;

                    // Remove other subjects if existingSubject is both registered and paper subject
                    if (subject.IsRegisteredSubject && subject.IsPaperSubject)
                    {
                        var otherSubjects = student.StudentsHasSubjects.Where(s => s.SubjectId != subject.SubjectId).ToList();
                        foreach (var otherSubject in otherSubjects)
                        {
                            _db.Remove(otherSubject);
                        }
                    }
                }

                _db.SaveChanges();

                response.IsSuccess = true;
                response.Data = subject;
            }
            catch (Exception ex)
            {
                response.ErrorMessage = "An unexpected error occurred";
                response.httpStatusCode = StatusCodes.Status500InternalServerError;
            }

            return response;
        }
    }
}

 
