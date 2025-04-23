using CodePulse.API.Data;
using CodePulse.API.Models.Domain;
using CodePulse.API.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace CodePulse.API.Repositories.Implementation
{
    public class BlogPostRepository : IBlogPostRepository
    {
        private readonly ApplicationDbContext dbContext;

        public BlogPostRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<BlogPost> CreateAsync(BlogPost blogPost)
        {
            await dbContext.blogPosts.AddAsync(blogPost);
            await dbContext.SaveChangesAsync();

            return blogPost;
        }

        public async Task<IEnumerable<BlogPost>> GetAllAsync()
        {
            return await dbContext.blogPosts.Include(x => x.Categories).ToListAsync();
        }

        public async Task<BlogPost?> GetByIdAsync(Guid id)
        {
            return await dbContext.blogPosts.Include(x =>x.Categories).FirstOrDefaultAsync(x => x.id == id);
        }

        public async Task<BlogPost?> UpdateAsync(BlogPost blogPost) 
        {
            var existingBlogPosts = await dbContext.blogPosts.Include(x => x.Categories).FirstOrDefaultAsync(x => x.id == blogPost.id);

            if(existingBlogPosts == null)
            {
                return null;
            }

            //update blogpost
            dbContext.Entry(existingBlogPosts).CurrentValues.SetValues(blogPost);

            //update categories
            existingBlogPosts.Categories = blogPost.Categories;

            await dbContext.SaveChangesAsync();

            return blogPost;
        }

        public async Task<BlogPost?> DeleteAsync(Guid id)
        {
            var existingBlogPost = await dbContext.blogPosts.FirstOrDefaultAsync(x => x.id == id);

            if(existingBlogPost != null)
            {
                dbContext.blogPosts.Remove(existingBlogPost);

                await dbContext.SaveChangesAsync();

                return existingBlogPost;
            }

            return null;
        }
    }
}
