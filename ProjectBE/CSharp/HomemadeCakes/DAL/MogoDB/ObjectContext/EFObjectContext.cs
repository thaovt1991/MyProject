using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Bson.Serialization;
using MongoDB.Bson;
using MongoDB.Driver.Core.Compression;
using MongoDB.Driver.Core.Configuration;
using MongoDB.Driver.Linq;
using MongoDB.Driver;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System;
using System.Linq;
using HomemadeCakes.Common;
using HomemadeCakes.DAL.Interface;
using HomemadeCakes.DAL.MogoDB.Conventions;
using HomemadeCakes.Common.Interface;
using HomemadeCakes.Common.Config;
using HomemadeCakes.Common.Entity;
using HomemadeCakes.DAL.MogoDB.Entity;
using HomemadeCakes.Common.Http;

namespace HomemadeCakes.DAL.MogoDB.ObjectContext
{
    public class EFObjectContext : IDbContext, IDisposable
    {
        private static ConcurrentDictionary<string, IMongoClient> _dicClients = new ConcurrentDictionary<string, IMongoClient>();

        private static bool _globalRegister = false;

        private object _lock = new object();

        private object _lockClls = new object();

        private readonly ConcurrentDictionary<string, ObjectStateManager> objStateManagers = new ConcurrentDictionary<string, ObjectStateManager>();

        internal ConcurrentDictionary<string, object> _collections { get; } = new ConcurrentDictionary<string, object>();


        public bool IsDateTimeUTC { get; set; } = true;


        public IMongoClient Client { get; set; }

        public IMongoDatabase Database { get; set; }

        public string DatabaseName => Database?.DatabaseNamespace?.DatabaseName;

        public IConnection Connection { get; private set; }

        public int CommandTimeout => (Client?.Settings?.ConnectTimeout.Seconds).Value;

        public MessageBody RequestSession => PJRequestContext.Current?.RequestSession;

        internal bool HasChanges
        {
            get
            {
                bool flag = false;
                foreach (KeyValuePair<string, ObjectStateManager> objStateManager in objStateManagers)
                {
                    if (objStateManager.Value != null)
                    {
                        flag = objStateManager.Value.HasChanges;
                        if (flag)
                        {
                            break;
                        }
                    }
                }

                return flag;
            }
        }

        public EFObjectContext(IConnection connection, bool isDateTimeUTC = true)
        {
            IsDateTimeUTC = isDateTimeUTC;
            if (!_globalRegister)
            {
                _globalRegister = true;
                BsonSerializer.RegisterSerializer(DateTimeSerializer.LocalInstance);
                BsonSerializer.RegisterSerializer(new GuidSerializer(BsonType.String));
                ConventionPack conventions = new ConventionPack
            {
                new IgnoreExtraElementsConvention(ignoreExtraElements: true)
            };
                ConventionRegistry.Register("IgnoreExtraElements", conventions, (t) => true);
                ConventionRegistry.Register("IgnoreNotMapped", EFAttributeConventionPack.Instance, (t) => true);
            }

            Connection = connection;
            if (!Connection.IsSystem)
            {
                Connection.IsSystem = (string.IsNullOrEmpty(PJConfig.Settings.MultiDB) || PJConfig.Settings.MultiDB == "0") && connection.Type.ToLower() == PJConfig.Settings.DBNameType.ToLower();
            }

            if (string.IsNullOrEmpty(connection.CnnString) && Connection.IsConfig)
            {
                Connection.CnnString = PJConfig.Connections[Connection.DBName];
                connection.IsConfig = false;
                if (string.IsNullOrEmpty(connection.CnnString) && !string.IsNullOrEmpty(connection.Type))
                {
                    connection.CnnString = PJConfig.Connections[connection.Type];
                }
            }

            if (string.IsNullOrEmpty(Connection.CnnString))
            {
                throw new Exception("EFDbContext: not found connectionString [" + Connection.DBName + "]");
            }

            MongoUrl mongoUrl = new MongoUrl(Connection.CnnString);
            string text = Connection.DBName;
            if (string.IsNullOrEmpty(text))
            {
                text = mongoUrl.DatabaseName;
            }

            string key = Connection.CnnString.ToLower();
            lock (_lock)
            {
                if (!_dicClients.ContainsKey(key))
                {
                    MongoClientSettings mongoClientSettings = MongoClientSettings.FromUrl(mongoUrl);
                    mongoClientSettings.LinqProvider = LinqProvider.V3;
                    mongoClientSettings.Compressors = new List<CompressorConfiguration>
                {
                    new CompressorConfiguration(CompressorType.Snappy),
                    new CompressorConfiguration(CompressorType.Zlib),
                    new CompressorConfiguration(CompressorType.ZStandard)
                };
                    _dicClients.TryAdd(key, new MongoClient(mongoClientSettings));
                }

                Client = _dicClients[key];
                Database = Client.GetDatabase(text);
            }
        }

