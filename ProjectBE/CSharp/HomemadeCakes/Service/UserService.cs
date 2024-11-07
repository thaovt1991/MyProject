using Amazon.Runtime.Internal;
using HomemadeCakes.Common;
using HomemadeCakes.Model;
using HomemadeCakes.Model.Common;
using HomemadeCakes.ModelView;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.Extensions.Configuration;//Doc tu config ra
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using Swashbuckle.Swagger;
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
        private readonly IMongoCollection<User> _usersDBContext;
        private readonly IConfiguration _config; //Doc tu config ra
        private const string ConnectionString = "mongodb://localhost:27017"; //_config["Config:ConnectionString"]
        private const string DatabaseName = "HomemadeCakesDatabase";//_config["Config:DatabaseName"]
        private const string UsersCollectionName = "Users";

        public UserService(IOptions<ConnectDatabaseSettings> conectDatabaseSettings, IConfiguration config)
        {
            var connectionString = config["Config:ConnectionStrings"];
            var databaseName = config["Config:DatabaseName"];
            var client = new MongoClient(connectionString);
            var database = client.GetDatabase(databaseName);

            _usersDBContext = database.GetCollection<User>(UsersCollectionName);
            _config = config;
            //var client = new MongoClient(conectDatabaseSettings.Value.ConnectionString);
            //var database = client.GetDatabase(conectDatabaseSettings.Value.DatabaseName);

            //_usersDBContext = database.GetCollection<User>(conectDatabaseSettings.Value.CollectionName);
        }

        public List<User> Get() =>
            _usersDBContext.Find(user => true).ToList();

        public User Get(string id) =>
            _usersDBContext.Find<User>(user => user.Id == id).FirstOrDefault();

        public User Create(User user)
        {
            _usersDBContext.InsertOne(user);
            return user;
        }

        public void Update(string id, User userIn) =>
            _usersDBContext.ReplaceOne(user => user.Id == id, userIn);

        public void Remove(User userIn) =>
            _usersDBContext.DeleteOne(user => user.Id == userIn.Id);

        public void Remove(string id) =>
            _usersDBContext.DeleteOne(user => user.Id == id);

        public async Task<List<User>> GetAsync() =>
        await _usersDBContext.Find(_ => true).ToListAsync();

        public async Task<User?> GetOneAsync(string id) =>
            await _usersDBContext.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task CreateAsync(User newUser)
        {
            newUser.CreatedOn = DateTime.Now;
            await _usersDBContext.InsertOneAsync(newUser);
        }
        public async Task UpdateAsync(string id, User updatedUser) =>
            await _usersDBContext.ReplaceOneAsync(x => x.Id == id, updatedUser);

        public async Task RemoveAsync(string id) =>
            await _usersDBContext.DeleteOneAsync(x => x.Id == id);
        public async Task<User?> GetOneByUserNameAsync(string userName) =>
           await _usersDBContext.Find(x => x.UserName == userName).FirstOrDefaultAsync();

        public async Task<User?> GetOneByUserIDAsync(string userID) =>
           await _usersDBContext.Find(x => x.UserID == userID).FirstOrDefaultAsync();

        public async Task<bool> IsExitUserIDAsync(string userID)
        {
            return await _usersDBContext.Find(x => x.UserID == userID).AnyAsync();
        }


        public async Task<object> Authencate(LoginRequest request)
        {
            //pas chua mã hóa
            var reponse = new LoginResponse();
            var user = await GetOneByUserIDAsync(request.UserID);
            if (user == null)
            {
                reponse.Error = true;
                reponse.Message = "Không có dữ liệu tương ứng, vui lòng kiểm tra lại";
                return reponse;
            }
            var checkPass = Helper.VerifyHashedPassword(user.Password, request.Password);

            if (!checkPass)
            {
                reponse.Error = true;
                reponse.Message = "Mật khẩu hoặc password không đúng,hãy kiểm tra lại";
                return reponse;
            }

            var claims = new[]
            {
                 new Claim(ClaimTypes.Email, user.Email??"vothao.tin@gmail.com"),
                 new Claim(ClaimTypes.GivenName, user.UserName),
                 new Claim(ClaimTypes.UserData, user.UserName),
                 new Claim("Role", user.Category),
                 new Claim("UserName", user.UserName),
                 new Claim("Phone", user.PhoneNumber??"")
            };

            var keystringJwt = _config["Tokens:Key"];
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(keystringJwt));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddMinutes(5);
            var token = new JwtSecurityToken(
                //  _config["Tokens: Issuer"],
                _config["Tokens:Issuer"],
                //  -xac thuc noi goi issuer :' https://yourwebapp.com' de vao Khi một yêu cầu đến API với một token có iss (issuer) bằng https://yourwebapp.com, hệ thống sẽ xác thực token và cho phép truy cập.
                _config["Tokens:Audience"],
                claims: claims,
                expires: expires,
                signingCredentials: creds
                );

            reponse.Expiress = expires;
            reponse.Maxage = 5 * 3600;
            reponse.Token = new JwtSecurityTokenHandler().WriteToken(token);
            reponse.UserID = user.UserID;

            return reponse;
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
