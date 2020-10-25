using System;
using JedzenioPlanner.Api.Application.Common;
using JedzenioPlanner.Api.Application.Common.Interfaces;

namespace JedzenioPlanner.Api.Infrastructure.Common
{
    public class DateTimeService : IDateTimeService
    {
        public DateTime GetCurrentTime()
        {
            return DateTime.Now;
        }
    }
}