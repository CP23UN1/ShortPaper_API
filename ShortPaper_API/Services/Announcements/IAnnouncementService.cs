using ShortPaper_API.DTO;
using ShortPaper_API.Entities;

namespace ShortPaper_API.Services.Announcements
{
    public interface IAnnouncementService
    {
        List<AnnouncementDTO> GetAnnouncements();
        AnnouncementDTO CreateAnnouncement(AnnouncementDTO announcement);
        AnnouncementDTO UpdateAnnouncement(AnnouncementDTO announcement);
        Announcement DeleteAnnouncement(int id);
    }
}
