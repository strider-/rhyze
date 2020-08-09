using System;

namespace Rhyze.Core.Models
{
    public class Album
    {
        public string Name { get; set; }

        public string Artist { get; set; }

        public string ImageUrl { get; set; }

        public DateTime TouchedUtc { get; set; }
    }
}
