using ShortPaper_API.DTO;
using ShortPaper_API.Entities;
using ShortPaper_API.Helper;

namespace ShortPaper_API.Services.Announcements
{
    public interface IAnnouncementService
    {
        ServiceResponse<List<AnnouncementDTO>> GetAnnouncements();
        ServiceResponse<AnnouncementDTO> CreateAnnouncement(AnnouncementDTO newAnnouncement);
        ServiceResponse<AnnouncementDTO> UpdateAnnouncement(AnnouncementDTO updatedAnnouncement, int id);
        ServiceResponse<Announcement> DeleteAnnouncement(int id);
    }
}