        public bool Exists(string collectionName)
        {
            BsonDocument bsonDocument = new BsonDocument("name", collectionName);
            return Database.ListCollections(new ListCollectionsOptions
            {
                Filter = bsonDocument
            }).Any();
        }

        public async Task<bool> ExistsAsync(string collectionName)
        {
            BsonDocument bsonDocument = new BsonDocument("name", collectionName);
            return await (await Database.ListCollectionsAsync(new ListCollectionsOptions
            {
                Filter = bsonDocument
            })).AnyAsync();
        }

        public IMongoCollection<TEntity> GetCollection<TEntity>() where TEntity : class
        {
            string name = typeof(TEntity).Name;
            if (!_collections.ContainsKey(name))
            {
                lock (_lockClls)
                {
                    _collections.TryAdd(name, Database.GetCollection<TEntity>(name));
                }
            }

            return (IMongoCollection<TEntity>)_collections[name];
        }

        public IMongoCollection<BsonDocument> GetCollection(string name)
        {
            string key = name + "_BsonDocument";
            if (!_collections.ContainsKey(key))
            {
                if (!Exists(name))
                {
                    IMongoDatabase database = Database;
                    CreateCollectionOptions createCollectionOptions = new CreateCollectionOptions();
                    Optional<CollationStrength?> strength = CollationStrength.Secondary;
                    createCollectionOptions.Collation = new Collation("en_US", default, default, strength);
                    database.CreateCollection(name, createCollectionOptions);
                }

                lock (_lockClls)
                {
                    _collections.TryAdd(key, Database.GetCollection<BsonDocument>(name));
                }
            }

            return (IMongoCollection<BsonDocument>)_collections[key];
        }

        public void SetCollection<TEntity>(IMongoCollection<TEntity> collection) where TEntity : class
        {
            string name = typeof(TEntity).Name;
            if (!_collections.ContainsKey(name))
            {
                lock (_lockClls)
                {
                    _collections.TryAdd(name, collection);
                }
            }
        }

        public TEntity GetOriginal<TEntity>(TEntity entity) where TEntity : class
        {
            Type type = entity.GetType();
            IMongoCollection<TEntity> collection = GetCollection<TEntity>();
            BsonDocument bsonDocument = entity.ToBsonDocument(type);
            FilterDefinition<TEntity> filter = Builders<TEntity>.Filter.Eq("_id", bsonDocument.GetValue("_id"));
            return collection.Find(filter).FirstOrDefault();
        }

        public List<IEntity> GetDataState<TEntity>(DataState dataState) where TEntity : class
        {
            string name = typeof(TEntity).Name;
            if (!objStateManagers.ContainsKey(name))
            {
                ObjectStateManager objectStateManager = objStateManagers[name];
                switch (dataState)
                {
                    case DataState.Added:
                        return objectStateManager.AddedEntities.Values.ToList();
                    case DataState.Modified:
                        return objectStateManager.UpdatedEntities.Values.ToList();
                    case DataState.Deleted:
                        return objectStateManager.DeletedEntities.Values.ToList();
                }
            }

            return new List<IEntity>();
        }

