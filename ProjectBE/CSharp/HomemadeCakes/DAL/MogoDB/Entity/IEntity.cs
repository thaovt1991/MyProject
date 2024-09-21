using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace HomemadeCakes.DAL.MogoDB.Entity
{
    public interface IEntity
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        string Id { get; set; }
    }
}
