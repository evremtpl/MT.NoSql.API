using Couchbase;
using Couchbase.Core;
using Couchbase.Extensions.DependencyInjection;
using MT.NoSql.API.DAL.Interfaces;
using MT.NoSql.API.Entities;
using Newtonsoft.Json;

namespace MT.NoSql.API.DAL.Concrete
{
    public class TaskCouchBaseRepository : ITaskRepository
    {
        private readonly IBucket _bucket;


        public TaskCouchBaseRepository(IBucketProvider bucketProvider)
        {


            _bucket = bucketProvider.GetBucket("tasks");

        }
        public async Task CreateTask(MyTask task)
        {
            var document = new Document<MyTask>
            {
                Id = Guid.NewGuid().ToString(),
                Content = task
            };


            { await _bucket.InsertAsync(document); }
        }

        public async Task DeleteTask(int id)
        {
            try
            {
                var query = $"DELETE FROM {_bucket.Name} AS a  WHERE a.id = {id} RETURNING a";
                var result = _bucket.Query<MyTask>(query);
                if (result.Success)
                {
                    string documentContent = JsonConvert.SerializeObject(result.Rows.FirstOrDefault());


                    MyTask a = JsonConvert.DeserializeObject<MyTask>(documentContent);

                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<IEnumerable<MyTask>> GetAllTasks()
        {
            try
            {
                var tasks = new List<MyTask>();
                var query = $"SELECT id,taskName,completed,categoryId FROM {_bucket.Name}";
                var result = _bucket.Query<MyTask>(query);

                foreach (var row in result.Rows)
                {
                    string documentContent = JsonConvert.SerializeObject(row);


                    MyTask document = JsonConvert.DeserializeObject<MyTask>(documentContent);

                    tasks.Add(document);
                    Console.WriteLine($"Belge bulundu: {document}");
                }



                return tasks;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<MyTask?> GetTaskById(int id)
        {
            var query = $"SELECT id,taskName,completed,categoryId FROM {_bucket.Name} WHERE id = {id} LIMIT 1";
            var result = _bucket.Query<MyTask>(query);

       

            if (result.Success)
            {
                string documentContent = JsonConvert.SerializeObject(result.Rows.FirstOrDefault());


                MyTask document = JsonConvert.DeserializeObject<MyTask>(documentContent);
                return document;
            }
            else
            {
                return null;
            }
        }

        public async Task UpdateTask(int id, MyTask task)
        {
            try
            {
                var query = @$"UPDATE {_bucket.Name} SET taskName = ""{task.TaskName}"", completed =""{task.Completed}""  WHERE id = {id} RETURNING a";
                var result = _bucket.Query<MyTask>(query);
                if (result.Success)
                {
                    string documentContent = JsonConvert.SerializeObject(result.Rows.FirstOrDefault());


                    MyTask a = JsonConvert.DeserializeObject<MyTask>(documentContent);

                }

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
