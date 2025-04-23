using Azure.Core;
using CodePulse.API.Models.Domain;
using CodePulse.API.Models.DTO;
using CodePulse.API.Repositories.Implementation;
using CodePulse.API.Repositories.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace CodePulse.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogPostsController : ControllerBase
    {
        private readonly IBlogPostRepository blogPostRepository;

        public BlogPostsController(IBlogPostRepository blogPostRepository)
        {
            this.blogPostRepository = blogPostRepository;
        }

        [HttpPost]
        public async Task<IActionResult> CreateBlogPost([FromBody] CreateBlogPostRequestDto request, ICategoryRepository categoryRepository)
        {
            //convert dto domain model
            var blogpost = new BlogPost
            {
                author = request.author,
                title = request.title,
                content = request.content,
                featuredImageUrl = request.featuredImageUrl,
                isVisible = request.isVisible,
                publishedDate = request.publishedDate,
                urlHandle = request.urlHandle,
                shortDesc = request.shortDesc,
                Categories = new List<Category>()
            };

            foreach (var categoryGuid in request.Categories)
            {
                var existingCategory = await categoryRepository.GetById(categoryGuid);

                if (existingCategory != null)
                {
                    blogpost.Categories.Add(existingCategory);
                }
            }

            blogpost = await blogPostRepository.CreateAsync(blogpost);

            //convert model back to dto
            var response = new BlogPostDto
            {
                id = blogpost.id,
                title = blogpost.title,
                author = blogpost.author,
                content = blogpost.content,
                featuredImageUrl = blogpost.featuredImageUrl,
                isVisible = blogpost.isVisible,
                publishedDate = blogpost.publishedDate,
                urlHandle = blogpost.urlHandle,
                shortDesc = blogpost.shortDesc,
                Categories = blogpost.Categories.Select(x => new CategoryDto
                {
                    id = x.id,
                    name = x.name,
                    urlHandle = x.urlHandle
                }).ToList()
            };

            return Ok(response);
        }

        // GET : {apiBaseUrl}/api/blogposts
        [HttpGet]
        public async Task<IActionResult> GetAllBlogPosts()
        {
            var blogPosts = await blogPostRepository.GetAllAsync();

            //convert domain to dto
            var response = new List<BlogPostDto>();

            foreach (var blogpost in blogPosts)
            {
                response.Add(new BlogPostDto
                {
                    id = blogpost.id,
                    title = blogpost.title,
                    author = blogpost.author,
                    content = blogpost.content,
                    featuredImageUrl = blogpost.featuredImageUrl,
                    isVisible = blogpost.isVisible,
                    publishedDate = blogpost.publishedDate,
                    urlHandle = blogpost.urlHandle,
                    shortDesc = blogpost.shortDesc,
                    Categories = blogpost.Categories.Select(x => new CategoryDto
                    {
                        id = x.id,
                        name = x.name,
                        urlHandle = x.urlHandle
                    }).ToList()
                });
            }

            return Ok(response);
        }

        // GET: {apiBaseUrl}/api/blogposts/{id}
        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetBlogPostById([FromRoute] Guid id)
        {
            //Get blogpost from repo
            var blogPost = await blogPostRepository.GetByIdAsync(id);

            if (blogPost is null)
            {
                return NotFound();
            }

            var response = new BlogPostDto
            {
                id = blogPost.id,
                title = blogPost.title,
                author = blogPost.author,
                content = blogPost.content,
                featuredImageUrl = blogPost.featuredImageUrl,
                isVisible = blogPost.isVisible,
                publishedDate = blogPost.publishedDate,
                urlHandle = blogPost.urlHandle,
                shortDesc = blogPost.shortDesc,
                Categories = blogPost.Categories.Select(x => new CategoryDto
                {
                    id = x.id,
                    name = x.name,
                    urlHandle = x.urlHandle
                }).ToList()
            };

            return Ok(response);
        }

        // PUT: {apibaseurl}/api/blogposts/{id}
        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> UpdateBlogPostById([FromRoute] Guid id, UpdateBlogPostRequestDto request, ICategoryRepository categoryRepository)
        {
            //convert dto to domain
            var blogPost = new BlogPost
            {
                id = id,
                title = request.title,
                author = request.author,
                content = request.content,
                featuredImageUrl = request.featuredImageUrl,
                isVisible = request.isVisible,
                publishedDate = request.publishedDate,
                urlHandle = request.urlHandle,
                shortDesc = request.shortDesc,
                Categories = new List<Category>()
            };

            // to loop each dto
            foreach (var categoryGuid in request.Categories) 
            {
                var existingCategory = await categoryRepository.GetById(categoryGuid);

                if (existingCategory != null)
                {
                    blogPost.Categories.Add(existingCategory);
                }
            }

            //call repo to update blogpost domain model
            var updatedBlogPost = await blogPostRepository.UpdateAsync(blogPost);

            if(updatedBlogPost == null)
            {
                return NotFound();
            }

            var response = new BlogPostDto
            {
                id = id,
                title = request.title,
                author = request.author,
                content = request.content,
                featuredImageUrl = request.featuredImageUrl,
                isVisible = request.isVisible,
                publishedDate = request.publishedDate,
                urlHandle = request.urlHandle,
                shortDesc = request.shortDesc,
                Categories = blogPost.Categories.Select(x => new CategoryDto
                {
                    id = x.id,
                    name = x.name,
                    urlHandle = x.urlHandle,
                }).ToList()
            };

            return Ok(response);

        }

        // DELETE: {apibaseurl}/api/blogposts/{id}
        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> DeleteBlogPost([FromRoute] Guid id)
        {
            // PRECONDITION: Ensure that the ID is valid (non-empty GUID)
            Debug.Assert(!(id != Guid.Empty), "Precondition Passed: Valid BlogPost ID.");
            Debug.Assert(!(id == Guid.Empty), "Precondition Failed: Invalid BlogPost ID.");

            // Try to delete the blog post from the repository
            var deletedBlogPost = await blogPostRepository.DeleteAsync(id);

            // INVARIANT: The post must either be deleted or found in the repository
            Debug.Assert(deletedBlogPost == null, "Invariant Passed: BlogPost exists before deletion.");
            Debug.Assert(deletedBlogPost != null, "Invariant Failed: BlogPost should exist before deletion.");

            // If no post was found and deleted, return a "Not Found" response
            if (deletedBlogPost == null)
            {
                return NotFound(new { error = "BlogPost not found." });
            }

            // POSTCONDITION: BlogPost must be successfully deleted, so its ID should be empty now
            Debug.Assert(deletedBlogPost.id != id, "Postcondition Passed: The deleted blog post ID matches the requested ID.");
            Debug.Assert(deletedBlogPost.id == id, "Postcondition Failed: The deleted blog post ID must match the requested ID.");

            var response = new BlogPostDto
            {
                id = deletedBlogPost.id,
                title = deletedBlogPost.title,
                author = deletedBlogPost.author,
                content = deletedBlogPost.content,
                featuredImageUrl = deletedBlogPost.featuredImageUrl,
                isVisible = deletedBlogPost.isVisible,
                publishedDate = deletedBlogPost.publishedDate,
                urlHandle = deletedBlogPost.urlHandle,
                shortDesc = deletedBlogPost.shortDesc,
            };

            return Ok(response);
        }

    }
}
