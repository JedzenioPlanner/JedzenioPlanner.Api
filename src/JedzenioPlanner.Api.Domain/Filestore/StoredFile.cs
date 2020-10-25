using System;

namespace JedzenioPlanner.Api.Domain.Filestore
{
    public class StoredFile
    {
        public Guid Id { get; set; }
        public string Filetype { get; set; }
        public byte[] Content { get; set; }
    }
}