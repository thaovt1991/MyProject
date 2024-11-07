using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;

namespace HomemadeCakes.Model.NumberModels
{
    public class Category
    {
        //dai
        public Category()
        {
            this.Id = ObjectId.GenerateNewId().ToString();
            this.RecID = Guid.NewGuid().ToString();
            this.CreatedOn = DateTime.Now;

        }
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string RecID { get; set; }
        public string Name { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string ModifiedBy { get; set; }

    }
}
