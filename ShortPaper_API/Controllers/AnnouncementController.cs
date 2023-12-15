using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShortPaper_API.DTO;
using ShortPaper_API.Entities;
using ShortPaper_API.Services.Announcements;
using ShortPaper_API.Services.Subjects;
using ShortPaper_API.Services.Users;

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
        public List<AnnouncementDTO> GetAnnouncement()
        {
            var announcements = _announcementService.GetAnnouncements();
            return announcements;
        }

        [HttpPost]
        [Route("announcement/create")]
        public AnnouncementDTO CreateAnnouncement(AnnouncementDTO announcement)
        {
            var createAnnouncement = _announcementService.CreateAnnouncement(announcement);
            return createAnnouncement;
        }

        [HttpPut]
        [Route("announcement/update/{id}")]
        public AnnouncementDTO UpdateAnnouncement(AnnouncementDTO announcement)
        {
            var updateAnnouncement = _announcementService.UpdateAnnouncement(announcement);
            return updateAnnouncement;
        }

        [HttpDelete]
        [Route("announcement/delete/{id}")]
        public Announcement DeleteAnnouncement(int id)
        {
            var deleteAnnouncement = _announcementService.DeleteAnnouncement(id);
            return deleteAnnouncement;
        }
    }
}
