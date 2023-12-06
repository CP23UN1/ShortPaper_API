
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

        public List<Announcement> GetAnnouncements()
        {
            var announcements = (from a in _db.Announcements
                            select a).ToList();

            return announcements;
        }

    }
}
