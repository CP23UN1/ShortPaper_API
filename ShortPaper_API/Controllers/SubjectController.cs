using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShortPaper_API.Entities;
using ShortPaper_API.Repositories;
using ShortPaper_API.Services.Subjects;
using ShortPaper_API.Services.Users;

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
        public List<Subject> GetSubjects()
        {
            var subjects = _subjectService.GetSubjects();
            return subjects;
        }

        [HttpGet]
        [Route("subject/{id}")]
        public Subject GetSubjectById(int id)
        {
            var subject = _subjectService.GetSubject(id);
            return subject;
        }
    }
}
