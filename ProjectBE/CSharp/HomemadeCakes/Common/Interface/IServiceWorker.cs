using HomemadeCakes.Common.Entity;
using HomemadeCakes.Common.Http;
using System.Collections.Generic;

namespace HomemadeCakes.Common.Interface
{
    public interface IServiceWorker
    {
        void AddParams(MessageBody request, List<EntityData> lstDatas, string DBName = null);

        void DoWorker();
    }
}
