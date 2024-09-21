using HomemadeCakes.Common.Interface;

namespace HomemadeCakes.Common.Http
{
    public class ConnectionDB : IConnection
    {
        public string Tenant { get; set; }

        public string Service { get; set; }

        public string Type { get; set; }

        public string CnnString { get; set; }

        public string DBSource { get; set; }

        public string DBName { get; set; }

        public string DBSchema { get; set; }

        public bool IsSystem { get; set; }

        public bool IsConfig { get; set; }

        public IConnection Clone()
        {
            return new ConnectionDB
            {
                Tenant = Tenant,
                Service = Service,
                Type = Type,
                CnnString = CnnString,
                DBSource = DBSource,
                DBName = DBName,
                DBSchema = DBSchema,
                IsSystem = IsSystem,
                IsConfig = IsConfig
            };
        }
    }
}
