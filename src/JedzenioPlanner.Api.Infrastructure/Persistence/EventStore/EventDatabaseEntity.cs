using System;

namespace JedzenioPlanner.Api.Infrastructure.Persistence.EventStore
{
    public class EventDatabaseEntity
    {
        public Guid EntityId { get; set; }
        public double EntityVersion { get; set; }
        public string EventName { get; set; }
        public DateTime EventPublished { get; set; }
        
        /// <summary>
        /// Event details stored in JSON.
        /// </summary>
        public string EventDetails { get; set; }
    }
}