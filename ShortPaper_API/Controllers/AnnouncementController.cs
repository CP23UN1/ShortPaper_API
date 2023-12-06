using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShortPaper_API.Entities;
using ShortPaper_API.Repositories;
using ShortPaper_API.Services.Announcements;
using ShortPaper_API.Services.Subjects;
using ShortPaper_API.Services.Users;

namespace ShortPaper_API.Controllers
{
    [Route("/api")]
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
        [Route("/subjects")]
        public List<Announcement> GetAnnouncement()
        {
            var announcements = _announcementService.GetAnnouncements();
            return announcements;
        }
    }
}
