using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using ScrumPlanningBot.Core.Entities;

namespace ScrumPlanningBot.Core.Services
{
    public class RoomService
    {
        private readonly IMongoCollection<Room> _rooms;

        public RoomService(IMongoDbSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _rooms = database.GetCollection<Room>(settings.RoomsCollectionName);
        }

        public List<Room> Get() =>
            _rooms.Find(room => true).ToList();

        public Room Get(string id) =>
            _rooms.Find<Room>(room => room.Id == id).FirstOrDefault();

        public Room Create(Room room)
        {
            _rooms.InsertOne(room);
            return room;
        }

        public void Update(string id, Room bookIn) =>
            _rooms.ReplaceOne(room => room.Id == id, bookIn);

        public void Remove(Room bookIn) =>
            _rooms.DeleteOne(room => room.Id == bookIn.Id);

        public void Remove(string id) =>
            _rooms.DeleteOne(room => room.Id == id);
    }
}