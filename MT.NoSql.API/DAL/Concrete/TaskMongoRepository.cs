using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MT.NoSql.API.DAL.Interfaces;
using MT.NoSql.API.Entities;
using MT.NoSql.API.Settings;

namespace MT.NoSql.API.DAL.Concrete
{
    public class TaskMongoRepository :ITaskRepository
    {
        private readonly IMongoCollection<MyTask> _taskCollection;
        public TaskMongoRepository(IOptions<MongoDbSettings> mongoDbSettings)
        {
            var mongoClient = new MongoClient(mongoDbSettings.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(mongoDbSettings.Value.DatabaseName);
            _taskCollection = mongoDatabase.GetCollection<MyTask>(mongoDbSettings.Value.CollectionNameTask);
        }

        public async Task CreateTask(MyTask task)
        {
            await _taskCollection.InsertOneAsync(task);
        }

        public async Task DeleteTask(int id)
        {
            await _taskCollection.DeleteOneAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<MyTask>> GetAllTasks()
        {
            return await _taskCollection.Find(_ => true).ToListAsync();
        }

        public async  Task<MyTask?> GetTaskById(int id)
        {
            return await _taskCollection.Find(_ => _.Id == id).FirstOrDefaultAsync();
        }

        public async Task UpdateTask(int id, MyTask task)
        {
            await _taskCollection.ReplaceOneAsync(x => x.Id == id, task);
        }
    }
}
