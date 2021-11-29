namespace ScrumPlanningBot.Core.Entities
{
    public class MongoDbSettings : IMongoDbSettings
    {
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
        public string UsersCollectionName { get; set; }
        public string RoomsCollectionName { get; set; }
        public string BooksCollectionName { get; set; }
    }

    public interface IMongoDbSettings
    {
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
        string UsersCollectionName { get; set; }
        string RoomsCollectionName { get; set; }
        string BooksCollectionName { get; set; }
    }
}