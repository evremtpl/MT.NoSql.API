using MT.NoSql.API.Entities;

namespace MT.NoSql.API.DAL.Interfaces
{
    public interface ICategoryRepository 
    {
        Task CreateCategory(Category category);

        Task AssignTask(int categoryId, int taskId);
        Task<Category?> GetCategoryById(int id);
        Task<IEnumerable<Category>> GetAllCategories();

        Task DeleteCategory(int id);

        Task UpdateCategory(int id, Category category);
    }
}
