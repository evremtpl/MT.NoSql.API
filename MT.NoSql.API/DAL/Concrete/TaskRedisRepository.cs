
using MT.NoSql.API.DAL.Interfaces;
using MT.NoSql.API.Entities;
using StackExchange.Redis;
using System.Text.Json;

namespace MT.NoSql.API.DAL.Concrete
{
    public class TaskRedisRepository : ITaskRepository
    {
        private readonly IConnectionMultiplexer _redis;

        public TaskRedisRepository(IConnectionMultiplexer redis)
        {
            _redis = redis;
        }
        public async Task CreateTask(MyTask task)
        {
            if (task == null)
            {
                throw new ArgumentOutOfRangeException(nameof(task));
            }
            var db = _redis.GetDatabase();

            var serialTask = JsonSerializer.Serialize(task);

            db.StringSet(task.Id.ToString(), serialTask);
            db.SetAdd("TaskSet", serialTask);
        }

        public async Task DeleteTask(int id)
        {
            var db = _redis.GetDatabase();
            var task = await db.StringGetAsync(id.ToString());
            var serialTask = JsonSerializer.Serialize(task);
            if (serialTask != null)
            {
                try
                {
                    var result = db.SetRemove("TaskSet", task);
                    if (result)
                    {
                        Console.WriteLine("Değer başarıyla silindi.");
                    }
                    db.KeyDelete(id.ToString());

                }
                catch (Exception ex)
                {

                    throw ex;
                }
            }


        }

        public async Task<IEnumerable<MyTask>> GetAllTasks()
        {
            var db = _redis.GetDatabase();

            var completeSet = db.SetMembers("TaskSet");

            if (completeSet.Length > 0)
            {
                var obj = Array.ConvertAll(completeSet, val =>
                    JsonSerializer.Deserialize<MyTask>(val)).ToList();
                Console.WriteLine("Tasks ");
                return obj;
            }

            return null;
        }

        public async  Task<MyTask?> GetTaskById(int id)
        {
            var db = _redis.GetDatabase();


            var task = db.StringGet(id.ToString());

            if (!string.IsNullOrEmpty(task))
            {
                return JsonSerializer.Deserialize<MyTask>(task);
            }
            return null;
        }

        public async Task UpdateTask(int id, MyTask task)
        {
            var db = _redis.GetDatabase();
            var dbTask = await db.StringGetAsync(task.Id.ToString());
            var serialTask = JsonSerializer.Serialize(task);
            var serialTaskdb = JsonSerializer.Serialize(dbTask);
            if (serialTaskdb != null)
            {
                try
                {
                    var result = db.SetRemove("TaskSet", dbTask);
                    if (result)
                    {
                        Console.WriteLine("Değer başarıyla silindi.");
                        db.StringSet(task.Id.ToString(), serialTask);
                        db.SetAdd("TaskSet", serialTask);
                        Console.WriteLine($"{task.TaskName} değeri başarıyla eklendi.");
                    }

                }
                catch (Exception ex)
                {

                    throw ex;
                }
            }
        }
    }
}
