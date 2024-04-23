namespace MT.NoSql.API.Settings
{
    public class MongoDbSettings
    {
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
        public string CollectionNameCategory { get; set; }

        public string CollectionNameTask { get; set; }
    }
}
