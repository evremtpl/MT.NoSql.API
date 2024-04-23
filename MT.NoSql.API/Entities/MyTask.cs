namespace MT.NoSql.API.Entities
{
    public class MyTask
    {
        public int Id { get; set; }
        public string TaskName { get; set; }
        public bool Completed { get; set; }
        public int CategoryId { get; set; }
 
    }
}
