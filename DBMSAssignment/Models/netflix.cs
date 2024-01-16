
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Bson.Serialization;
using System.Globalization;

namespace DBMSAssignment.Models
{
    public class Netflix
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("id")]
        public string MovieId { get; set; }

        [BsonElement("imdb_score")]
        public double ImdbScore { get; set; }        

        [BsonElement("age_certification")]
        public string Age_Certification { get; set; }

        [BsonElement("description")]
        public string Description { get; set; }

        [BsonElement("genres")]
        public List<string> Genres { get; set; }

        [BsonElement("production_countries")]
        public List<string> Production_Countries { get; set; }

        [BsonElement("release_year")]
        public int Release_Year { get; set; }

        [BsonElement("runtime")]
        public int Runtime { get; set; }

        [BsonElement("title")]
        [BsonSerializer(typeof(BsonStringNumericSerializer))]
        public string Title { get; set; }

        [BsonElement("type")]
        public string Type { get; set; }
    }

    public class BsonStringNumericSerializer : SerializerBase<string>
    {
        public override string Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
        {
            var bsonType = context.Reader.CurrentBsonType;
            switch (bsonType)
            {
                case BsonType.Null:
                    context.Reader.ReadNull();
                    return null;
                case BsonType.String:
                    return context.Reader.ReadString();
                case BsonType.Int32:
                    return context.Reader.ReadInt32().ToString(CultureInfo.InvariantCulture);
                default:
                    var message = string.Format($"Custom Cannot deserialize BsonString or BsonInt32 from BsonType {bsonType}");
                    throw new BsonSerializationException(message);
            }
        }

        public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, string value)
        {
            if (value != null)
            {
                if (int.TryParse(value, out var result))
                {
                    context.Writer.WriteInt32(result);
                }
                else
                {
                    context.Writer.WriteString(value);
                }
            }
            else
            {
                context.Writer.WriteNull();
            }
        }
    }
}
