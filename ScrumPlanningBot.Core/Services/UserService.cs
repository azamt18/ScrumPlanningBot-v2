using System.Collections.Generic;
using MongoDB.Driver;
using ScrumPlanningBot.Core.Entities;

namespace ScrumPlanningBot.Core.Services
{
    public class UserService
    {
        private readonly IMongoCollection<User> _users;

        public UserService(IMongoDbSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _users = database.GetCollection<User>(settings.UsersCollectionName);
        }

        public void AddUserByChat(long telegramId, string firstName, string lastName, string userName)
        {
            var user = GetByTelegramId(telegramId);
            if (user == null)
            {
                Create(new User()
                {
                    TelegramId = telegramId,
                    FullName = firstName + " " + lastName,
                    UserName = userName
                });
            }
        }

        public List<User> Get() =>
            _users.Find(user => true).ToList();

        public User Get(string id) =>
            _users.Find<User>(user => user.Id == id).FirstOrDefault();

        public User GetByTelegramId(long telegramId) =>
            _users.Find<User>(user => user.TelegramId == telegramId).FirstOrDefault();

        public User Create(User user)
        {
            _users.InsertOne(user);
            return user;
        }

        public void Update(string id, User userIn) =>
            _users.ReplaceOne(book => book.Id == id, userIn);

        public void Remove(User bookIn) =>
            _users.DeleteOne(book => book.Id == bookIn.Id);

        public void Remove(string id) =>
            _users.DeleteOne(book => book.Id == id);
    }
}