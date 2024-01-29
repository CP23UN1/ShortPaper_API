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
                                   select new 
                                   {
                                       Shortpaper = new ShortpaperDTO {
                                            ShortpaperId = shortpaper.ShortpaperId,
                                            ShortpaperTopic = shortpaper.ShortpaperTopic,
                                        },
                                       Subjects = (from sps in _db.Subjects
                                                   where sps.SubjectId == shortpaper.SubjectId
                                                   select new SubjectDTO
                                                   {
                                                       SubjectId = sps.SubjectId,
                                                       SubjectName = sps.SubjectName,
                                                   }).ToList(),
                                       Student = (from s in _db.Students
                                                  where s.StudentId == shortpaper.StudentId
                                                  select new StudentForShortpaperDTO
                                                  {
                                                      StudentId = s.StudentId,
                                                      Firstname = s.Firstname, 
                                                      Lastname = s.Lastname,
                                                      Email = s.Email,
                                                  }).ToList(),
                                       Committees = (from sc in _db.Committees
                                                     where sc.CommitteeId == sphc.CommitteeId
                                                     select new CommitteeForShortpaperDTO
                                                     {
                                                         CommitteeId = sc.CommitteeId,
                                                         Firstname = sc.Firstname,
                                                         Lastname = sc.Lastname,
                                                         Email = sc.Email,
                                                     }).ToList(),
                                   }).GroupBy(x => x.Shortpaper.ShortpaperId) // Group by ShortpaperId
                .Select(group => new ShortpaperDTO
                {
                    // Select properties from the first item in the group (assuming Shortpaper properties are the same for each group)
                    ShortpaperId = group.First().Shortpaper.ShortpaperId,
                    ShortpaperTopic = group.First().Shortpaper.ShortpaperTopic,
                    Subjects = group.First().Subjects,
                    Student = group.First().Student,
                    Committees = group.First().Committees,
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
                                   select new
                                   {
                                       Shortpaper = new ShortpaperDTO
                                       {
                                           ShortpaperId = shortpaper.ShortpaperId,
                                           ShortpaperTopic = shortpaper.ShortpaperTopic,
                                       },
                                       Subjects = (from sps in _db.Subjects
                                                   where sps.SubjectId == shortpaper.SubjectId
                                                   select new SubjectDTO
                                                   {
                                                       SubjectId = sps.SubjectId,
                                                       SubjectName = sps.SubjectName,
                                                   }).ToList(),
                                       Student = (from s in _db.Students
                                                  where s.StudentId == shortpaper.StudentId
                                                  select new StudentForShortpaperDTO
                                                  {
                                                      StudentId = s.StudentId,
                                                      Firstname = s.Firstname,
                                                      Lastname = s.Lastname,
                                                      Email = s.Email,
                                                  }).ToList(),
                                       Committees = (from sc in _db.Committees
                                                     where sc.CommitteeId == sphc.CommitteeId
                                                     select new CommitteeForShortpaperDTO
                                                     {
                                                         CommitteeId = sc.CommitteeId,
                                                         Firstname = sc.Firstname,
                                                         Lastname = sc.Lastname,
                                                         Email = sc.Email,
                                                     }).ToList(),
                                   }).GroupBy(x => x.Shortpaper.ShortpaperId) // Group by ShortpaperId
                .Select(group => new ShortpaperDTO
                {
                    // Select properties from the first item in the group (assuming Shortpaper properties are the same for each group)
                    ShortpaperId = group.First().Shortpaper.ShortpaperId,
                    ShortpaperTopic = group.First().Shortpaper.ShortpaperTopic,
                    Subjects = group.First().Subjects,
                    Student = group.First().Student,
                    Committees = group.First().Committees,
                })
                .ToList();
                }
                else
                {
                    shortpapers = (from shortpaper in _db.Shortpapers
                                   join hasCommittee in _db.ShortpapersHasCommittees on shortpaper.ShortpaperId equals hasCommittee.ShortpaperId
                                   into shortperHasCommittee
                                   from sphc in shortperHasCommittee.DefaultIfEmpty()
                                   select new
                                   {
                                       Shortpaper = new ShortpaperDTO
                                       {
                                           ShortpaperId = shortpaper.ShortpaperId,
                                           ShortpaperTopic = shortpaper.ShortpaperTopic,
                                       },
                                       Subjects = (from sps in _db.Subjects
                                                   where sps.SubjectId == shortpaper.SubjectId
                                                   select new SubjectDTO
                                                   {
                                                       SubjectId = sps.SubjectId,
                                                       SubjectName = sps.SubjectName,
                                                   }).ToList(),
                                       Student = (from s in _db.Students
                                                  where s.StudentId == shortpaper.StudentId
                                                  select new StudentForShortpaperDTO
                                                  {
                                                      StudentId = s.StudentId,
                                                      Firstname = s.Firstname,
                                                      Lastname = s.Lastname,
                                                      Email = s.Email,
                                                  }).ToList(),
                                       Committees = (from sc in _db.Committees
                                                     where sc.CommitteeId == sphc.CommitteeId
                                                     select new CommitteeForShortpaperDTO
                                                     {
                                                         CommitteeId = sc.CommitteeId,
                                                         Firstname = sc.Firstname,
                                                         Lastname = sc.Lastname,
                                                         Email = sc.Email,
                                                     }).ToList(),
                                   }).GroupBy(x => x.Shortpaper.ShortpaperId) // Group by ShortpaperId
                .Select(group => new ShortpaperDTO
                {
                    // Select properties from the first item in the group (assuming Shortpaper properties are the same for each group)
                    ShortpaperId = group.First().Shortpaper.ShortpaperId,
                    ShortpaperTopic = group.First().Shortpaper.ShortpaperTopic,
                    Subjects = group.First().Subjects,
                    Student = group.First().Student,
                    Committees = group.First().Committees,
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
