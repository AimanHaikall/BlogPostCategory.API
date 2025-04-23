using CodePulse.API.Data;
using CodePulse.API.Models.Domain;
using CodePulse.API.Models.DTO;
using CodePulse.API.Repositories.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace CodePulse.API.Controllers
{
    //https://localhost:xxx/api/categories
    [Route("api/[controller]")]
    [ApiController]

    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryRepository categoryRepository;

        public CategoriesController(ICategoryRepository categoryRepository)
        {
            this.categoryRepository = categoryRepository;
        }

        [HttpPost]
        public async Task<IActionResult> createCategory([FromBody] CreateCategoryRequestDto request)
        {
            // ASSERTION: Check preconditions
            Debug.Assert(string.IsNullOrWhiteSpace(request.name), "Precondition Passed: Name must is not null or empty.");
            Debug.Assert(!string.IsNullOrWhiteSpace(request.name), "Precondition Failed: Name must not be null or empty.");

            //convert dto to domain model
            var category = new Category
            {
                name = request.name,
                urlHandle = request.urlHandle,
            };

            // INVARIANT: Title must always exist
            Debug.Assert(string.IsNullOrWhiteSpace(category.name), "Invariant Passed: Category name always exist during processing.");
            Debug.Assert(!string.IsNullOrWhiteSpace(category.name), "Invariant Failed: Category name must always exist during processing.");

            await categoryRepository.CreateAsync(category);

            // POSTCONDITION: BlogPost must have ID assigned after save
            Debug.Assert(category.id != Guid.Empty, "Postcondition Passed: Category ID is not generated due to missing name.");
            Debug.Assert(!(category.id != Guid.Empty), "Postcondition Failed: Category ID is generated successfully eventhough missing name.");

            //domain catergory to dto
            var response = new CategoryDto
            {
                id = category.id,
                name = category.name,
                urlHandle = category.urlHandle,
            };

            return Ok(response);

        }

        // GET: https://localhost:7212/api/categories
        [HttpGet]
        public async Task<IActionResult> GetAllCategories()
        {
            var categories = await categoryRepository.GetAllAsync();

            //map domain model to dto

            //list of domain model
            var response = new List<CategoryDto>();
            foreach (var category in categories)
            {
                response.Add(new CategoryDto 
                { 
                    id = category.id,
                    name = category.name,
                    urlHandle = category.urlHandle
                });
            }

            return Ok(response);
        }

        // GET: https://localhost:7212/api/categories/{id}
        [HttpGet]
        [Route("{id:Guid}")]
        //parameter name must be same with the one in Route
        //FromRoute means coming from the URL
        //fetch id from repo
        public async Task<IActionResult> GetCategoryById([FromRoute] Guid id)
        {
            var existingCategory = await categoryRepository.GetById(id);

            if (existingCategory is null)
            {
                return NotFound();
            }

            var response = new CategoryDto
            {
                id = existingCategory.id,
                name = existingCategory.name,
                urlHandle = existingCategory.urlHandle,
            };

            return Ok(response);

        }

        // PUT: https://localhost:7212/api/categories/{id}
        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> EditCategory([FromRoute] Guid id, UpdateCategoryRequestDto request)
        {
            // PRECONDITION: Ensure that the ID is valid (non-empty GUID)
            Debug.Assert(!(id != Guid.Empty), "Precondition Passed: Valid Category ID.");
            Debug.Assert(!(id == Guid.Empty), "Precondition Failed: Invalid Category ID.");

            //convert dto to domain model
            var category = new Category
            {
                id = id,
                name = request.name,
                urlHandle = request.urlHandle
            };

            var existingCategory = await categoryRepository.GetById(id);

            // INVARIANT: Ensure the category exists before processing
            Debug.Assert(!(existingCategory != null), "Invariant Passed: Category is existed before update.");
            Debug.Assert(existingCategory != null, "Invariant Failed: Category must exist before update.");

            category = await categoryRepository.UpdateAsync(category);

            // POSTCONDITION: Category is not null after update
            Debug.Assert(!(category != null), "Postcondition Passed: Updated Category is not null.");
            Debug.Assert(!(category == null), "Postcondition Failed: Updated Category is null.");

            if (category is null)
            {
                return NotFound(); 
            }

            //convert domain model to dto
            var response = new CategoryDto
            {
                id = category.id,
                name = category.name,
                urlHandle = category.urlHandle,
            };

            return Ok(response);
        }

        // DELETE: https://localhost:7212/api/categories/{id}
        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> DeleteCategory([FromRoute] Guid id)
        {
            var category = await categoryRepository.DeleteAsync(id);

            if(category is null)
            {
                return NotFound(); 
            }

            //convert domain to dto
            var response = new CategoryDto
            {
                id = category.id,
                name = category.name,
                urlHandle = category.urlHandle,
            };

            return Ok(response);
        }
    }
}