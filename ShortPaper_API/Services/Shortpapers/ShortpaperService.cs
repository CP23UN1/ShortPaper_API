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
                                   join student in _db.Students on shortpaper.StudentId equals student.StudentId
                                   into studentShortpaper
                                   from studentShort in studentShortpaper.DefaultIfEmpty()
                                   join subject in _db.Subjects on shortpaper.SubjectId equals subject.SubjectId
                                   into shortpaperSubject
                                   from sps in shortpaperSubject.DefaultIfEmpty()
                                   join hasCommittee in _db.ShortpapersHasCommittees on shortpaper.ShortpaperId equals hasCommittee.ShortpaperId
                                   into shortperHasCommittee
                                   from sphc in shortperHasCommittee.DefaultIfEmpty()
                                   join committee in _db.Committees  on sphc.CommitteeId equals committee.CommitteeId
                                   into shortpaperCommittee
                                   from spct in shortpaperCommittee.DefaultIfEmpty()
                                   select new ShortpaperDTO
                                   {
                                    ShortpaperId = shortpaper.ShortpaperId,
                                    ShortpaperTopic = shortpaper.ShortpaperTopic,
                                    Subject = sps != null ? new SubjectDTO
                                    {
                                        SubjectId = sps.SubjectId,
                                        SubjectName = sps.SubjectName,
                                    } : null,
                                       StudentForShortpaper = studentShort != null ? new StudentForShortpaperDTO
                                    {
                                        StudentId = studentShort.StudentId,
                                        Firstname = studentShort.Firstname,
                                        Lastname = studentShort.Lastname,
                                        Email = studentShort.Email,
                                    } : null,
                                    CommitteeForShortpaper = spct != null
                                    ? new CommitteeForShortpaperDTO
                                    {
                                        CommitteeId = spct.CommitteeId,
                                        Firstname = spct.Firstname,
                                        Lastname = spct.Lastname,
                                        Email = spct.Email,
                                    } : null,
                                }).ToList();

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
                                   join student in _db.Students on shortpaper.StudentId equals student.StudentId
                                   into studentShortpaper
                                   from studentShort in studentShortpaper.DefaultIfEmpty()
                                   join subject in _db.Subjects on shortpaper.SubjectId equals subject.SubjectId
                                   into shortpaperSubject
                                   from sps in shortpaperSubject.DefaultIfEmpty()
                                   join hasCommittee in _db.ShortpapersHasCommittees on shortpaper.ShortpaperId equals hasCommittee.ShortpaperId
                                   into shortperHasCommittee
                                   from sphc in shortperHasCommittee.DefaultIfEmpty()
                                   join committee in _db.Committees on sphc.CommitteeId equals committee.CommitteeId
                                   into shortpaperCommittee
                                   from spct in shortpaperCommittee.DefaultIfEmpty()
                                   select new ShortpaperDTO
                                   {
                                       ShortpaperId = shortpaper.ShortpaperId,
                                       ShortpaperTopic = shortpaper.ShortpaperTopic,
                                       Subject = sps != null ? new SubjectDTO
                                       {
                                           SubjectId = sps.SubjectId,
                                           SubjectName = sps.SubjectName,
                                       } : null,
                                       StudentForShortpaper = studentShort != null ? new StudentForShortpaperDTO
                                       {
                                           StudentId = studentShort.StudentId,
                                           Firstname = studentShort.Firstname,
                                           Lastname = studentShort.Lastname,
                                           Email = studentShort.Email,
                                       } : null,
                                       CommitteeForShortpaper = spct != null
                                    ? new CommitteeForShortpaperDTO
                                    {
                                        CommitteeId = spct.CommitteeId,
                                        Firstname = spct.Firstname,
                                        Lastname = spct.Lastname,
                                        Email = spct.Email,
                                    } : null,
                                   }).ToList();
                }
                else
                {
                    shortpapers = (from shortpaper in _db.Shortpapers
                                   join student in _db.Students on shortpaper.StudentId equals student.StudentId
                                   into studentShortpaper
                                   from studentShort in studentShortpaper.DefaultIfEmpty()
                                   join subject in _db.Subjects on shortpaper.SubjectId equals subject.SubjectId
                                   into shortpaperSubject
                                   from sps in shortpaperSubject.DefaultIfEmpty()
                                   join hasCommittee in _db.ShortpapersHasCommittees on shortpaper.ShortpaperId equals hasCommittee.ShortpaperId
                                   into shortperHasCommittee
                                   from sphc in shortperHasCommittee.DefaultIfEmpty()
                                   join committee in _db.Committees on sphc.CommitteeId equals committee.CommitteeId
                                   into shortpaperCommittee
                                   from spct in shortpaperCommittee.DefaultIfEmpty()
                                   where (string.IsNullOrEmpty(searchText) || 
                                   studentShort.StudentId.Contains(searchText) ||
                                   studentShort.Year.Contains(searchText) ||
                                   sps.SubjectId.Contains(searchText))
                                   select new ShortpaperDTO
                                   {
                                       ShortpaperId = shortpaper.ShortpaperId,
                                       ShortpaperTopic = shortpaper.ShortpaperTopic,
                                       Subject = sps != null ? new SubjectDTO
                                       {
                                           SubjectId = sps.SubjectId,
                                           SubjectName = sps.SubjectName,
                                       } : null,
                                       StudentForShortpaper = studentShort != null ? new StudentForShortpaperDTO
                                       {
                                           StudentId = studentShort.StudentId,
                                           Firstname = studentShort.Firstname,
                                           Lastname = studentShort.Lastname,
                                           Email = studentShort.Email,
                                       } : null,
                                       CommitteeForShortpaper = spct != null
                                    ? new CommitteeForShortpaperDTO
                                    {
                                        CommitteeId = spct.CommitteeId,
                                        Firstname = spct.Firstname,
                                        Lastname = spct.Lastname,
                                        Email = spct.Email,
                                    } : null,
                                   }).ToList();
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