        public List<ObjectStateManager> GetDataChanges()
        {
            return objStateManagers.Values.ToList();
        }

        public IMongoQueryable<TEntity> GetQuery<TEntity>(Expression<Func<TEntity, bool>> filter = null) where TEntity : class
        {
            IMongoCollection<TEntity> collection = GetCollection<TEntity>();
            if (filter == null)
            {
                return collection.AsQueryable();
            }

            return collection.AsQueryable().Where(filter);
        }

        public void Add(object entity)
        {
            if (entity is IEntity)
            {
                string name = entity.GetType().Name;
                if (!objStateManagers.ContainsKey(name))
                {
                    objStateManagers.TryAdd(name, new ObjectStateManager());
                }

                objStateManagers[name].SetAdd(entity);
            }
        }

        public void Update(object entity)
        {
            if (entity is IEntity)
            {
                string name = entity.GetType().Name;
                if (!objStateManagers.ContainsKey(name))
                {
                    objStateManagers.TryAdd(name, new ObjectStateManager());
                }

                objStateManagers[name].SetUpdate(entity);
            }
        }

        public void Delete(object entity)
        {
            if (entity is IEntity)
            {
                string name = entity.GetType().Name;
                if (!objStateManagers.ContainsKey(name))
                {
                    objStateManagers.TryAdd(name, new ObjectStateManager());
                }

                objStateManagers[name].SetDelete(entity);
            }
        }

        public void Delete<TEntity>(Expression<Func<TEntity, bool>> criteria) where TEntity : class
        {
            if (criteria == null)
            {
                return;
            }

            Type typeFromHandle = typeof(TEntity);
            if (!typeof(IEntity).IsAssignableFrom(typeFromHandle))
            {
                return;
            }

            string name = typeFromHandle.Name;
            if (!objStateManagers.ContainsKey(name))
            {
                objStateManagers.TryAdd(name, new ObjectStateManager());
            }

            ObjectStateManager objectStateManager = objStateManagers[name];
            foreach (TEntity item in GetCollection<TEntity>().Find(criteria).ToEnumerable())
            {
                objectStateManager.SetDelete(item);
            }
        }

        public void Delete<TEntity>(string predicate, params object[] values) where TEntity : class
        {
            if (string.IsNullOrEmpty(predicate))
            {
                return;
            }

            Type typeFromHandle = typeof(TEntity);
            if (!typeof(IEntity).IsAssignableFrom(typeFromHandle))
            {
                return;
            }

            string name = typeFromHandle.Name;
            if (!objStateManagers.ContainsKey(name))
            {
                objStateManagers.TryAdd(name, new ObjectStateManager());
            }

            ObjectStateManager objectStateManager = objStateManagers[name];
            //foreach (TEntity item in GetCollection<TEntity>().AsQueryable().Where(predicate, values).ToList())
            //{
            //    objectStateManager.SetDelete(item);
            //}
        }

        public int SaveChanges(bool transaction = false)
        {
            if (transaction)
            {
                return Commit();
            }

            return Save();
        }

        public Task<int> SaveChangesAsync(bool transaction = false)
        {
            if (transaction)
            {
                return CommitAsync();
            }

            return SaveAsync();
        }

