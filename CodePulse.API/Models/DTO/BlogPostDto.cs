﻿namespace CodePulse.API.Models.DTO
{
    public class BlogPostDto
    {
        public Guid id { get; set; }

        public string title { get; set; }

        public string shortDesc { get; set; }

        public string content { get; set; }

        public string featuredImageUrl { get; set; }

        public string urlHandle { get; set; }

        public DateTime publishedDate { get; set; }

        public string author { get; set; }

        public bool isVisible { get; set; }

        public List<CategoryDto> Categories { get; set; } = new List<CategoryDto>();
    }
}
