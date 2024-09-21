using System.Text.Json.Serialization;
using System;

namespace HomemadeCakes.Common.EventBus.Events
{
    public class IntegrationEvent
    {
        public string Name { get; private set; }

        [JsonInclude]
        public Guid Id { get; private set; }

        [JsonInclude]
        public DateTime CreatedDate { get; private set; }

        public IntegrationEvent(string name = "")
            : this(Guid.NewGuid(), DateTime.UtcNow, name)
        {
        }

        [JsonConstructor]
        public IntegrationEvent(Guid id, DateTime createdDate, string name = "")
        {
            Id = id;
            CreatedDate = createdDate;
            Name = name;
            if (string.IsNullOrEmpty(name))
            {
                Name = GetType().Name;
            }
        }
    }
}
