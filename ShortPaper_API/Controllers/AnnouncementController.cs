using Microsoft.AspNetCore.Mvc;
using ShortPaper_API.DTO;
using ShortPaper_API.Entities;
using ShortPaper_API.Helper;
using ShortPaper_API.Services.Announcements;

namespace ShortPaper_API.Controllers
{
    [Route("api")]
    [ApiController]
    public class AnnouncementController : ControllerBase
    {
        private readonly ShortpaperDbContext _dbContext;
        private readonly IAnnouncementService _announcementService;

        public AnnouncementController(ShortpaperDbContext dbContext, IAnnouncementService announcementService)

        {
            _dbContext = dbContext;
            _announcementService = announcementService;
        }

        [HttpGet]
        [Route("announcements")]
        public ServiceResponse<List<AnnouncementDTO>> GetAnnouncement()
        {
            var announcements = _announcementService.GetAnnouncements();
            return announcements;
        }

        [HttpPost]
        [Route("announcement/create")]
        public ServiceResponse<AnnouncementDTO> CreateAnnouncement(AnnouncementDTO announcement)
        {
            var createAnnouncement = _announcementService.CreateAnnouncement(announcement);
            return createAnnouncement;
        }

        [HttpPut]
        [Route("announcement/update/{id}")]
        public ServiceResponse<AnnouncementDTO> UpdateAnnouncement(AnnouncementDTO announcement, int id)
        {
            var updateAnnouncement = _announcementService.UpdateAnnouncement(announcement, id);
            return updateAnnouncement;
        }

        [HttpDelete]
        [Route("announcement/delete/{id}")]
        public ServiceResponse<Announcement> DeleteAnnouncement(int id)
        {
            var deleteAnnouncement = _announcementService.DeleteAnnouncement(id);
            return deleteAnnouncement;
        }
    }
}
