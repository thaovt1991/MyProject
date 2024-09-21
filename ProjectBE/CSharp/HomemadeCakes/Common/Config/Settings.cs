namespace HomemadeCakes.Common.Config
{
    public class Settings : IConfig
    {
        public bool Loaded { get; set; }

        public string HostUI { get; set; }

        public string APIGateway { get; set; }

        public string MultiDB { get; set; }

        public string DBShare { get; set; }

        public string DBShareType { get; set; }

        public string DBName { get; set; }

        public string DBNameType { get; set; }

        public string Secret { get; set; }

        public string Language { get; set; } = "VN";


        public string PocoAssembly { get; set; }

        public int IdleTimeout { get; set; }

        public string PublicPath { get; set; }

        public string TenantExists { get; set; }

        public string ServiceShares { get; set; }

        public int ConnectTimeout { get; set; } = 1800;


        public bool AuthService { get; set; }

        public bool IsLogFile { get; set; } = true;


        public bool SingleExec { get; set; }

        public string[] MethodByPassAuths { get; set; }

        public string CacheType { get; set; }

        public Mail Mail { get; set; } = new Mail();


        public AzureServiceBus AzureServiceBus { get; set; } = new AzureServiceBus();


        public RabbitMQ RabbitMQ { get; set; } = new RabbitMQ();


        public Kafka Kafka { get; set; } = new Kafka();


        public Elastic Elastic { get; set; } = new Elastic();


        public Grafana Grafana { get; set; } = new Grafana();


        public FCM FCM { get; set; } = new FCM();

    }
}
