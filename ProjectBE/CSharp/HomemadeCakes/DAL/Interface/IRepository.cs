using HomemadeCakes.Common;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace HomemadeCakes.DAL.Interface
{
    public interface IRepository : IDisposable
    {
        IUnitOfWork UnitOfWork { get; }

        string DBName { get; }

        void SetCmdTimeout(int time);

        void Add<TEntity>(TEntity entity) where TEntity : class;

        void Update<TEntity>(TEntity entity) where TEntity : class;

        void Delete<TEntity>(TEntity entity) where TEntity : class;

        void Delete<TEntity>(params TEntity[] entities) where TEntity : class;

        void Delete<TEntity>(Expression<Func<TEntity, bool>> criteria) where TEntity : class;

        void Delete<TEntity>(string predicate, params object[] values) where TEntity : class;

        IQueryable<TEntity> Get<TEntity>(Expression<Func<TEntity, bool>> criteria = null) where TEntity : class;

        IAsyncEnumerable<TEntity> GetAsync<TEntity>(Expression<Func<TEntity, bool>> criteria = null) where TEntity : class;

        List<TEntity> GetFromContext<TEntity>(DataState dataState, Func<TEntity, bool> criteria = null) where TEntity : class;

        List<TEntity> GetFromContext<TEntity>(DataState dataState, string predicate, params object[] values) where TEntity : class;

        TEntity GetOne<TEntity>(string predicate, object[] values, bool inContext = false) where TEntity : class;

        Task<TEntity> GetOneAsync<TEntity>(string predicate, object[] values, bool inContext = false) where TEntity : class;

        TEntity GetOne<TEntity>(Expression<Func<TEntity, bool>> criteria = null, bool inContext = false) where TEntity : class;

        Task<TEntity> GetOneAsync<TEntity>(Expression<Func<TEntity, bool>> criteria = null, bool inContext = false) where TEntity : class;

        TEntity GetOriginal<TEntity>(TEntity item) where TEntity : class;

        Task<TEntity> GetOriginalAsync<TEntity>(TEntity item) where TEntity : class;

        int Count<TEntity>(Expression<Func<TEntity, bool>> criteria = null) where TEntity : class;

        Task<int> CountAsync<TEntity>(Expression<Func<TEntity, bool>> criteria = null) where TEntity : class;

        IQueryable<TEntity> GetQuery<TEntity>() where TEntity : class;

        IAsyncEnumerable<TEntity> GetQueryAsync<TEntity>() where TEntity : class;

        Task<int> ExecAsync(string sql);

        Task<int> ExecProcAsync(string sql, params Parameter[] parameters);

        Task<int> ExecProcAsync(string sql, params object[] parameters);

        Task<T> ExecProcAsync<T>(string sql, params Parameter[] parameters);

        Task<T> ExecProcAsync<T>(string sql, params object[] parameters);

        Task<List<T>> ExecScalarAsync<T>(string sql, params Parameter[] parameters);

        Task<List<T>> ExecScalarAsync<T>(string sql, params object[] parameters);

        bool IsExits<TEntity>(Expression<Func<TEntity, bool>> criteria, bool includeContext = false) where TEntity : class;

        Task<bool> IsExitsAsync<TEntity>(Expression<Func<TEntity, bool>> criteria, bool includeContext = false) where TEntity : class;

        bool IsExits<TEntity>(string predicate, object[] dataValue, bool includeContext = false) where TEntity : class;

        Task<bool> IsExitsAsync<TEntity>(string predicate, object[] dataValue, bool includeContext = false) where TEntity : class;
    }

}
