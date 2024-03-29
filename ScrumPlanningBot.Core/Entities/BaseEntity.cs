﻿using System;
using System.Globalization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ScrumPlanningBot.Core.Entities
{
    public abstract class BaseEntity
    {
        // standard BSonId generated by MongoDb
        [BsonId]
        [BsonElement("_id")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        // external Id, easier to reference: 1,2,3 or A, B, C etc.
        //[BsonElement("Id")]
        //public string Id { get; set; }

        [BsonElement("createdAt")]
        //[BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        //[DataType(DataType.DateTime)]
        public string CreatedAt { get; set; } = DateTime.Now.ToString(CultureInfo.InvariantCulture);

        [BsonElement("updatedAt")]
        //[BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        //[DataType(DataType.DateTime)]
        public string UpdatedAt { get; set; } = DateTime.Now.ToString(CultureInfo.InvariantCulture);
    }
}