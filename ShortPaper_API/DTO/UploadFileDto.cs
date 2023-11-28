namespace ShortPaper_API.DTO
{
    public class UploadFileDto
    {
        public int Id { get; set; }

        public string Filename { get; set; } = null!;
        public string Filesize { get; set; } = null!;
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string? project { get; set; }
        public string? ExplanationVideo { get; set; }
        public byte[]? Data { get; set; }
        public string? Filetype { get; set; }
        public DateTime CreatedDatetime { get; set; }
        public DateTime UpdatedDatetime { get; set; }
        public int StatusId { get; set; }
        public int ProjectId { get; set; }
    }
}
