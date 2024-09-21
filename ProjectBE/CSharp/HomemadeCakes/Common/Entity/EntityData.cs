namespace HomemadeCakes.Common.Entity
{
    public class EntityData
    {
        public string[] Keys { get; set; }

        public object Entity { get; set; }

        public object Original { get; set; }

        public string EntityName { get; set; }

        public DataState State { get; set; }
    }
}