        private int Save(IClientSessionHandle session = null)
        {
            int num = -1;
            Dictionary<string, ErrorResult> dictionary = new Dictionary<string, ErrorResult>();
            int num2 = 0;
            int num3 = 0;
            int num4 = 0;
            foreach (KeyValuePair<string, ObjectStateManager> objStateManager in objStateManagers)
            {
                if (objStateManager.Value != null && objStateManager.Value.HasChanges)
                {
                    num = 0;
                    IMongoCollection<BsonDocument> collection = GetCollection(objStateManager.Key);
                    var (addedResult, num5) = AddItems(collection, objStateManager.Value.AddedEntities, session);
                    var (updatedResult, num6) = UpdateItems(collection, objStateManager.Value.UpdatedEntities, session);
                    var (deletedResult, num7) = DeleteItems(collection, objStateManager.Value.DeletedEntities, session);
                    objStateManager.Value.AddedEntities.Clear();
                    objStateManager.Value.UpdatedEntities.Clear();
                    objStateManager.Value.DeletedEntities.Clear();
                    num2 += num5;
                    num3 += num6;
                    num4 += num7;
                    ErrorResult.CreateErrorResult(addedResult, updatedResult, deletedResult, objStateManager.Key, dictionary);
                }
            }

            objStateManagers.Clear();
            if (num == -1)
            {
                return num;
            }

            if (dictionary.Count > 0)
            {
                throw new ExceptionResult("SaveChanges Error!")
                {
                    ErrorResults = dictionary,
                    AddedCount = num2,
                    UpdatedCount = num3,
                    DeletedCount = num4,
                    HasError = dictionary.Count > 0
                };
            }

            return num2 + num3 + num4;
        }

        private async Task<int> SaveAsync(IClientSessionHandle session = null)
        {
            int iReturn = -1;
            Dictionary<string, ErrorResult> errorResults = new Dictionary<string, ErrorResult>();
            int addedCount = 0;
            int updatedCount = 0;
            int deletedCount = 0;
            foreach (KeyValuePair<string, ObjectStateManager> obj in objStateManagers)
            {
                if (obj.Value != null && obj.Value.HasChanges)
                {
                    iReturn = 0;
                    IMongoCollection<BsonDocument> collection = GetCollection(obj.Key);
                    var (addedErrors, iAddedCount) = await AddItemsAsync(collection, obj.Value.AddedEntities);
                    var (updatedErrors, iUpdatedCount) = await UpdateItemsAsync(collection, obj.Value.UpdatedEntities);
                    var (deletedResult, num) = await DeleteItemsAsync(collection, obj.Value.DeletedEntities);
                    obj.Value.AddedEntities.Clear();
                    obj.Value.UpdatedEntities.Clear();
                    obj.Value.DeletedEntities.Clear();
                    addedCount += iAddedCount;
                    updatedCount += iUpdatedCount;
                    deletedCount += num;
                    ErrorResult.CreateErrorResult(addedErrors, updatedErrors, deletedResult, obj.Key, errorResults);
                }
            }

            objStateManagers.Clear();
            if (iReturn == -1)
            {
                return iReturn;
            }

            if (errorResults.Count > 0)
            {
                throw new ExceptionResult("SaveChanges Error!")
                {
                    AddedCount = addedCount,
                    UpdatedCount = updatedCount,
                    DeletedCount = deletedCount,
                    HasError = errorResults.Count > 0,
                    ErrorResults = errorResults
                };
            }

            return addedCount + updatedCount + deletedCount;
        }

        private (List<ActionResult>, int) AddItems(IMongoCollection<BsonDocument> collection, ConcurrentDictionary<string, IEntity> items, IClientSessionHandle session = null)
        {
            List<ActionResult> list = new List<ActionResult>();
            int num = 0;
            foreach (KeyValuePair<string, IEntity> item in items)
            {
                IEntity value = item.Value;
                try
                {
                    BsonDocument document = SetDataDefault(value);
                    if (session == null)
                    {
                        collection.InsertOne(document);
                    }
                    else
                    {
                        collection.InsertOne(session, document);
                    }

                    num++;
                }
                catch (Exception ex)
                {
                    list.Add(new ActionResult("EFObjectContext_Save_AddItems", ex)
                    {
                        ObjectData = value
                    });
                    Log.Instance.Error(ex, "EFObjectContext AddItems");
                }
            }

            return (list, num);
        }

