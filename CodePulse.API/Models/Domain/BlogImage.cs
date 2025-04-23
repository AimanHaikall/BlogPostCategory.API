namespace CodePulse.API.Models.Domain
{
    public class BlogImage
    {
        public Guid id { get; set; }

        public string fileName { get; set; }

        public string fileExtension { get; set; }

        public string title { get; set; }

        public string url { get; set; }

        public DateTime dateCreated { get; set; }
    }
}
