using Couchbase;
using Couchbase.Core;
using Couchbase.Extensions.DependencyInjection;
using MT.NoSql.API.DAL.Interfaces;
using MT.NoSql.API.Entities;
using Newtonsoft.Json;

namespace MT.NoSql.API.DAL.Concrete
{
    public class CategoryCouchBaseRepository : ICategoryRepository
    {

        private readonly IBucket _bucket;


        public CategoryCouchBaseRepository(IBucketProvider bucketProvider)
        {


            _bucket = bucketProvider.GetBucket("categories");

        }
        public Task AssignTask(int categoryId, int taskId)
        {
            throw new NotImplementedException();
        }

        public async Task CreateCategory(Category category)
        {
            var document = new Document<Category>
            {
                Id = Guid.NewGuid().ToString(),
                Content = category
            };


            { await _bucket.InsertAsync(document); }
        }

        public async Task DeleteCategory(int id)
        {
            try
            {
                var query = $"DELETE FROM {_bucket.Name} AS a  WHERE a.id = {id} RETURNING a";
                var result = _bucket.Query<Category>(query);
                if (result.Success)
                {
                    string documentContent = JsonConvert.SerializeObject(result.Rows.FirstOrDefault());


                    Category a = JsonConvert.DeserializeObject<Category>(documentContent);

                }
            }
            catch (Exception ex)
            {

                throw ex;
            }


        }

        public async Task<IEnumerable<Category>> GetAllCategories()
        {


            try
            {
                var categories = new List<Category>();
                var query = $"SELECT id,color,categoryName FROM {_bucket.Name}";
                var result = _bucket.Query<Category>(query);

                foreach (var row in result.Rows)
                {
                    string documentContent = JsonConvert.SerializeObject(row);


                    Category document = JsonConvert.DeserializeObject<Category>(documentContent);

                    categories.Add(document);
                    Console.WriteLine($"Belge bulundu: {document}");
                }



                return categories;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public async Task<Category?> GetCategoryById(int id)
        {
            var query = $"SELECT id,color,categoryName FROM {_bucket.Name} WHERE id = {id} LIMIT 1";
            var result = _bucket.Query<Category>(query);

            // var document = await _bucket.GetDocumentAsync<Category>("d16f955e-4ea5-4225-a9cf-a8bdde5407a2");

            if (result.Success)
            {
                string documentContent = JsonConvert.SerializeObject(result.Rows.FirstOrDefault());


                Category document = JsonConvert.DeserializeObject<Category>(documentContent);
                return document;
            }
            else
            {
                return null;
            }
        }

        public async Task UpdateCategory(int id, Category category)
        {
            try
            {
                var query = @$"UPDATE {_bucket.Name} SET color = ""{category.Color}"", categoryName =""{category.CategoryName}""  WHERE id = {id} RETURNING a";
                var result = _bucket.Query<Category>(query);
                if (result.Success)
                {
                    string documentContent = JsonConvert.SerializeObject(result.Rows.FirstOrDefault());


                    Category a = JsonConvert.DeserializeObject<Category>(documentContent);

                }

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
