using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;

namespace ScrumPlanningBot.Core.Entities
{
    public class Room : BaseEntity
    {
        [BsonElement("title")]
        public string Title { get; set; }

        [BsonElement("stories")]
        public List<Story> Stories { get; set; }

        [BsonElement("users")]
        public List<User> Users { get; set; }
    }
}