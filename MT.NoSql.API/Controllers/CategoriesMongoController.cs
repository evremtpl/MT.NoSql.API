using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MT.NoSql.API.DAL.Concrete;
using MT.NoSql.API.DAL.Interfaces;
using MT.NoSql.API.Entities;

namespace MT.NoSql.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesMongoController : ControllerBase
    {
        private readonly CategoryMongoRepository _categoryRepository;
        private readonly IRepositoryFactory _repositoryFactory;

        public CategoriesMongoController(IRepositoryFactory repositoryFactory)
        {
            _repositoryFactory = repositoryFactory;

            _categoryRepository = (CategoryMongoRepository)_repositoryFactory.CreateCategoryRepository(DataStore.Mongo).Result;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCategories()
        {
            var categories = await _categoryRepository.GetAllCategories();
            return Ok(categories);
        }

        [HttpGet("{categoryId}/{taskId}/")]
        public async Task<IActionResult> GetAssignTask(int categoryId, int taskId)
        {
            //  await _categoryRepository.AssignTask(categoryId, taskId);
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> CreateCategory([FromBody] Category category)
        {
            await _categoryRepository.CreateCategory(category);

            return NoContent();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var category = await _categoryRepository.GetCategoryById(id);
            return Ok(category);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Category category)
        {
            await _categoryRepository.UpdateCategory(id, category);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _categoryRepository.DeleteCategory(id);

            return Ok("Deleted");
        }
    }
}
