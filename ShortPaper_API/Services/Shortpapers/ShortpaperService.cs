using ShortPaper_API.DTO;
using ShortPaper_API.Entities;
using ShortPaper_API.Helper;

namespace ShortPaper_API.Services.Shortpapers
{
    public class ShortpaperService : IShortpaperService
    {
        private readonly ShortpaperDbContext _db;

        public ShortpaperService(ShortpaperDbContext db)
        {
            _db = db;
        }

        public ServiceResponse<List<ShortpaperDTO>> GetShortpaper()
        {
            try
            {
                var shortpapers = (from shortpaper in _db.Shortpapers
                                   join hasCommittee in _db.ShortpapersHasCommittees on shortpaper.ShortpaperId equals hasCommittee.ShortpaperId
                                   into shortperHasCommittee
                                   from sphc in shortperHasCommittee.DefaultIfEmpty()
                                   join c in _db.Committees on sphc.CommitteeId equals c.CommitteeId
                                   into shortCommittee
                                   from sct in shortCommittee.DefaultIfEmpty()
                                   join student in _db.Students on shortpaper.StudentId equals student.StudentId
                                   into shortpaperStudent
                                   from s in shortpaperStudent.DefaultIfEmpty()
                                   join shs in _db.StudentsHasSubjects on shortpaper.StudentId equals shs.StudentId
                                   into studentHasSubjects
                                   from sshs in studentHasSubjects.DefaultIfEmpty()
                                   join sub in _db.Subjects on sshs.SubjectId equals sub.SubjectId
                                   into shortpaperSubjects
                                   from sps in shortpaperSubjects.DefaultIfEmpty()
                                   select new 
                                   {
                                       Shortpaper = new ShortpaperDTO {
                                            ShortpaperId = shortpaper.ShortpaperId,
                                            ShortpaperTopic = shortpaper.ShortpaperTopic,
                                            Student = s != null ? new StudentForShortpaperDTO
                                            {
                                               StudentId = s.StudentId,
                                               Firstname = s.Firstname,
                                               Lastname = s.Lastname,
                                               Email = s.Email,
                                            } : null,
                                       },
                                       //Subjects = (from sps in _db.Subjects
                                       //            where sps.SubjectId == shortpaper.SubjectId
                                       //            select new SubjectDTO
                                       //            {
                                       //                SubjectId = sps.SubjectId,
                                       //                SubjectName = sps.SubjectName,
                                       //            }).FirstOrDefault(),
                                       Subjects = (from spf in _db.Subjects
                                                   where spf.SubjectId == sshs.SubjectId
                                                   select new SubjectDTO
                                                   {
                                                       SubjectId = spf.SubjectId,
                                                       SubjectName = spf.SubjectName,
                                                       IsRegisteredSubject = sshs.IsRegisteredSubject,
                                                       IsPaperSubject = sshs.IsPaperSubject
                                                   }).ToList(),
                                       Committee = sct != null ? new CommitteeDTO
                                       {
                                           CommitteeId = sct.CommitteeId,
                                           Firstname = sct.Firstname,
                                           Lastname = sct.Lastname,
                                           Email = sct.Email,
                                           AlternativeEmail = sct.AlternativeEmail,
                                           Phonenumber = sct.Phonenumber,
                                           IsAdvisor = sphc.IsAdvisor,
                                           IsCommittee = sphc.IsCommittee,
                                           IsPrincipal = sphc.IsPrincipal,
                                       } : null,
                                   }).GroupBy(x => x.Shortpaper.ShortpaperId) // Group by ShortpaperId
                .Select(group => new ShortpaperDTO
                {
                    // Select properties from the first item in the group (assuming Shortpaper properties are the same for each group)
                    ShortpaperId = group.First().Shortpaper.ShortpaperId,
                    ShortpaperTopic = group.First().Shortpaper.ShortpaperTopic,
                    //Subjects = group.First().Subjects,
                    Student = group.First().Shortpaper.Student,
                    Subjects = group.First().Subjects,
                    Committees = group.Select(x => x.Committee).ToList(),
                })
                .ToList();

                if(shortpapers.Count == 0)
                {
                    var noresult = new ServiceResponse<List<ShortpaperDTO>>()
                    {
                        httpStatusCode = StatusCodes.Status404NotFound,
                        ErrorMessage = "no shortpaper found."
                    };
                    return noresult;
                }
                else
                {
                    var result = new ServiceResponse<List<ShortpaperDTO>>()
                    {
                        httpStatusCode = StatusCodes.Status200OK,
                        Data = shortpapers
                    };

                    return result;
                }

            }
            catch (Exception ex)
            {

                var result = new ServiceResponse<List<ShortpaperDTO>>()
                {
                    httpStatusCode = StatusCodes.Status400BadRequest,
                    ErrorMessage = ex.Message
                };

                return result;
            }
        }

