namespace ShortPaper_API.DTO
{
    public class AddCommitteeDTO
    {
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }
        public string AlternativeEmail { get; set; }
        public string Password { get; set; }
        public string Phonenumber { get; set; }
        public ulong IsAdvisor { get; set; }
    }
}
