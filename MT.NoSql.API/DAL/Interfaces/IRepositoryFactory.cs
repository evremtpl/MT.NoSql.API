namespace MT.NoSql.API.DAL.Interfaces
{
    public interface IRepositoryFactory
    {

        public Task<ITaskRepository> CreateTaskRepository(DataStore store);

        public Task<ICategoryRepository?> CreateCategoryRepository(DataStore store);
    }

    public  enum DataStore
    {
        Neo4j,
        Neo4jCompany,
        Neo4jCategory,
        Neo4jTask,
        CouchBase,
        Mongo,
        Redis

    }
}
