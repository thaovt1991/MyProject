namespace HomemadeCakes.Common.Config
{
    public class AzureServiceBus
    {
        public bool IsActive => !string.IsNullOrEmpty(ConnectionString);

        public string ConnectionString { get; set; }
    }
}
