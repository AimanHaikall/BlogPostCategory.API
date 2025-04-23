namespace CodePulse.API.Models.Domain
{
    public class Category
    {
        public Guid id { get; set; }

        public string name { get; set; }

        public string urlHandle { get; set; }

        public ICollection<BlogPost> BlogPosts { get; set; }
    }
}
