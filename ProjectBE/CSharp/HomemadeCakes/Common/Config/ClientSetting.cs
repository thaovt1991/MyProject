namespace HomemadeCakes.Common.Config
{
    public class ClientSetting : IConfig
    {
        public bool Loaded { get; set; }

        public string APIGateway { get; set; }

        public string AuthUrl { get; set; }

        public string Secret { get; set; }

        public int RequestTimeout { get; set; } = 1800;


        public int CookieTime { get; set; } = 7;


        public string ClientType { get; set; } = "1";


        public string Language { get; set; } = "VN";

    }
}