        private async Task<(List<ActionResult>, int)> AddItemsAsync(IMongoCollection<BsonDocument> collection, ConcurrentDictionary<string, IEntity> items, IClientSessionHandle session = null)
        {
            List<ActionResult> lstActionResults = new List<ActionResult>();
            int iCount = 0;
            foreach (KeyValuePair<string, IEntity> item in items)
            {
                IEntity entity = item.Value;
                try
                {
                    BsonDocument document = SetDataDefault(entity);
                    if (session != null)
                    {
                        await collection.InsertOneAsync(session, document);
                    }
                    else
                    {
                        await collection.InsertOneAsync(document);
                    }

                    iCount++;
                }
                catch (Exception ex)
                {
                    lstActionResults.Add(new ActionResult("EFObjectContext_Save_AddItemsAsync", ex)
                    {
                        ObjectData = entity
                    });
                    Log.Instance.Error(ex, "EFObjectContext AddItemsAsync");
                }
            }

            return (lstActionResults, iCount);
        }

        private BsonDocument SetDataDefault<TEntity>(TEntity entity) where TEntity : class
        {
            Type type = entity.GetType();
            BsonDocument bsonDocument = entity.ToBsonDocument(type);
            if (RequestSession != null)
            {
                if (bsonDocument.Contains("Owner"))
                {
                    if (bsonDocument.GetElement("Owner").Value.ToString() == "")
                    {
                        bsonDocument.Set("Owner", RequestSession.UserID);
                    }

                    if (bsonDocument.Contains("BUID") && bsonDocument.GetElement("BUID").Value.ToString() == "")
                    {
                        bsonDocument.Set("BUID", RequestSession.BUID);
                    }
                }

                if (bsonDocument.Contains("CreatedBy") && bsonDocument.GetElement("CreatedBy").Value.ToString() == "")
                {
                    bsonDocument.Set("CreatedBy", RequestSession.UserID);
                }

                if (type.Name != "AD_Users")
                {
                    PJUser lVUser = null;
                    if (!string.IsNullOrEmpty(RequestSession.SecurityKey) && !string.IsNullOrEmpty(RequestSession.UserID))
                    {
                        lVUser = UserStore.Get(RequestSession.UserID, RequestSession.SecurityKey);
                    }

                    if (lVUser != null && lVUser.Employee?.EmployeeID != null && bsonDocument.Contains("EmployeeID") && bsonDocument.GetElement("EmployeeID").Value == null)
                    {
                        bsonDocument.Set("EmployeeID", lVUser.Employee.EmployeeID);
                        if (bsonDocument.Contains("PositionID") && bsonDocument.GetElement("PositionID").Value == null)
                        {
                            bsonDocument.Set("PositionID", lVUser.Employee.PositionID);
                        }

                        if (bsonDocument.Contains("OrgUnitID") && bsonDocument.GetElement("OrgUnitID").Value == null)
                        {
                            bsonDocument.Set("OrgUnitID", lVUser.Employee.OrgUnitID);
                        }

                        if (bsonDocument.Contains("DivisionID") && bsonDocument.GetElement("DivisionID").Value == null)
                        {
                            bsonDocument.Set("DivisionID", lVUser.Employee.DivisionID);
                        }

                        if (bsonDocument.Contains("CompanyID") && bsonDocument.GetElement("CompanyID").Value == null)
                        {
                            bsonDocument.Set("CompanyID", lVUser.Employee.CompanyID);
                        }
                    }
                }
            }

            if (bsonDocument.Contains("CreatedOn") && bsonDocument.GetElement("CreatedOn").Value == null)
            {
                bsonDocument.Set("CreatedOn", DateTime.Now);
            }

            if (entity is IBaseEntity)
            {
                IBaseEntity baseEntity = (IBaseEntity)entity;
                if (baseEntity.IncludeTables != null)
                {
                    for (int i = 0; i < baseEntity.IncludeTables.Count; i++)
                    {
                        bsonDocument.Set(baseEntity.IncludeTables[i], null);
                    }
                }
            }

            return bsonDocument;
        }

