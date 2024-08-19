using Amazon.Runtime.Internal;
using HomemadeCakes.Model;
using HomemadeCakes.ModelView;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace HomemadeCakes.Service
{
    public class UserService : IUserService
    {
        private readonly IMongoCollection<User> _users;
        private readonly IConfiguration _config;
        private const string ConnectionString = "mongodb://localhost:27017";
        private const string DatabaseName = "HomemadeCakesDatabase";
        private const string UsersCollectionName = "Users";
        public UserService(IOptions<ConnectDatabaseSettings> conectDatabaseSettings, IConfiguration config)
        {
            var client = new MongoClient(ConnectionString);
            var database = client.GetDatabase(DatabaseName);

            _users = database.GetCollection<User>(UsersCollectionName);
            _config = config;
            //var client = new MongoClient(conectDatabaseSettings.Value.ConnectionString);
            //var database = client.GetDatabase(conectDatabaseSettings.Value.DatabaseName);

            //_users = database.GetCollection<User>(conectDatabaseSettings.Value.CollectionName);
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

        public async Task<User?> GetOneAsync(string id) =>
            await _users.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task CreateAsync(User newUser)
        {
            newUser.CreatedOn = DateTime.Now;
            await _users.InsertOneAsync(newUser);
        }
        public async Task UpdateAsync(string id, User updatedUser) =>
            await _users.ReplaceOneAsync(x => x.Id == id, updatedUser);

        public async Task RemoveAsync(string id) =>
            await _users.DeleteOneAsync(x => x.Id == id);
        public async Task<User?> GetOneByUserNameAsync(string userName) =>
           await _users.Find(x => x.UserName == userName).FirstOrDefaultAsync();
        public async Task<string> Authencate(LoginRequest request)
        {
            var user = await GetOneByUserNameAsync(request.UserName);
            if (user == null) return null;
            var claims = new[]
            {
                new Claim(ClaimTypes.Email, user.Email),
                 new Claim(ClaimTypes.GivenName, user.FirstName),
                 new Claim(ClaimTypes.Email, user.Email)
            };
            //var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Tokens: Key"]));
 
            var keystringJwt = "cuocdoivandepsaotinhyeuvandepsao22011991love15071996somuchs";
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(keystringJwt));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now,
                signingCredentials: creds
                );
            return new JwtSecurityTokenHandler().WriteToken(token);
            //Tham khao
            //Chuyen pass
            //sUserName = AESCrypto.Decrypt(sUserName);
            //sPassword = AESCrypto.Decrypt(sPassword);

            //check pass =>   var checkPass = Helper.VerifyHashedPassword(oUser.Password, password); 
            //Chuyen pass

            //var expired = DateTime.UtcNow.AddDays(1);
            //var securityKey = Guid.NewGuid().ToString();
            //var key = Encoding.ASCII.GetBytes(LVConfig.Settings.Secret);
            //var tokenDescriptor = new SecurityTokenDescriptor
            //{
            //    Subject = new ClaimsIdentity(new[]
            //{
            //        new Claim(ClaimTypes.Name, oUser.AccountID),
            //        new Claim(JwtRegisteredClaimNames.NameId, oUser.AccountID),
            //        new Claim(JwtRegisteredClaimNames.Email, oUser.Email),
            //        new Claim("FullName", oUser.AccountName),
            //        new Claim("sk",securityKey),
            //        new Claim("Mobile", oUser.Phone ?? ""),
            //        new Claim("TenantID",RequestContext.RequestSession.Tenant),
            //        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            //    }),
            //    Issuer = "erm.lacviet.vn",
            //    Audience = "erm.lacviet.vn",
            //    Expires = expired,
            //    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            //};

            //var jwtTokenHandler = new JwtSecurityTokenHandler();
            //var token = jwtTokenHandler.CreateToken(tokenDescriptor);
            //var jwtToken = jwtTokenHandler.WriteToken(token);
        }

        public Task<bool> Register(RegisterRequest request)
        {
            return null;
        }
    }
}
