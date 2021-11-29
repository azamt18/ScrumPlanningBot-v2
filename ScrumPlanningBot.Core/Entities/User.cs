using MongoDB.Bson.Serialization.Attributes;

namespace ScrumPlanningBot.Core.Entities
{
    public class User : BaseEntity
    {
        [BsonElement("telegramId")]
        public long TelegramId { get; set; }

        [BsonElement("phoneNumber")]
        public string PhoneNumber { get; set; }

        [BsonElement("userName")]
        public string UserName { get; set; }

        [BsonElement("fullName")]
        public string FullName { get; set; }
    }
}