using System.Collections.Generic;

namespace HomemadeCakes.Common.Entity
{
    public interface IBaseEntity
    {
        bool Write { get; set; }

        bool Delete { get; set; }

        bool Share { get; set; }

        bool Assign { get; set; }

        List<string> IncludeTables { get; set; }

        string UpdateColumns { get; set; }

        Dictionary<string, object> Unbounds { get; set; }

        object GetUnboundValue(string fieldName);

        object SetUnboundValue(string fieldName, object value);
    }
}
