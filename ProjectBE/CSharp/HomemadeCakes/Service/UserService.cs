using HomemadeCakes.Model;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HomemadeCakes.Service
{
    public class UserService
    {
        private readonly IMongoCollection<User> _users;
        //private const string ConnectionString = "mongodb://localhost:27017";
        //private const string DatabaseName = "HomemadeCakes";
        //private const string UsersCollectionName = "Users";
        public UserService(IOptions<ConnectDatabaseSettings> conectDatabaseSettings)
        {
            //var client = new MongoClient(ConnectionString);
            //var database = client.GetDatabase(DatabaseName);

            //_users = database.GetCollection<User>(CollectionName);
            var client = new MongoClient(conectDatabaseSettings.Value.ConnectionString);
            var database = client.GetDatabase(conectDatabaseSettings.Value.DatabaseName);

            _users = database.GetCollection<User>(conectDatabaseSettings.Value.CollectionName);
        }

        public List<User> Get() =>
            _users.Find(user => true).ToList();

        public User Get(string id) =>
            _users.Find<User>(user => user.Id == id).FirstOrDefault();

        public User Create(User user)
        {
            _users.InsertOne(user);
            return user;
        }

        public void Update(string id, User userIn) =>
            _users.ReplaceOne(user => user.Id == id, userIn);

        public void Remove(User userIn) =>
            _users.DeleteOne(user => user.Id == userIn.Id);

        public void Remove(string id) =>
            _users.DeleteOne(user => user.Id == id);

        public async Task<List<User>> GetAsync() =>
        await _users.Find(_ => true).ToListAsync();

        public async Task<User?> GetAsync(string id) =>
            await _users.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task CreateAsync(User newUser) =>
            await _users.InsertOneAsync(newUser);

        public async Task UpdateAsync(string id, User updatedUser) =>
            await _users.ReplaceOneAsync(x => x.Id == id, updatedUser);

        public async Task RemoveAsync(string id) =>
            await _users.DeleteOneAsync(x => x.Id == id);
    }
}
