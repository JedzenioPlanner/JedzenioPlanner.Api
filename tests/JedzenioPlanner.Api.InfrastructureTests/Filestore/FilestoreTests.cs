using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using JedzenioPlanner.Api.Domain.Filestore;
using JedzenioPlanner.Api.Infrastructure.Filestore;
using Microsoft.Extensions.Configuration;
using Moq;
using Xunit;

namespace JedzenioPlanner.Api.InfrastructureTests.Filestore
{
    public class FilestoreTests
    {
        private IConfiguration BuildFakeConfiguration()
        {
            return new ConfigurationBuilder().AddInMemoryCollection(new[]
            {
                new KeyValuePair<string, string>("DatastorePath", "datastore/"),
            }).Build();
        }
        
        [Fact]
        public async void ShouldReadFileCorrectly()
        {
            var fileMock = new Mock<IFile>();
            fileMock
                .Setup(x => x.ReadAllBytesAsync("datastore/c55811c0-92ee-4346-902a-c302dddb7f9f.file", It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(Convert.FromBase64String("aGVsbG93b3JsZA==")));
            var fsMock = new Mock<IFileSystem>();
            fsMock
                .Setup(x => x.File)
                .Returns(fileMock.Object);
            var fileStore = new FileStore(fsMock.Object, BuildFakeConfiguration());

            var result = await fileStore.ReadFile(new Guid("c55811c0-92ee-4346-902a-c302dddb7f9f"));
            
            result.Should().BeEquivalentTo(Convert.FromBase64String("aGVsbG93b3JsZA=="));
        }

        [Fact]
        public async void ShouldCreateFileCorrectly()
        {
            var ms = new MemoryStream();
            
            var fileMock = new Mock<IFile>();
            fileMock
                .Setup(x => x.Create(It.IsAny<string>()))
                .Returns(ms);
            var fsMock = new Mock<IFileSystem>();
            fsMock
                .Setup(x => x.File)
                .Returns(fileMock.Object);
            var fileStore = new FileStore(fsMock.Object, BuildFakeConfiguration());

            var id = await fileStore.CreateFile(Convert.FromBase64String("aGVsbG93b3JsZA=="));

            id.Should().NotBeEmpty();
            Convert.ToBase64String(ms.ToArray()).Should().BeEquivalentTo("aGVsbG93b3JsZA==");
        }

        [Fact]
        public async void ShouldDeleteFileCorrectly()
        {
            var fileMock = new Mock<IFile>();
            var fsMock = new Mock<IFileSystem>();
            fsMock
                .Setup(x => x.File)
                .Returns(fileMock.Object);
            var fileStore = new FileStore(fsMock.Object, BuildFakeConfiguration());

            await fileStore.DeleteFile(new Guid("c55811c0-92ee-4346-902a-c302dddb7f9f"));
            fileMock.Verify(x => x.Delete($"datastore/c55811c0-92ee-4346-902a-c302dddb7f9f.file"));
            fileMock.VerifyNoOtherCalls();            
        }
    }
}