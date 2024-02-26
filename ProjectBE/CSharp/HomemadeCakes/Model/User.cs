﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace HomemadeCakes.Model
{
    public class User
    {


        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public Guid RecID { get; set; }

        [BsonElement("Name")]
        public string Name { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public string Category { get; set; }

        public DateTime CreatedOn { get; set; }

        public string CreatedBy { get; set; }
    }
}