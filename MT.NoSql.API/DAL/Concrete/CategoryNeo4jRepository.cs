using Couchbase.Configuration.Server.Serialization;
using Couchbase.Utils;
using MT.NoSql.API.DAL.Interfaces;
using MT.NoSql.API.Entities;
using Neo4jClient;

namespace MT.NoSql.API.DAL.Concrete
{
    public class CategoryNeo4jRepository : ICategoryRepository
    {
        private readonly IGraphClient _graphClient;

        public CategoryNeo4jRepository(IGraphClient graphClient)
        {
            _graphClient = graphClient;
        }

        public async Task AssignTask(int categoryId, int taskId)
        {
            try
            {
                await _graphClient.Cypher.Match("(c:Category), (t:Tasks)")
                            .Where((Category c, MyTask t) => c.Id == categoryId && t.Id == taskId)
                            .Create("(c)-[r:hasTasks]->(t)")
                            .ExecuteWithoutResultsAsync();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        

        }

        public async Task CreateCategory(Category category)
        {
            try
            {
                await _graphClient.Cypher.Create("(e:Category $cat)")
                                               .WithParam("cat", category)
                                               .ExecuteWithoutResultsAsync();
            }
            catch (Exception ex)
            {


            }
        }

        public async Task DeleteCategory(int id)
        {
            try
            {
               
                string cypherQuery = @$"
                             MATCH (a:Category)-[r:hasTasks]->(b:Tasks)
                             WHERE a.Id = {id}
                             RETURN a, r, b";



                var result = await _graphClient.Cypher
                            .WithParam("value", id)
                            .Match("(a:Category)-[r:hasTasks]->(b:Tasks)")
                            .Where("a.Id = $value")
                            .Return((a, b) => new { A = a.As<Category>(), B = b.As<MyTask>() })
                            .ResultsAsync;
               
                if (result.Any())
                {
                    await _graphClient.Cypher
                              .WithParam("value", id)
                              .Match("(a:Category)-[r:hasTasks]->(b:Tasks)")
                              .Where("a.Id = $value")
                              .Delete("r,b")
                              .ExecuteWithoutResultsAsync();
                    Console.WriteLine("Düğüm ve ilişki başarıyla silindi.");

                }
                else
                {

                    await _graphClient.Cypher.Match("(d: Category)")
                                                    .Where((Category d) => d.Id == id)
                                                    .Delete("d")
                                                    .ExecuteWithoutResultsAsync();

                }





            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<IEnumerable<Category>> GetAllCategories()
        {
            var categories = await _graphClient.Cypher.Match("(n: Category)")
                                                  .Return(n => n.As<Category>()).ResultsAsync;
            return categories;
        }

        public async Task<Category?> GetCategoryById(int id)
        {
            var categories = await _graphClient.Cypher.Match("(n: Category)")
                                               .Where((Category n) => n.Id == id)
                                               .Return(n => n.As<Category>()).ResultsAsync;

            return categories.FirstOrDefault();
        }

        public async Task UpdateCategory(int id, Category category)
        {
            try
            {
                await _graphClient.Cypher.Match("(d: Category)")
                                                .WithParam("category", category)
                                                .Where((Category d) => d.Id == id)
                                                .Set("d = $category")
                                                .ExecuteWithoutResultsAsync();


            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

       
    }
}
