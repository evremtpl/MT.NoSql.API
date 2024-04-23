using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MT.NoSql.API.DAL.Interfaces;
using MT.NoSql.API.Entities;
using MT.NoSql.API.Settings;

namespace MT.NoSql.API.DAL.Concrete
{
    public class CategoryMongoRepository : ICategoryRepository
    {

        private readonly IMongoCollection<Category> _categoryCollection;
        public CategoryMongoRepository(IOptions<MongoDbSettings> mongoDbSettings)
        {
            var mongoClient = new MongoClient(mongoDbSettings.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(mongoDbSettings.Value.DatabaseName);
            _categoryCollection = mongoDatabase.GetCollection<Category>(mongoDbSettings.Value.CollectionNameCategory);
        }

        public async Task AssignTask(int categoryId, int taskId)
        {
            throw new NotImplementedException();
        }

        public async Task CreateCategory(Category category)
        {
            await _categoryCollection.InsertOneAsync(category);
        }

        public async Task DeleteCategory(int id)
        {
            await _categoryCollection.DeleteOneAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<Category>> GetAllCategories()
        {
            return await _categoryCollection.Find(_ => true).ToListAsync();
        }

        public async Task<Category?> GetCategoryById(int id)
        {
            return await _categoryCollection.Find(_ => _.Id == id).FirstOrDefaultAsync();
        }

        public async Task UpdateCategory(int id, Category category)
        {
            await _categoryCollection.ReplaceOneAsync(x => x.Id == id, category);
        }
    }
}
