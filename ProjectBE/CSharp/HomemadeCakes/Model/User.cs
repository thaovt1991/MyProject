using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace HomemadeCakes.Model
{
    public class User
    {
        public User()
        {
            this.Id = ObjectId.GenerateNewId().ToString();
            this.RecID = Guid.NewGuid();
            this.CreatedOn = DateTime.Now;
          
        }
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public Guid RecID { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string UserID { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public string Category { get; set; }

        public DateTime CreatedOn { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public string ModifiedBy { get; set; }

        public string PhoneNumber { get; set; }

        public string Email { get; set; }
    }
}
