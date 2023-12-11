using ShortPaper_API.DTO;
using ShortPaper_API.Entities;

namespace ShortPaper_API.Services.Projects
{
    public interface IProjectService
    {
        List<ProjectDTO> GetProjects();
        ProjectDTO GetProject(int id);
        void ChooseCommitteeMembers(int projectId, int advisorId, int advisorId1, int advisorId2);
    }
}