        public ServiceResponse<List<ShortpaperDTO>> GetShortpaperByFilter(string searchText)
        {
            try
            {
                var shortpapers = new List<ShortpaperDTO>();

                if (searchText == "" || searchText == null)
                {
                    shortpapers = (from shortpaper in _db.Shortpapers
                                   join hasCommittee in _db.ShortpapersHasCommittees on shortpaper.ShortpaperId equals hasCommittee.ShortpaperId
                                   into shortperHasCommittee
                                   from sphc in shortperHasCommittee.DefaultIfEmpty()
                                   join c in _db.Committees on sphc.CommitteeId equals c.CommitteeId
                                   into ShortCommittee
                                   from sct in ShortCommittee.DefaultIfEmpty()
                                   join student in _db.Students on shortpaper.StudentId equals student.StudentId
                                   into shortpaperStudent
                                   from s in shortpaperStudent.DefaultIfEmpty()
                                   join shs in _db.StudentsHasSubjects on shortpaper.StudentId equals shs.StudentId
                                   into studentHasSubjects
                                   from sshs in studentHasSubjects.DefaultIfEmpty()
                                   select new
                                   {
                                       Shortpaper = new ShortpaperDTO
                                       {
                                           ShortpaperId = shortpaper.ShortpaperId,
                                           ShortpaperTopic = shortpaper.ShortpaperTopic,
                                           Student = s != null ? new StudentForShortpaperDTO
                                           {
                                               StudentId = s.StudentId,
                                               Firstname = s.Firstname,
                                               Lastname = s.Lastname,
                                               Email = s.Email,
                                           } : null,
                                       },
                                       Subjects = (from spf in _db.Subjects
                                               where spf.SubjectId == sshs.SubjectId
                                               select new SubjectDTO
                                               {
                                                   SubjectId = spf.SubjectId,
                                                   SubjectName = spf.SubjectName,
                                                   IsRegisteredSubject = sshs.IsRegisteredSubject,
                                                   IsPaperSubject = sshs.IsPaperSubject
                                               }).ToList(),
                                       Committee = sct != null ? new CommitteeDTO
                                       {
                                           CommitteeId = sct.CommitteeId,
                                           Firstname = sct.Firstname,
                                           Lastname = sct.Lastname,
                                           Email = sct.Email,
                                           AlternativeEmail = sct.AlternativeEmail,
                                           Phonenumber = sct.Phonenumber,
                                           IsAdvisor = sphc.IsAdvisor,
                                           IsCommittee = sphc.IsCommittee,
                                           IsPrincipal = sphc.IsPrincipal,
                                       } : null,
                                   }).GroupBy(x => x.Shortpaper.ShortpaperId) // Group by ShortpaperId
                .Select(group => new ShortpaperDTO
                {
                    // Select properties from the first item in the group (assuming Shortpaper properties are the same for each group)
                    ShortpaperId = group.First().Shortpaper.ShortpaperId,
                    ShortpaperTopic = group.First().Shortpaper.ShortpaperTopic,
                    Student = group.First().Shortpaper.Student,
                    Subjects = group.First().Subjects,
                    Committees = group.Select(x => x.Committee).ToList(),
                })
                .ToList();
                }
                else
                {
                    shortpapers = (from shortpaper in _db.Shortpapers
                                   join hasCommittee in _db.ShortpapersHasCommittees on shortpaper.ShortpaperId equals hasCommittee.ShortpaperId
                                   into shortperHasCommittee
                                   from sphc in shortperHasCommittee.DefaultIfEmpty()
                                   join c in _db.Committees on sphc.CommitteeId equals c.CommitteeId
                                   into ShortCommittee
                                   from sct in ShortCommittee.DefaultIfEmpty()
                                   join student in _db.Students on shortpaper.StudentId equals student.StudentId
                                   into shortpaperStudent
                                   from s in shortpaperStudent.DefaultIfEmpty()
                                   join shs in _db.StudentsHasSubjects on s.StudentId equals shs.StudentId
                                   into studentHasSubjects
                                   from sshs in studentHasSubjects.DefaultIfEmpty()
                                       //where (shortpaper.ShortpaperTopic.Contains(searchText) || shortpaper.SubjectId.Contains(searchText) || s.StudentId.Contains(searchText) || s.Year.Contains(searchText))
                                   where (shortpaper.ShortpaperTopic.Contains(searchText) || s.StudentId.Contains(searchText) || s.Year.Contains(searchText))
                                   select new
                                   {
                                       Shortpaper = new ShortpaperDTO
                                       {
                                           ShortpaperId = shortpaper.ShortpaperId,
                                           ShortpaperTopic = shortpaper.ShortpaperTopic,
                                           Student = s != null ? new StudentForShortpaperDTO
                                           {
                                               StudentId = s.StudentId,
                                               Firstname = s.Firstname,
                                               Lastname = s.Lastname,
                                               Email = s.Email,
                                           } : null,
                                       },
                                       //Subjects = (from sps in _db.Subjects
                                       //            where sps.SubjectId == shortpaper.SubjectId
                                       //            select new SubjectDTO
                                       //            {
                                       //                SubjectId = sps.SubjectId,
                                       //                SubjectName = sps.SubjectName,
                                       //            }).FirstOrDefault(),
                                       Subjects = (from spf in _db.Subjects
                                                   where spf.SubjectId == sshs.SubjectId
                                                   select new SubjectDTO
                                                   {
                                                       SubjectId = spf.SubjectId,
                                                       SubjectName = spf.SubjectName,
                                                       IsRegisteredSubject = sshs.IsRegisteredSubject,
                                                       IsPaperSubject = sshs.IsPaperSubject
                                                   }).ToList(),
                                       Committee = sct != null ? new CommitteeDTO
                                       {
                                           CommitteeId = sct.CommitteeId,
                                           Firstname = sct.Firstname,
                                           Lastname = sct.Lastname,
                                           Email = sct.Email,
                                           AlternativeEmail = sct.AlternativeEmail,
                                           Phonenumber = sct.Phonenumber,
                                           IsAdvisor = sphc.IsAdvisor,
                                           IsCommittee = sphc.IsCommittee,
                                           IsPrincipal = sphc.IsPrincipal,
                                       } : null,
                                   }).GroupBy(x => x.Shortpaper.ShortpaperId) // Group by ShortpaperId
                .Select(group => new ShortpaperDTO
                {
                    // Select properties from the first item in the group (assuming Shortpaper properties are the same for each group)
                    ShortpaperId = group.First().Shortpaper.ShortpaperId,
                    ShortpaperTopic = group.First().Shortpaper.ShortpaperTopic,
                    //Subjects = group.First().Subjects,
                    Student = group.First().Shortpaper.Student,
                    Subjects = group.First().Subjects,
                    Committees = group.Select(x => x.Committee).ToList(),
                })
                .ToList();
                }

                if (shortpapers.Count == 0)
                {
                    var noresult = new ServiceResponse<List<ShortpaperDTO>>()
                    {
                        httpStatusCode = StatusCodes.Status404NotFound,
                        ErrorMessage = "no shortpaper found."
                    };
                    return noresult;
                }
                else
                {
                    var result = new ServiceResponse<List<ShortpaperDTO>>()
                    {
                        httpStatusCode = StatusCodes.Status200OK,
                        Data = shortpapers
                    };

                    return result;
                }
            }
            catch (Exception ex)
            {

                var result = new ServiceResponse<List<ShortpaperDTO>>()
                {
                    httpStatusCode = StatusCodes.Status400BadRequest,
                    ErrorMessage = ex.Message
                };

                return result;
            }
        }

