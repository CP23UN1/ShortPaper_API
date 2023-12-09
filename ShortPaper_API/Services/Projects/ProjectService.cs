using Microsoft.EntityFrameworkCore;
using ShortPaper_API.Entities;

namespace ShortPaper_API.Services.Projects
{
    public class ProjectService : IProjectService
    {
        private readonly ShortpaperDbContext _db;
        public ProjectService(ShortpaperDbContext db)
        {
            _db = db;
        }

        public List<Project> GetProjects()
        {
            var projects = (from a in _db.Projects
                            select a).ToList();

            return projects;
        }

        public Project GetProject(int id)
        {
            var project = (from a in _db.Projects
                           where a.ProjectId == id
                           select a).FirstOrDefault();

            return project;
        }

        public void ChooseCommitteeMembers(int projectId, int advisorId, int advisorId2, int advisorId3)
        {
            // Fetch the project from the database
            var project = _db.Projects.SingleOrDefault(p => p.ProjectId == projectId);

            if (project == null)
            {
                // Handle the case where the project is not found
                // You might want to throw an exception or return an error response
                return;
            }

                // Assign the advisor to the first available committee slot
                    project.CommitteeFirst = advisorId;
                    project.CommitteeSecond = advisorId2;
                    project.CommitteeThird = advisorId3;

                // Save changes to the database
                _db.SaveChanges();
        }
    }
}
