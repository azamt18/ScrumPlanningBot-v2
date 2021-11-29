using System;
using System.Collections.Generic;
using System.Globalization;
using MongoDB.Bson.Serialization.Attributes;

namespace ScrumPlanningBot.Core.Entities
{
    public class Story
    {
        // external Id, easier to reference: 1,2,3 or A, B, C etc.
        [BsonElement("_id")]
        public string Id { get; set; } = new Random().Next(100, 999).ToString();

        [BsonElement("createdAt")]
        public string CreatedAt { get; set; } = DateTime.Now.ToString(CultureInfo.InvariantCulture);

        [BsonElement("updatedAt")]
        public string UpdatedAt { get; set; } = DateTime.Now.ToString(CultureInfo.InvariantCulture);

        [BsonElement("title")]
        public string Title { get; set; }

        [BsonElement("creatorUserId")]
        public long CreatorUserId { get; set; }

        [BsonElement("users")]
        public List<StoryUser> Users { get; set; }

        [BsonElement("isClosed")]
        public bool IsClosed { get; set; }

        [BsonElement("closedAt")]
        public string ClosedAt { get; set; } = DateTime.Now.ToString(CultureInfo.InvariantCulture);

        [BsonElement("averageStoryPoint")]
        public double AverageStoryPoint { get; set; }

        [BsonElement("averageHour")]
        public double AverageHour { get; set; }
    }

    public class StoryUser
    {
        [BsonElement("userId")]
        public long UserId { get; set; }

        [BsonElement("storyPoint")]
        public double StoryPoint { get; set; }

        [BsonElement("hour")]
        public double Hour { get; set; }
    }
}