using CodePulse.API.Models.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CodePulse.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        // POST: {apibaseurl}/api/images -- to upload images
        [HttpPost]
        public async Task<IActionResult> UploadImage([FromForm] IFormFile file, [FromForm] string fileName, [FromForm] string title)
        {
            validateFileUpload(file);

            if (ModelState.IsValid)
            {
                // file upload
                var blogImage = new BlogImage
                {
                    fileExtension = Path.GetExtension(fileName).ToLower(),
                    fileName = fileName,
                    title = title,
                    dateCreated = DateTime.Now,
                };
            }

            return null;
        }

        private void validateFileUpload(IFormFile file)
        {
            var allowedExtensions = new string[] { ".jpg", ".jpeg", ".png" };

            if (!allowedExtensions.Contains(Path.GetExtension(file.FileName).ToLower()))
            {
                ModelState.AddModelError("file", "Unsupported file format");
            }

            // 10mb
            if (file.Length > 10485760)
            {
                ModelState.AddModelError("file", "File size cannot be more than 10MB");
            }
        }
    }
}
