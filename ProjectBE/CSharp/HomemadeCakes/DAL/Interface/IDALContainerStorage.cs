using System.Collections.Generic;
using System;

namespace HomemadeCakes.DAL.Interface
{
    public interface IDALContainerStorage : IDisposable
    {
        void SetDALContainerForKey(string key, IDALContainer dalContainer);

        IDALContainer GetDALContainerForKey(string key);

        void RemoveAllDALContainer();

        IEnumerable<IDALContainer> GetAllDALContainer();
    }
}
