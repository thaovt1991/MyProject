using System;

namespace HomemadeCakes.Common
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class CRUD : Attribute
    {
        public DataState State { get; }

        public CRUD(DataState state)
        {
            State = state;
        }
    }
}
