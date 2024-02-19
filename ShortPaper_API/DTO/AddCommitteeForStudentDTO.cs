namespace ShortPaper_API.DTO
{
    public class AddCommitteeForStudentDTO
    {
        public string StudentId { get; set; }
        public string CommitteeName { get; set; }
        public ulong IsAdvisor { get; set; }
        public ulong IsPrincipal { get; set; }
        public ulong IsCommittee { get; set; }
    }
}
