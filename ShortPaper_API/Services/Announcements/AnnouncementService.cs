
using ShortPaper_API.DTO;
using ShortPaper_API.Entities;

namespace ShortPaper_API.Services.Announcements
{
    public class AnnouncementService : IAnnouncementService
    {
        private readonly ShortpaperDbContext _db;
        public AnnouncementService(ShortpaperDbContext db) 
        {
            _db = db;
        }

        public List<AnnouncementDTO> GetAnnouncements()
        {
            var announcements = (from a in _db.Announcements
                            select new AnnouncementDTO
                            {
                                Id = a.Id,
                                Title = a.Title,
                                Content = a.Content,
                                CreatedDatetime = a.CreatedDatetime,
                                Status = a.Status,
                                ImageUrl = a.ImageUrl,
                                AuthorId = a.AuthorId,
                                FileId = a.FileId,
                            }).ToList();

            return announcements;
        }

        public AnnouncementDTO CreateAnnouncement(AnnouncementDTO announcement)
        {
            var newAnnounce = new Announcement
            {
                Title = announcement.Title,
                Content = announcement.Content,
                Status = announcement.Status,
                ImageUrl = announcement.ImageUrl,
                AuthorId = announcement.AuthorId,
                FileId = announcement.FileId,
            };

            _db.Announcements.Add(newAnnounce);

            _db.SaveChanges();

            return announcement;
        }

        public AnnouncementDTO UpdateAnnouncement(AnnouncementDTO announcement)
        {
            var updateAnnounce = (from a in _db.Announcements
                                  where a.Id == announcement.Id
                                  select a).FirstOrDefault();
                                  
            updateAnnounce.Title = announcement.Title;
            updateAnnounce.Content = announcement.Content;
            updateAnnounce.Status = announcement.Status;
            updateAnnounce.ImageUrl = announcement.ImageUrl;
            updateAnnounce.FileId = announcement.FileId;

            _db.SaveChanges();

            return announcement;
        }

        public Announcement DeleteAnnouncement(int id)
        {
            var deleteAnnounce = (from a in _db.Announcements
                              where a.Id == id
                              select a).FirstOrDefault();

            _db.Announcements.Remove(deleteAnnounce);

            _db.SaveChanges();

            return deleteAnnounce;
        }

    }
}
