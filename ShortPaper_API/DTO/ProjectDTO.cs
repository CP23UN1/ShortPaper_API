using ShortPaper_API.Entities;

namespace ShortPaper_API.DTO
{
    public class ProjectDTO
    {
        public int ProjectId { get; set; }
        public string Topic { get; set; } = null!;
        public ProjectInfoDTO? CommitteeFirst { get; set; }
        public ProjectInfoDTO? CommitteeSecond { get; set; }
        public ProjectInfoDTO? CommitteeThird { get; set; }
        public ProjectInfoDTO Student { get; set; }
    }
    public class ProjectInfoDTO
    {
        public int UserId { get; set; }
        public string? StudentId { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
    }
}
