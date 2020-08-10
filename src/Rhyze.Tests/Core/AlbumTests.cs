using Rhyze.Core.Models;
using Xunit;

namespace Rhyze.Tests.Core
{

    [Trait("Core.Models", nameof(Album))]
    public class AlbumTests
    {
        [Fact]
        public void A_New_Album_Has_No_Id()
        {
            var album = new Album();

            Assert.Null(album.Id);
        }

        [Fact]
        public void A_Name_Alone_Is_Not_Enough_To_Generate_An_Id()
        {
            var album = new Album { Name = "Name" };

            Assert.Null(album.Id);
        }

        [Fact]
        public void An_Artst_Alone_Is_Not_Enough_To_Generate_An_Id()
        {
            var album = new Album { Artist = "Artist" };

            Assert.Null(album.Id);
        }

        [Fact]
        public void A_Name_And_Artist_Generates_An_Id()
        {
            var album = new Album { Name = "Name", Artist = "Artist" };

            Assert.NotNull(album.Id);
        }

        [Theory]
        [InlineData("name")]
        [InlineData(null)]
        public void Changing_The_Name_Generates_A_New_Id(string name)
        {
            var album = new Album { Name = "Test", Artist = "Test Artist" };
            var idValue = album.Id.Value;

            album.Name = name;

            Assert.NotEqual(idValue, album.Id?.Value);
            if (name == null)
            {
                Assert.Null(album.Id);
            }
        }

        [Theory]
        [InlineData("artist")]
        [InlineData(null)]
        public void Changing_The_Artist_Generates_A_New_Id(string artist)
        {
            var album = new Album { Name = "Test", Artist = "Test Artist" };
            var idValue = album.Id.Value;

            album.Artist = artist;

            Assert.NotEqual(idValue, album.Id?.Value);
            if (artist == null)
            {
                Assert.Null(album.Id);
            }
        }
    }
}