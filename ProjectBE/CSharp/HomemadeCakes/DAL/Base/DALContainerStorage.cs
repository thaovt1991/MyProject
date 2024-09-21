using HomemadeCakes.DAL.Interface;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System;

namespace HomemadeCakes.DAL.Base
{
    public class DALContainerStorage : IDALContainerStorage, IDisposable
    {
        private readonly ConcurrentDictionary<string, IDALContainer> _storage = new ConcurrentDictionary<string, IDALContainer>();

        private readonly object _lockObj = new object();

        public void SetDALContainerForKey(string key, IDALContainer dalContainer)
        {
            lock (_lockObj)
            {
                if (!_storage.ContainsKey(key))
                {
                    _storage.TryAdd(key, dalContainer);
                }
            }
        }

        public IDALContainer GetDALContainerForKey(string key)
        {
            lock (_lockObj)
            {
                if (_storage.ContainsKey(key))
                {
                    return _storage[key];
                }

                return null;
            }
        }

        public void RemoveAllDALContainer()
        {
            lock (_lockObj)
            {
                if (_storage == null)
                {
                    return;
                }

                foreach (KeyValuePair<string, IDALContainer> item in _storage)
                {
                    item.Value.Dispose();
                }

                _storage.Clear();
            }
        }

        public IEnumerable<IDALContainer> GetAllDALContainer()
        {
            return _storage.Values;
        }

        public void Dispose()
        {
            RemoveAllDALContainer();
            GC.SuppressFinalize(this);
        }
    }

}
