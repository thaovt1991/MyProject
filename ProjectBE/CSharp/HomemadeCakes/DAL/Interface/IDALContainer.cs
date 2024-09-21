using System;
using HomemadeCakes.Common.Interface;

namespace HomemadeCakes.DAL.Interface
{
    public interface IDALContainer : IDisposable
    {
        IRepository Repository { get; }

        IUnitOfWork UnitOfWork { get; }

        IConnection Connection { get; }

        void SetCmdTimeout(int time);
    }
}
