using ShortPaper_API.DTO;
using ShortPaper_API.Entities;
using ShortPaper_API.Helper;
using System.Globalization;

namespace ShortPaper_API.Services.Announcements
{
    public class AnnouncementService : IAnnouncementService
    {
        private readonly ShortpaperDbContext _db;

        public AnnouncementService(ShortpaperDbContext db)
        {
            _db = db;
        }

        public ServiceResponse<List<AnnouncementDTO>> GetAnnouncements()
        {
            try
            {
                var announcements = (from a in _db.Announcements
                                     select new AnnouncementDTO
                                     {
                                         AnnouncementId = a.AnnouncementId,
                                         Topic = a.Topic,
                                         Content = a.Content,
                                         CreatedDatetime = a.CreatedDatetime,
                                         ExpiredDatetime = a.ExpiredDatetime,
                                         ImageUrl = a.ImageUrl
                                     }).ToList();

                // Convert date and time to Thai format
                var thaiCulture = new CultureInfo("th-TH");

                var result = new ServiceResponse<List<AnnouncementDTO>>()
                {
                    httpStatusCode = StatusCodes.Status200OK,
                    Data = announcements
                };

                return result;

            }
            catch (Exception ex)
            {

                var result = new ServiceResponse<List<AnnouncementDTO>>()
                {
                    httpStatusCode = StatusCodes.Status400BadRequest,
                    ErrorMessage = ex.Message
                };

                return result;
            }


        }

        public ServiceResponse<AnnouncementDTO> CreateAnnouncement(AnnouncementDTO newAnnouncement)
        {
            try
            {
                var newAnnounce = new Announcement
                {
                    Topic = newAnnouncement.Topic,
                    Content = newAnnouncement.Content,
                    CreatedDatetime = DateTime.Now,
                    ExpiredDatetime = newAnnouncement.ExpiredDatetime,
                    ImageUrl = newAnnouncement.ImageUrl
                };

                _db.Announcements.Add(newAnnounce);

                _db.SaveChanges();

                var result = new ServiceResponse<AnnouncementDTO>()
                {
                    httpStatusCode = StatusCodes.Status201Created,
                    Data = newAnnouncement,
                };

                return result;
            }
            catch (Exception ex)
            {
                var result = new ServiceResponse<AnnouncementDTO>()
                {
                    httpStatusCode = StatusCodes.Status400BadRequest,
                    ErrorMessage = ex.Message
                };

                return result;
            }
        }

        public ServiceResponse<AnnouncementDTO> UpdateAnnouncement(AnnouncementDTO updatedAnnouncement, int id)
        {
            try
            {
                var oldAnnounce = (from a in _db.Announcements
                                   where a.AnnouncementId == id
                                   select a).FirstOrDefault();

                oldAnnounce.Topic = updatedAnnouncement.Topic;
                oldAnnounce.Content = updatedAnnouncement.Content;
                oldAnnounce.ExpiredDatetime = updatedAnnouncement.ExpiredDatetime;
                oldAnnounce.ImageUrl = updatedAnnouncement.ImageUrl;

                _db.SaveChanges();

                var result = new ServiceResponse<AnnouncementDTO>()
                {
                    httpStatusCode = StatusCodes.Status200OK,
                    Data = updatedAnnouncement
                };

                return result;
            }
            catch (Exception ex)
            {
                var result = new ServiceResponse<AnnouncementDTO>()
                {
                    httpStatusCode = StatusCodes.Status400BadRequest,
                    ErrorMessage = ex.Message
                };

                return result;
            }
        }

        public ServiceResponse<Announcement> DeleteAnnouncement(int deletedId)
        {
            try
            {
                var deleteAnnounce = (from a in _db.Announcements
                                      where a.AnnouncementId == deletedId
                                      select a).FirstOrDefault();

                _db.Announcements.Remove(deleteAnnounce);

                _db.SaveChanges();

                var result = new ServiceResponse<Announcement>()
                {
                    httpStatusCode = StatusCodes.Status200OK,
                    Data = deleteAnnounce
                };

                return result;
            }
            catch (Exception ex)
            {
                var result = new ServiceResponse<Announcement>()
                {
                    httpStatusCode = StatusCodes.Status400BadRequest,
                    ErrorMessage = ex.Message
                };

                return result;
            }
        }
    }
}
