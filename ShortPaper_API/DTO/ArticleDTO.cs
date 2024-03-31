using System;
namespace ShortPaper_API.DTO
{
	public class ArticleDTO
	{
        public int ArticleId { get; set; }

        public string Topic { get; set; }

        public string Author { get; set; }

        public string? FileName { get; set; }

        public string? FileSize { get; set; }

        public string? FileType { get; set; }

        public string Year { get; set; }

        public SubjectDTO Subjects { get; set; }
    }
}

