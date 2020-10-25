using System;
using System.IO;
using System.IO.Abstractions;
using System.IO.Enumeration;
using System.Threading.Tasks;
using JedzenioPlanner.Api.Application.Common.Interfaces;
using JedzenioPlanner.Api.Domain.Filestore;
using Microsoft.Extensions.Configuration;

namespace JedzenioPlanner.Api.Infrastructure.Filestore
{
    public class FileStore : IFileStore
    {
        private readonly IFileSystem _fileSystem;
        private readonly IConfiguration _configuration;
        
        public FileStore(IConfiguration configuration)
        {
            // default FS implementation
            _fileSystem = new FileSystem();
            _configuration = configuration;
        }
        
        public FileStore(IFileSystem fileSystem, IConfiguration configuration)
        {
            // for Unit Tests
            _fileSystem = fileSystem;
            _configuration = configuration;
        }
        
        public async Task<byte[]> ReadFile(Guid id)
        {
            try
            {
                return await _fileSystem.File.ReadAllBytesAsync($"{_configuration["DatastorePath"]}{id.ToString()}.file");
            }
            catch
            {
                return new byte[0];
            }
        }

        public async Task<Guid> CreateFile(byte[] content)
        {
            var id = Guid.NewGuid();
            var fs = _fileSystem.File.Create($"{_configuration["DatastorePath"]}{id.ToString()}.file");
            await fs.WriteAsync(content);
            await fs.FlushAsync();
            await fs.DisposeAsync();
            return id; 
        }

        public Task DeleteFile(Guid id)
        {
            _fileSystem.File.Delete($"{_configuration["DatastorePath"]}{id.ToString()}.file");
            return Task.CompletedTask;
        }
    }
}