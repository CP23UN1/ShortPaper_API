using ShortPaper_API.Entities;

namespace ShortPaper_API.DTO
{
    public class ProjectDTO
    {
        public int ProjectId { get; set; }
        public string Topic { get; set; } = null!;
        public ProjectInfo? CommitteeFirst { get; set; }
        public ProjectInfo? CommitteeSecond { get; set; }
        public ProjectInfo? CommitteeThird { get; set; }
        public ProjectInfo Student { get; set; }
    }
    public class ProjectInfo
    {
        public int UserId { get; set; }
        public string? StudentId { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
    }
}
