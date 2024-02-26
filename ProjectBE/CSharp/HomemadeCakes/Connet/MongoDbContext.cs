using HomemadeCakes.Model;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HomemadeCakes.Connet
{
    public class MongoDbContext
    {
        private readonly IMongoDatabase _database;

        //public MongoDbContext(IMongoOptions options)
        //{
        //    var client = new MongoClient(options.ConnectionString);
        //    _database = client.GetDatabase(options.DefaultDatabase);
        //}

        public IMongoCollection<User> users => _database.GetCollection<User>(nameof(User));
    }
}
