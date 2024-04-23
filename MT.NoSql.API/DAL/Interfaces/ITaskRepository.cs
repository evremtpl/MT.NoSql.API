using MT.NoSql.API.Entities;

namespace MT.NoSql.API.DAL.Interfaces
{
    public interface ITaskRepository 
    {
        Task CreateTask(MyTask task);
        Task<MyTask?> GetTaskById(int id);
        Task<IEnumerable<MyTask>> GetAllTasks();

        Task DeleteTask(int id);

        Task UpdateTask(int id, MyTask task);
    }
}
