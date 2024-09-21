namespace HomemadeCakes.Common.Interface
{
    //interface conectstring
    public interface IConnection 
    {
        string Tenant { get; set; }

        string Service { get; set; }

        string Type { get; set; }

        string CnnString { get; set; }

        string DBSource { get; set; }

        string DBName { get; set; }

        string DBSchema { get; set; }

        bool IsSystem { get; set; }

        bool IsConfig { get; set; }

        IConnection Clone();
    }
}
