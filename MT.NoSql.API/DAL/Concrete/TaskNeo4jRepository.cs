
using MT.NoSql.API.DAL.Interfaces;
using MT.NoSql.API.Entities;
using Neo4jClient;

namespace MT.NoSql.API.DAL.Concrete
{
    public class TaskNeo4jRepository : ITaskRepository
    {
        private readonly IGraphClient _graphClient;

        public TaskNeo4jRepository(IGraphClient graphClient)
        {
            _graphClient = graphClient;
        }
        public async Task CreateTask(MyTask task)
        {
            await _graphClient.Cypher.Create("(d:Tasks $tasks)")
                                    .WithParam("tasks", task)
                                    .ExecuteWithoutResultsAsync();
        }

        public async Task DeleteTask(int id)
        {
            try
            {



                var result = await _graphClient.Cypher
                            .WithParam("value", id)
                            .Match("(a:Category)-[r:hasTasks]->(b:Tasks)")
                            .Where("b.Id = $value")
                            .Return((a, b) => new { A = a.As<Category>(), B = b.As<MyTask>() })
                            .ResultsAsync;

                if (result.Any())
                {
                    await _graphClient.Cypher
                              .WithParam("value", id)
                              .Match("(a:Category)-[r:hasTasks]->(b:Tasks)")
                              .Where("b.Id = $value")
                              .Delete("r,b")
                              .ExecuteWithoutResultsAsync();
                    Console.WriteLine("Düğüm ve ilişki başarıyla silindi.");

                }
                else
                {

                    await _graphClient.Cypher.Match("(d: Tasks)")
                                                  .Where((MyTask d) => d.Id == id)
                                                  .Delete("d")
                                                  .ExecuteWithoutResultsAsync();

                }
 

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<IEnumerable<MyTask>> GetAllTasks()
        {
            var tasks = await _graphClient.Cypher.Match("(n: Tasks)")
                                                    .Return(n => n.As<MyTask>()).ResultsAsync;
            return tasks;
        }

        public async Task<MyTask?> GetTaskById(int id)
        {
            var tasks = await _graphClient.Cypher.Match("(n: Tasks)")
                                                    .Where((MyTask n) => n.Id == id)
                                                    .Return(n => n.As<MyTask>()).ResultsAsync;

            return tasks.FirstOrDefault();
        }

        public async Task UpdateTask(int id, MyTask task)
        {
            try
            {
                await _graphClient.Cypher.Match("(d: Tasks)")
                                                .Where((MyTask d) => d.Id == id)
                                                .Set("d = $tasks")
                                                .WithParam("tasks", task)
                                                .ExecuteWithoutResultsAsync();


            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
