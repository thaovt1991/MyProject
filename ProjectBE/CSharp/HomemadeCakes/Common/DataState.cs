using System;

namespace HomemadeCakes.Common
{

    [Flags]
    public enum DataState
    {
        Detached = 1,
        Unchanged = 2,
        Added = 4,
        Deleted = 8,
        Modified = 0x10
    }
}
