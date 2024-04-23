using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MT.NoSql.API.DAL.Concrete;
using MT.NoSql.API.DAL.Interfaces;
using MT.NoSql.API.Entities;

namespace MT.NoSql.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TasksMongoController : ControllerBase
    {
        private readonly IRepositoryFactory _repositoryFactory;
        private readonly TaskMongoRepository _taskRepository;

        public TasksMongoController(IRepositoryFactory repositoryFactory)
        {
            _repositoryFactory = repositoryFactory;
            _taskRepository = (TaskMongoRepository)_repositoryFactory.CreateTaskRepository(DataStore.Mongo).Result;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTasks()
        {
            var tasks = await _taskRepository.GetAllTasks();
            return Ok(tasks);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var task = await _taskRepository.GetTaskById(id);
            return Ok(task);
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] MyTask task)
        {
            await _taskRepository.CreateTask(task);
            return NoContent();
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] MyTask task)
        {
            await _taskRepository.UpdateTask(id, task);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _taskRepository.DeleteTask(id);

            return Ok("Deleted");
        }
    }
}
