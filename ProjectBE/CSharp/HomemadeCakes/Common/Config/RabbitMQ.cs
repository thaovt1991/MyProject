namespace HomemadeCakes.Common.Config
{
    public class RabbitMQ
    {
        public bool IsActive => !string.IsNullOrEmpty(HostName);

        public string Username { get; set; }

        public string Password { get; set; }

        public string HostName { get; set; }

        public int Port { get; set; } = 5670;


        public string VHostUrl { get; set; } = "/";


        public int RetryCount { get; set; } = 5;

    }
}
