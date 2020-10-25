using System;
using System.Collections.Generic;

namespace JedzenioPlanner.Api.Domain.Common.SubElements
{
    public class Metadata
    {
        public Metadata()
        {
            Updates = ArraySegment<Update>.Empty;
            Creation = new Creation();
        }
        public double Version { get; set; } = -1;
        public bool Removed { get; set; }
        public Creation Creation { get; set; }
        public IEnumerable<Update> Updates { get; set; }
    }
}