        public ServiceResponse<ShortpaperDTO> GetShortpaperByStudent(string id)
        {
            try
            {
                var shortpapers = (from shortpaper in _db.Shortpapers
                                   join hasCommittee in _db.ShortpapersHasCommittees on shortpaper.ShortpaperId equals hasCommittee.ShortpaperId
                                   into shortperHasCommittee
                                   from sphc in shortperHasCommittee.DefaultIfEmpty()
                                   join c in _db.Committees on sphc.CommitteeId equals c.CommitteeId
                                   into shortCommittee
                                   from sct in shortCommittee.DefaultIfEmpty()
                                   join student in _db.Students on shortpaper.StudentId equals student.StudentId
                                   into shortpaperStudent
                                   from s in shortpaperStudent.DefaultIfEmpty()
                                   join shs in _db.StudentsHasSubjects on shortpaper.StudentId equals shs.StudentId
                                   into studentHasSubjects
                                   from sshs in studentHasSubjects.DefaultIfEmpty()
                                   where shortpaper.StudentId == id
                                   select new
                                   {
                                       Shortpaper = new ShortpaperDTO
                                       {
                                           ShortpaperId = shortpaper.ShortpaperId,
                                           ShortpaperTopic = shortpaper.ShortpaperTopic,
                                           Student = s != null ? new StudentForShortpaperDTO
                                           {
                                               StudentId = s.StudentId,
                                               Firstname = s.Firstname,
                                               Lastname = s.Lastname,
                                               Email = s.Email,
                                           } : null,
                                       },
                                       //Subjects = (from sps in _db.Subjects
                                       //            where sps.SubjectId == shortpaper.SubjectId
                                       //            select new SubjectDTO
                                       //            {
                                       //                SubjectId = sps.SubjectId,
                                       //                SubjectName = sps.SubjectName,
                                       //            }).FirstOrDefault(),
                                       Subjects = (from spf in _db.Subjects
                                                   where spf.SubjectId == sshs.SubjectId
                                                   select new SubjectDTO
                                                   {
                                                       SubjectId = spf.SubjectId,
                                                       SubjectName = spf.SubjectName,
                                                       IsRegisteredSubject = sshs.IsRegisteredSubject,
                                                       IsPaperSubject = sshs.IsPaperSubject
                                                   }).ToList(),
                                       Committee = sct != null ? new CommitteeDTO
                                       {
                                           CommitteeId = sct.CommitteeId,
                                           Firstname = sct.Firstname,
                                           Lastname = sct.Lastname,
                                           Email = sct.Email,
                                           AlternativeEmail = sct.AlternativeEmail,
                                           Phonenumber = sct.Phonenumber,
                                           IsAdvisor = sphc.IsAdvisor,
                                           IsCommittee = sphc.IsCommittee,
                                           IsPrincipal = sphc.IsPrincipal,
                                       } : null,
                                   }).GroupBy(x => x.Shortpaper.ShortpaperId)
                .Select(group => new ShortpaperDTO
                {
                    ShortpaperId = group.First().Shortpaper.ShortpaperId,
                    ShortpaperTopic = group.First().Shortpaper.ShortpaperTopic,
                    //Subjects = group.First().Subjects,
                    Student = group.First().Shortpaper.Student,
                    Subjects = group.First().Subjects,
                    Committees = group.Select(x => x.Committee).ToList(),
                })
                .FirstOrDefault();

                if (shortpapers == null)
                {
                    var noresult = new ServiceResponse<ShortpaperDTO>()
                    {
                        httpStatusCode = StatusCodes.Status404NotFound,
                        ErrorMessage = "no shortpaper found."
                    };

                    return noresult;
                }
                else
                {
                    var result = new ServiceResponse<ShortpaperDTO>()
                    {
                        httpStatusCode = StatusCodes.Status200OK,
                        Data = shortpapers
                    };

                    return result;
                }

            }
            catch (Exception ex)
            {

                var result = new ServiceResponse<ShortpaperDTO>()
                {
                    httpStatusCode = StatusCodes.Status400BadRequest,
                    ErrorMessage = ex.Message
                };

                return result;
            }
        }

