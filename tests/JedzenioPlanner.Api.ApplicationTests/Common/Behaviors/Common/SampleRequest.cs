﻿namespace JedzenioPlanner.Api.ApplicationTests.Common.Behaviors.Common
{
    public class SampleRequest
    {
        public string Version { get; set; }
        public bool IsWorking { get; set; }

        public override string ToString()
        {
            return "{Version = " + Version + ", IsWorking = " + IsWorking + "}";
        }
    }
}