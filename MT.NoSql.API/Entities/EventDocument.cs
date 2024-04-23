namespace MT.NoSql.API.Entities
{
    public class EventDocument
    {
        public string Id { get; set; }
        public string Subject { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
    }
}