        public ServiceResponse<AddShortpaperDTO> AddShortpaper(AddShortpaperDTO addShortpaperDTO)
        {
            var response = new ServiceResponse<AddShortpaperDTO>();

            try
            {
                // Check if the student exists
                var existingStudent = _db.Students.FirstOrDefault(s => s.StudentId == addShortpaperDTO.StudentId);
                if (existingStudent == null)
                {
                    response.ErrorMessage = "Cannot found student id";
                    response.httpStatusCode = StatusCodes.Status400BadRequest;
                    return response;
                }

                // Check if the subject exists
                //var existingSubject = _db.Subjects.FirstOrDefault(s => s.SubjectId == addShortpaperDTO.SubjectId);
                //if (existingSubject == null)
                //{
                //    response.ErrorMessage = "Cannot found subject id";
                //    response.httpStatusCode = StatusCodes.Status400BadRequest;
                //    return response;
                //}

                // Create a new Shortpaper entity
                var newShortpaper = new Shortpaper
                {
                    ShortpaperTopic = addShortpaperDTO.ShortpaperTopic,
                    StudentId = addShortpaperDTO.StudentId,
                    //SubjectId = addShortpaperDTO.SubjectId
                };

                // Add the new Shortpaper to the database
                _db.Shortpapers.Add(newShortpaper);
                _db.SaveChanges();

                response.IsSuccess = true;
                response.Data = addShortpaperDTO;
            }
            catch (Exception ex)
            {
                response.ErrorMessage = "An unexpected error occurred";
                response.httpStatusCode = StatusCodes.Status500InternalServerError;

            }
            return response;
        }

        public ServiceResponse<UpdateShortpaperDTO> UpdateShortpaper(UpdateShortpaperDTO shortpaperDTO)
        {
            var response = new ServiceResponse<UpdateShortpaperDTO>();

            try
            {
                // Check if the subject exists
                //var existingSubject = _db.Subjects.FirstOrDefault(s => s.SubjectId == shortpaperDTO.SubjectId);
                //if (existingSubject == null)
                //{
                //    response.ErrorMessage = "Cannot found subject id";
                //    response.httpStatusCode = StatusCodes.Status400BadRequest;
                //    return response;
                //}

                var updateShortpaper = (from a in _db.Shortpapers
                                     where a.ShortpaperId == shortpaperDTO.ShortpaperId
                                     select a).FirstOrDefault();

                updateShortpaper.ShortpaperTopic = shortpaperDTO.ShortpaperTopic;
                //updateShortpaper.SubjectId = shortpaperDTO.SubjectId;

                _db.SaveChanges();

                response.IsSuccess = true;
                response.Data = shortpaperDTO;
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