        private (List<ActionResult>, int) UpdateItems(IMongoCollection<BsonDocument> collection, ConcurrentDictionary<string, IEntity> items, IClientSessionHandle session = null)
        {
            List<ActionResult> list = new List<ActionResult>();
            int num = 0;
            foreach (KeyValuePair<string, IEntity> item in items)
            {
                IEntity value = item.Value;
                try
                {
                    BsonDocument bsonDocument = value.ToBsonDocument(value.GetType());
                    BsonValue value2 = bsonDocument.GetValue("_id");
                    FilterDefinition<BsonDocument> filter = Builders<BsonDocument>.Filter.Eq("_id", value2);
                    BsonDocument bsonDocument2 = session != null ? collection.Find(session, filter).FirstOrDefault() : collection.Find(filter).FirstOrDefault();
                    if (bsonDocument2 == null)
                    {
                        list.Add(new ActionResult($"EFObjectContext_Save_UpdateItems not found id: {value2}")
                        {
                            ObjectData = value
                        });
                        continue;
                    }

                    foreach (BsonElement element in bsonDocument.Elements)
                    {
                        bsonDocument2.Set(element.Name, element.Value);
                    }

                    bsonDocument = bsonDocument2;
                    if (bsonDocument.Contains("ModifiedOn"))
                    {
                        bsonDocument.GetElement("ModifiedOn");
                        bsonDocument.Set("ModifiedOn", DateTime.Now);
                    }

                    if (bsonDocument.Contains("ModifiedBy") && !string.IsNullOrEmpty(RequestSession?.UserID))
                    {
                        bsonDocument.GetElement("ModifiedBy");
                        bsonDocument.Set("ModifiedBy", RequestSession.UserID);
                    }

                    ReplaceOneResult replaceOneResult = session != null ? collection.ReplaceOne(session, filter, bsonDocument) : collection.ReplaceOne(filter, bsonDocument);
                    num += (int)replaceOneResult.ModifiedCount;
                }
                catch (Exception ex)
                {
                    list.Add(new ActionResult("EFObjectContext_Save_UpdateItems", ex)
                    {
                        ObjectData = value
                    });
                    Log.Instance.Error(ex, "EFObjectContext UpdateItems");
                }
            }

            return (list, num);
        }

        private async Task<(List<ActionResult>, int)> UpdateItemsAsync(IMongoCollection<BsonDocument> collection, ConcurrentDictionary<string, IEntity> items, IClientSessionHandle session = null)
        {
            List<ActionResult> list = new List<ActionResult>();
            int num = 0;
            foreach (KeyValuePair<string, IEntity> item in items)
            {
                IEntity value = item.Value;
                try
                {
                    BsonDocument bsonDocument = value.ToBsonDocument(value.GetType());
                    FilterDefinition<BsonDocument> filter = Builders<BsonDocument>.Filter.Eq("_id", bsonDocument.GetValue("_id"));
                    BsonDocument bsonDocument2 = session != null ? collection.Find(session, filter).FirstOrDefault() : collection.Find(filter).FirstOrDefault();
                    foreach (BsonElement element in bsonDocument.Elements)
                    {
                        bsonDocument2.Set(element.Name, element.Value);
                    }

                    bsonDocument = bsonDocument2;
                    if (bsonDocument.Contains("ModifiedOn"))
                    {
                        bsonDocument.GetElement("ModifiedOn");
                        bsonDocument.Set("ModifiedOn", DateTime.Now);
                    }

                    if (bsonDocument.Contains("ModifiedBy") && !string.IsNullOrEmpty(RequestSession?.UserID))
                    {
                        bsonDocument.GetElement("ModifiedBy");
                        bsonDocument.Set("ModifiedBy", RequestSession.UserID);
                    }

                    ReplaceOneResult replaceOneResult = session != null ? collection.ReplaceOne(session, filter, bsonDocument) : collection.ReplaceOne(filter, bsonDocument);
                    num += (int)replaceOneResult.ModifiedCount;
                }
                catch (Exception ex)
                {
                    list.Add(new ActionResult("EFObjectContext_Save_UpdateItemsAsync", ex)
                    {
                        ObjectData = value
                    });
                    Log.Instance.Error(ex, "EFObjectContext UpdateItemsAsync");
                }
            }

            return (list, num);
        }

