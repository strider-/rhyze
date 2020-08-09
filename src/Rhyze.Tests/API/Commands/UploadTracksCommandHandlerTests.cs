using Microsoft.AspNetCore.Http;
using Moq;
using Rhyze.API.Commands;
using Rhyze.API.Models;
using Rhyze.Core.Interfaces;
using Rhyze.Core.Models;
using System;
using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace Rhyze.Tests.API.Commands
{
    [Trait("API.Commands", nameof(UploadTracksCommandHandler))]
    public class UploadTracksCommandHandlerTests
    {
        private readonly string _contentType = "audio/mpeg";
        private readonly Guid _id = Guid.NewGuid();
        private readonly UploadTracksCommandHandler _handler;
        private readonly Mock<IUploadService> _service = new Mock<IUploadService>();
        private readonly FormFileCollection _files = new FormFileCollection();
        
        public UploadTracksCommandHandlerTests()
        {
            _handler = new UploadTracksCommandHandler(_service.Object);
        }

        [Fact]
        public async Task Handle_Returns_Rejected_Uploads()
        {
            PopulateFiles(1);
            var expectedMsg = "nope";
            var request = new UploadTracksCommand { OwnerId = _id, Tracks = _files };
            _service.Setup(s => s.UploadTrackAsync(_id, _contentType, It.IsAny<Stream>()))
                    .ReturnsAsync(new Error(expectedMsg));

            var result = await _handler.Handle(request, default);

            Assert.NotNull(result);
            var item = Assert.Single(result);
            Assert.Equal("dummy-0.mp3", item.Filename);
            Assert.Equal(UploadStatus.Rejected, item.Status);
            Assert.Equal(expectedMsg, item.StatusDetail);
        }

        [Fact]
        public async Task Handle_Returns_Accepted_Uploads()
        {
            PopulateFiles(1);
            var request = new UploadTracksCommand { OwnerId = _id, Tracks = _files };
            _service.Setup(s => s.UploadTrackAsync(_id, _contentType, It.IsAny<Stream>()))
                    .ReturnsAsync((Error)null);

            var result = await _handler.Handle(request, default);

            Assert.NotNull(result);
            var item = Assert.Single(result);
            Assert.Equal("dummy-0.mp3", item.Filename);
            Assert.Equal(UploadStatus.Accepted, item.Status);
            Assert.Null(item.StatusDetail);
        }

        [Fact]
        public async Task Handle_Can_Process_An_Album()
        {
            PopulateFiles(12);
            var request = new UploadTracksCommand { OwnerId = _id, Tracks = _files };
            _service.SetupSequence(s => s.UploadTrackAsync(_id, _contentType, It.IsAny<Stream>()))
                    .ReturnsAsync((Error)null)
                    .ReturnsAsync((Error)null)
                    .ReturnsAsync((Error)null)
                    .ReturnsAsync(new Error("whoops"))
                    .ReturnsAsync((Error)null)
                    .ReturnsAsync((Error)null)
                    .ReturnsAsync((Error)null)
                    .ReturnsAsync((Error)null)
                    .ReturnsAsync((Error)null)
                    .ReturnsAsync(new Error("oh no"))
                    .ReturnsAsync((Error)null)
                    .ReturnsAsync((Error)null);

            var result = await _handler.Handle(request, default);

            Assert.NotNull(result);
            Assert.Collection(result,
                item => AssertResult(item, "dummy-0.mp3", successful: true),
                item => AssertResult(item, "dummy-1.mp3", successful: true),
                item => AssertResult(item, "dummy-2.mp3", successful: true),
                item => AssertResult(item, "dummy-3.mp3", successful: false, "whoops"),
                item => AssertResult(item, "dummy-4.mp3", successful: true),
                item => AssertResult(item, "dummy-5.mp3", successful: true),
                item => AssertResult(item, "dummy-6.mp3", successful: true),
                item => AssertResult(item, "dummy-7.mp3", successful: true),
                item => AssertResult(item, "dummy-8.mp3", successful: true),
                item => AssertResult(item, "dummy-9.mp3", successful: false, "oh no"),
                item => AssertResult(item, "dummy-10.mp3", successful: true),
                item => AssertResult(item, "dummy-11.mp3", successful: true));
        }

        private void PopulateFiles(int count)
        {
            for (var i = 0; i < count; i++)
            {
                _files.Add(new FormFile(new MemoryStream(), 0, 0, $"dummy-{i}", $"dummy-{i}.mp3")
                {
                    Headers = new HeaderDictionary(),
                    ContentType = _contentType
                });
            }
        }

        private void AssertResult(UploadResult result, string fileName, bool successful, string detail = null)
        {
            Assert.NotNull(result);
            Assert.Equal(fileName, result.Filename);
            Assert.Equal(successful ? UploadStatus.Accepted : UploadStatus.Rejected, result.Status);
            Assert.Equal(detail, result.StatusDetail);
        }
    }
}
