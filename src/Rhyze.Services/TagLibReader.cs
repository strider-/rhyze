using Rhyze.Core.Interfaces;
using Rhyze.Core.Models;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TagLib;
using static TagLib.File;

namespace Rhyze.Services
{
    public class TagLibReader : ITagReader
    {
        public Task<AudioTag> ReadTagAsync(Stream stream, string contentType)
        {
            using (var file = Create(new StreamAbstraction(stream, contentType)))
            {
                var tag = file.Tag;
                var artwork = tag.Pictures.Where(p => p.Type == PictureType.FrontCover).FirstOrDefault();

                var result = new AudioTag
                {
                    Album = tag.Album,
                    Artist = tag.JoinedPerformers,
                    AlbumArtist = tag.JoinedAlbumArtists,
                    DiscNumber = (int)tag.Disc,
                    Duration = file.Properties.Duration.TotalSeconds,
                    Title = tag.Title,
                    TotalDiscs = (int)tag.DiscCount,
                    TotalTracks = (int)tag.TrackCount,
                    TrackNumber = (int)tag.Track,
                    Year = (int)tag.Year
                };

                if (artwork != null)
                {
                    result.Artwork = new MemoryStream(artwork.Data.Data);
                    result.ArtworkMimeType = artwork.MimeType;
                }

                return Task.FromResult(result);
            }
        }
    }

    internal class StreamAbstraction : IFileAbstraction
    {
        public StreamAbstraction(Stream stream, string contentType)
        {
            ReadStream = stream;
            Name = contentType switch
            {
                "audio/mpeg" => "file.mp3",
                "audio/flac" => "file.flac",
                "audio/x-flac" => "file.flac",
                _ => throw new UnsupportedFormatException($"{contentType} files are not supported (how did you get this far?)")
            };
        }

        public string Name { get; }

        public Stream ReadStream { get; }

        public Stream WriteStream => throw new System.NotImplementedException();

        public void CloseStream(Stream stream) => stream.Close();
    }
}