        private (List<ActionResult>, int) DeleteItems(IMongoCollection<BsonDocument> collection, ConcurrentDictionary<string, IEntity> items, IClientSessionHandle session = null)
        {
            List<ActionResult> list = new List<ActionResult>();
            int num = 0;
            foreach (KeyValuePair<string, IEntity> item in items)
            {
                IEntity value = item.Value;
                try
                {
                    BsonDocument bsonDocument = value.ToBsonDocument(value.GetType());
                    FilterDefinition<BsonDocument> filter = Builders<BsonDocument>.Filter.Eq("_id", bsonDocument.GetValue("_id"));
                    DeleteResult deleteResult = session != null ? collection.DeleteOne(session, filter) : collection.DeleteOne(filter);
                    num += (int)deleteResult.DeletedCount;
                }
                catch (Exception ex)
                {
                    list.Add(new ActionResult("EFObjectContext_Save_DeleteItems", ex)
                    {
                        ObjectData = value
                    });
                    Log.Instance.Error(ex, "EFObjectContext DeleteItems");
                }
            }

            return (list, num);
        }

        private async Task<(List<ActionResult>, int)> DeleteItemsAsync(IMongoCollection<BsonDocument> collection, ConcurrentDictionary<string, IEntity> items, IClientSessionHandle session = null)
        {
            List<ActionResult> list = new List<ActionResult>();
            int num = 0;
            foreach (KeyValuePair<string, IEntity> item in items)
            {
                IEntity value = item.Value;
                try
                {
                    BsonDocument bsonDocument = value.ToBsonDocument(value.GetType());
                    FilterDefinition<BsonDocument> filter = Builders<BsonDocument>.Filter.Eq("_id", bsonDocument.GetValue("_id"));
                    DeleteResult deleteResult = session != null ? collection.DeleteOne(session, filter) : collection.DeleteOne(filter);
                    num += (int)deleteResult.DeletedCount;
                }
                catch (Exception ex)
                {
                    list.Add(new ActionResult("EFObjectContext_Save_DeleteItemsAsync", ex)
                    {
                        ObjectData = value
                    });
                    Log.Instance.Error(ex, "EFObjectContext DeleteItemsAsync");
                }
            }

            return (list, num);
        }

        private int Commit()
        {
            using IClientSessionHandle clientSessionHandle = Client.StartSession();
            clientSessionHandle.StartTransaction();
            try
            {
                int result = Save(clientSessionHandle);
                clientSessionHandle.CommitTransaction();
                return result;
            }
            catch
            {
                clientSessionHandle.AbortTransaction();
                throw;
            }
        }

        private async Task<int> CommitAsync()
        {
            using IClientSessionHandle session = await Client.StartSessionAsync();
            session.StartTransaction();
            try
            {
                int i = await SaveAsync(session);
                await session.CommitTransactionAsync();
                return i;
            }
            catch
            {
                await session.AbortTransactionAsync();
                throw;
            }
        }

        public void SetTimeout(int time)
        {
            TimeSpan connectTimeout = TimeSpan.FromSeconds(time);
            Client.Settings.ConnectTimeout = connectTimeout;
        }

        public virtual void Dispose()
        {
            foreach (KeyValuePair<string, ObjectStateManager> objStateManager in objStateManagers)
            {
                objStateManager.Value.Dispose();
            }

            objStateManagers.Clear();
            GC.SuppressFinalize(this);
        }
    }
}
