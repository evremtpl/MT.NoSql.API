using Amazon.Runtime.Internal.Transform;
using Couchbase.Core;
using Couchbase.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MT.NoSql.API.DAL.Concrete;
using MT.NoSql.API.DAL.Interfaces;
using MT.NoSql.API.Settings;
using Neo4jClient;
using StackExchange.Redis;

namespace MT.NoSql.API.DAL.Concrete
{
    public class RepositoryFactory : IRepositoryFactory
    {



        private readonly IOptions<MongoDbSettings> _mongoDbSettings;
        private readonly IBucketProvider _bucketProvider;
        private readonly IGraphClient _graphClient;
        private readonly IConnectionMultiplexer _redis;
        Dictionary<DataStore, ITaskRepository> dictTaskStore = new Dictionary<DataStore, ITaskRepository>();
        Dictionary<DataStore, ICategoryRepository> dictCategoryStore = new Dictionary<DataStore, ICategoryRepository>();
        public RepositoryFactory(IBucketProvider bucketProvider, IOptions<MongoDbSettings> mongoDbSettings, IGraphClient graphClient, IConnectionMultiplexer redis)
        {

            _mongoDbSettings = mongoDbSettings;
            _bucketProvider = bucketProvider;
            _graphClient = graphClient;
            _redis = redis;
        }

        public async  Task<ITaskRepository> CreateTaskRepository(DataStore store)
        {
            switch (store)
            {

                case DataStore.Neo4jTask:

                    if (dictTaskStore.ContainsKey(DataStore.Neo4jTask)) return dictTaskStore[DataStore.Neo4jTask];
                    dictTaskStore.Add(DataStore.Neo4jTask, new TaskNeo4jRepository(_graphClient));
                    return dictTaskStore[DataStore.Neo4jTask];
                case DataStore.Redis:
                    if (dictTaskStore.ContainsKey(DataStore.Redis)) return dictTaskStore[DataStore.Redis];
                    dictTaskStore.Add(DataStore.Redis, new TaskRedisRepository(_redis));
                    return dictTaskStore[DataStore.Redis];
                case DataStore.Mongo:
                    if (dictTaskStore.ContainsKey(DataStore.Mongo)) return dictTaskStore[DataStore.Mongo];
                    dictTaskStore.Add(DataStore.Mongo, new TaskMongoRepository(_mongoDbSettings));
                    return dictTaskStore[DataStore.Mongo];
                case DataStore.CouchBase:
                    if (dictTaskStore.ContainsKey(DataStore.CouchBase)) return dictTaskStore[DataStore.CouchBase];
                    dictTaskStore.Add(DataStore.CouchBase, new TaskCouchBaseRepository(_bucketProvider));
                    return dictTaskStore[DataStore.CouchBase];
                default:
                    throw new NotImplementedException();
            }
        }

        public async Task<ICategoryRepository?> CreateCategoryRepository(DataStore store)
        {
            switch (store)
            {

                case DataStore.Neo4jCategory:

                    if (dictCategoryStore.ContainsKey(DataStore.Neo4jTask)) return dictCategoryStore[DataStore.Neo4jTask];
                    dictCategoryStore.Add(DataStore.Neo4jTask, new CategoryNeo4jRepository(_graphClient));
                    return dictCategoryStore[DataStore.Neo4jTask];
                case DataStore.Redis:
                    if (dictCategoryStore.ContainsKey(DataStore.Redis)) return dictCategoryStore[DataStore.Redis];
                    dictCategoryStore.Add(DataStore.Redis, new CategoryRedisRepository(_redis));
                    return dictCategoryStore[DataStore.Redis];
                case DataStore.Mongo:
                    if (dictCategoryStore.ContainsKey(DataStore.Mongo)) return dictCategoryStore[DataStore.Mongo];
                    dictCategoryStore.Add(DataStore.Mongo, new CategoryMongoRepository(_mongoDbSettings));
                    return dictCategoryStore[DataStore.Mongo];
                case DataStore.CouchBase:
                    if (dictCategoryStore.ContainsKey(DataStore.CouchBase)) return dictCategoryStore[DataStore.CouchBase];
                    dictCategoryStore.Add(DataStore.CouchBase, new CategoryCouchBaseRepository(_bucketProvider));
                    return dictCategoryStore[DataStore.CouchBase];
                default:
                    throw new NotImplementedException();
            }
        }
    }
}



