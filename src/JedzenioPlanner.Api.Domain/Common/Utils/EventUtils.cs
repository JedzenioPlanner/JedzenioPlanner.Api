using System;
using System.Collections.Generic;
using System.Linq;
using JedzenioPlanner.Api.Domain.Common.Exceptions;
using JedzenioPlanner.Api.Domain.Common.SubElements;

namespace JedzenioPlanner.Api.Domain.Common.Utils
{
    public static class EventUtils
    {
        private static readonly Dictionary<(Type, string), Type> Types;

        static EventUtils()
        {
            var types = AppDomain.CurrentDomain.GetAssemblies().Where(x => !x.IsDynamic)
                .SelectMany(x => x.ExportedTypes)
                .Where(x => x.BaseType?.IsGenericType == true &&
                            x.BaseType?.GetGenericTypeDefinition() == typeof(Event<>));
            var dic = new Dictionary<(Type, string), Type>();
            foreach (var type in types) dic[(type.BaseType?.GetGenericArguments()[0], type.Name)] = type;
            Types = dic;
        }

        public static T Apply<T>(this Event<T> @event, T entity) where T : AggregateRoot
        {
            if (entity.Metadata.Version >= @event.Version) throw new IncompatibleVersionException();

            entity.Metadata.Version = @event.Version;

            if (@event.Version == 0)
            {
                entity.Id = @event.EntityId;
                entity.Metadata.Creation.Published = @event.Published;
                entity.Metadata.Creation.AuthorId = @event.AuthorId;
            }
            else
            {
                entity.Metadata.Updates = entity.Metadata.Updates.Append(new Update
                {
                    Version = @event.Version,
                    AuthorId = @event.AuthorId,
                    Published = @event.Published
                });
            }

            return @event.Apply(entity);
        }

        public static Type GetEvent<T>(string name) where T : AggregateRoot
        {
            return Types[(typeof(T), name)];
        }
    }
}