using System;

namespace JedzenioPlanner.Api.Domain.Common.SubElements
{
    public class Update
    {
        public double Version { get; set; }
        public string AuthorId { get; set; }
        public DateTime Published { get; set; }
    }
}