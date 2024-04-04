namespace ShortPaper_API.DTO
{
    public class StudentListFileStatusDTO
    {
        public string StudentId { get; set; }
        public List<FileStatusByTypeDTO> FileStatusByType { get; set; }
    }
}
