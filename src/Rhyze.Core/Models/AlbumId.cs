using System;
using System.Text;

namespace Rhyze.Core.Models
{
    public class AlbumId
    {
        public AlbumId(string albumArtist, string name)
        {
            AlbumArtist = albumArtist;
            Name = name;
            Value = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{albumArtist}\t{name}"));
        }

        public AlbumId(string value)
        {
            Value = value;

            var tmp = Encoding.UTF8.GetString(Convert.FromBase64String(Value)).Split("\t");
            AlbumArtist = tmp[0];
            Name = tmp[1];
        }

        public string Name { get; }

        public string AlbumArtist { get; }

        public string Value { get; }
    }
}