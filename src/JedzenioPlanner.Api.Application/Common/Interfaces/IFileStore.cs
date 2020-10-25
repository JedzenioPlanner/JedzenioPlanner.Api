using System;
using System.Threading.Tasks;
using JedzenioPlanner.Api.Domain.Filestore;

namespace JedzenioPlanner.Api.Application.Common.Interfaces
{
    public interface IFileStore
    {
        public Task<byte[]> ReadFile(Guid id);
        public Task<Guid> CreateFile(byte[] content);
        public Task DeleteFile(Guid id);
    }
}