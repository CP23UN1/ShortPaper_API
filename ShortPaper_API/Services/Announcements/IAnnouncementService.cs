using ShortPaper_API.Entities;

namespace ShortPaper_API.Services.Announcements
{
    public interface IAnnouncementService
    {
        List<Announcement> GetAnnouncements();
    }
}
