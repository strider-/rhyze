using Rhyze.Core.Models;
using Xunit;

namespace Rhyze.Tests.Core
{
    [Trait("Core.Models", nameof(AlbumId))]
    public class AlbumIdTests
    {
        [Fact]
        public void A_New_AlbumId_With_Name_And_Artist_Generates_The_Value()
        {
            var expectedValue = "QXJ0aXN0CU5hbWU=";

            var id = new AlbumId("Artist", "Name");

            Assert.Equal(expectedValue, id.Value);
        }

        [Fact]
        public void A_New_AlbumId_With_A_Value_Generates_The_Name_And_Artist()
        {
            string artist = "Artist",
                   name = "Name";
            var id = new AlbumId("QXJ0aXN0CU5hbWU=");

            Assert.Equal(artist, id.AlbumArtist);
            Assert.Equal(name, id.Name);
        }

        [Fact]
        public void An_Invalid_Value_Throws_An_Exception()
        {
            var ex = Record.Exception(() => new AlbumId("whhaaaattt"));

            Assert.NotNull(ex);
        }
    }
}
