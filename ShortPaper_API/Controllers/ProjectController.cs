using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShortPaper_API.Entities;
using ShortPaper_API.Services.Projects;

namespace ShortPaper_API.Controllers
{
    [Route("/api")]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        private readonly IProjectService _projectService;

        public ProjectController(IProjectService projectService)
        {
            _projectService = projectService;
        }

        [HttpGet]
        [Route("/projects")]
        public List<Project> GetSubjects()
        {
            var projects = _projectService.GetProjects();
            return projects;
        }

        [HttpGet]
        [Route("/project/{id}")]
        public Project GetSubjectById(int id)
        {
            var project = _projectService.GetProject(id);
            return project;
        }

        [HttpPost]
        [Route("/addCommitteeMember")]
        public IActionResult AddCommitteeMember(int projectId, int advisorId, int advisorId1, int advisorId2)
        {
            try
            {
                _projectService.ChooseCommitteeMembers(projectId, advisorId, advisorId1, advisorId2);
                return Ok("Committee member added successfully");
            }
            catch (Exception ex)
            {
                // Handle exceptions and return an appropriate error response
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }
    }
}
