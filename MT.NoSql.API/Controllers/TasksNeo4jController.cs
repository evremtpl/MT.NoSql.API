using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MT.NoSql.API.DAL.Concrete;
using MT.NoSql.API.DAL.Interfaces;
using MT.NoSql.API.Entities;
using System.Threading.Tasks;

namespace MT.NoSql.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TasksNeo4jController : ControllerBase
    {
        private readonly TaskNeo4jRepository _taskRepository;
        private readonly IRepositoryFactory _repositoryFactory;

        public TasksNeo4jController(IRepositoryFactory repositoryFactory)
        {
            _repositoryFactory = repositoryFactory;
            _taskRepository = (TaskNeo4jRepository)_repositoryFactory.CreateTaskRepository(DataStore.Neo4jTask).Result;
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
            return task!=null ? Ok(task) : NotFound();
           
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
