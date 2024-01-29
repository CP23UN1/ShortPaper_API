using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShortPaper_API.DTO;
using ShortPaper_API.Entities;
using ShortPaper_API.Helper;
using ShortPaper_API.Services.Announcements;
using ShortPaper_API.Services.Subjects;

namespace ShortPaper_API.Controllers
{
    [Route("api")]
    [ApiController]
    public class SubjectController : ControllerBase
    {
        private readonly ShortpaperDbContext _dbContext;
        private readonly ISubjectService _subjectService;

        public SubjectController(ShortpaperDbContext dbContext, ISubjectService subjectService)

        {
            _dbContext = dbContext;
            _subjectService = subjectService;
        }

        [HttpGet]
        [Route("subjects")]
        public ServiceResponse<List<SubjectDTO>> GetSubjects()
        {
            var subjects = _subjectService.GetSubjects();
            return subjects;
        }

        [HttpGet]
        [Route("subjectsbyfilter")]
        public ServiceResponse<List<SubjectDTO>> GetSubjectByFilter(string searchText)
        {
            var subjects = _subjectService.GetSubjectByFilter(searchText);
            return subjects;
        }
    }
}
