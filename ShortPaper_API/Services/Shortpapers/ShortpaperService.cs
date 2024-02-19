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
                                       Subjects = (from sps in _db.Subjects
                                                   where sps.SubjectId == shortpaper.SubjectId
                                                   select new SubjectDTO
                                                   {
                                                       SubjectId = sps.SubjectId,
                                                       SubjectName = sps.SubjectName,
                                                   }).FirstOrDefault(),
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
                    Subjects = group.First().Subjects,
                    Student = group.First().Shortpaper.Student,
                    Committees = group.Select(x => x.Committee).ToList(),
                })
                .ToList();

                var result = new ServiceResponse<List<ShortpaperDTO>>()
                {
                    httpStatusCode = StatusCodes.Status200OK,
                    Data = shortpapers
                };

                return result;
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
                                       Subjects = (from sps in _db.Subjects
                                                   where sps.SubjectId == shortpaper.SubjectId
                                                   select new SubjectDTO
                                                   {
                                                       SubjectId = sps.SubjectId,
                                                       SubjectName = sps.SubjectName,
                                                   }).FirstOrDefault(),
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
                    Subjects = group.First().Subjects,
                    Student = group.First().Shortpaper.Student,
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
                                   where (shortpaper.ShortpaperTopic.Contains(searchText) || shortpaper.SubjectId.Contains(searchText) || s.StudentId.Contains(searchText) || s.Year.Contains(searchText))
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
                                       Subjects = (from sps in _db.Subjects
                                                   where sps.SubjectId == shortpaper.SubjectId
                                                   select new SubjectDTO
                                                   {
                                                       SubjectId = sps.SubjectId,
                                                       SubjectName = sps.SubjectName,
                                                   }).FirstOrDefault(),
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
                    Subjects = group.First().Subjects,
                    Student = group.First().Shortpaper.Student,
                    Committees = group.Select(x => x.Committee).ToList(),
                })
                .ToList();
                }
                var result = new ServiceResponse<List<ShortpaperDTO>>()
                {
                    httpStatusCode = StatusCodes.Status200OK,
                    Data = shortpapers
                };

                return result;
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
    }
}
