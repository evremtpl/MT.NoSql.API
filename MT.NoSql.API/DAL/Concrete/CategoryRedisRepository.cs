using MT.NoSql.API.DAL.Interfaces;
using MT.NoSql.API.Entities;
using StackExchange.Redis;
using System.Text.Json;
using System.Threading.Tasks;

namespace MT.NoSql.API.DAL.Concrete
{
    public class CategoryRedisRepository : ICategoryRepository
    {

        private readonly IConnectionMultiplexer _redis;
        public CategoryRedisRepository(IConnectionMultiplexer redis)
        {
            _redis = redis;
        }
        public Task AssignTask(int categoryId, int taskId)
        {
            throw new NotImplementedException();
        }

        public async Task CreateCategory(Category category)
        {
            if (category == null)
            {
                throw new ArgumentOutOfRangeException(nameof(category));
            }
            var db = _redis.GetDatabase();

            var serialCategory = JsonSerializer.Serialize(category);

            db.StringSet(category.Id.ToString(), serialCategory);
            db.SetAdd("CategorySet", serialCategory);
        }

        public async Task DeleteCategory(int id)
        {
            var db = _redis.GetDatabase();
            var category = await db.StringGetAsync(id.ToString());
            var serialCategory = JsonSerializer.Serialize(category);
            if (serialCategory != null)
            {
                try
                {
                    var result = db.SetRemove("CategorySet", category);
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

        public async Task<IEnumerable<Category>> GetAllCategories()
        {
            var db = _redis.GetDatabase();

            var completeSet = db.SetMembers("CategorySet");

            if (completeSet.Length > 0)
            {
                var obj = Array.ConvertAll(completeSet, val =>
                    JsonSerializer.Deserialize<Category>(val)).ToList();
                Console.WriteLine("categories ");
                return obj;
            }

            return null;
        }

        public async Task<Category?> GetCategoryById(int id)
        {
            var db = _redis.GetDatabase();


            var category = db.StringGet(id.ToString());

            if (!string.IsNullOrEmpty(category))
            {
                return JsonSerializer.Deserialize<Category>(category);
            }
            return null;
        }

        public async Task UpdateCategory(int id, Category category)
        {
            var db = _redis.GetDatabase();
            var dbCategory = await db.StringGetAsync(category.Id.ToString());
            var serialCategory = JsonSerializer.Serialize(category);
            var serialCategorydb = JsonSerializer.Serialize(dbCategory);
            if (serialCategorydb != null)
            {
                try
                {
                    var result = db.SetRemove("CategorySet", dbCategory);
                    if (result)
                    {
                        Console.WriteLine("Değer başarıyla silindi.");
                        db.StringSet(category.Id.ToString(), serialCategory);
                        db.SetAdd("CategorySet", serialCategory);
                        Console.WriteLine($"{category.CategoryName} değeri başarıyla eklendi.");
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
