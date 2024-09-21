
using HomemadeCakes.Common.Interface;
using System;

namespace HomemadeCakes.DAL.Interface
{
    public interface IDbContext : IDisposable
    {
        IConnection Connection { get; }

        int CommandTimeout { get; }

        void SetTimeout(int time);
    }
}
