﻿using System;

namespace Rhyze.Core.Models
{
    public class Track
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public string Album { get; set; }

        public string Artist { get; set; }

        public string AlbumArtist { get; set; }

        public int TrackNo { get; set; }

        public int TrackCount { get; set; }

        public int DiscNo { get; set; }

        public int DiscCount { get; set; }

        public int Year { get; set; }

        public double Duration { get; set; }

        public string ImageUrl { get; set; }

        public string AudioUrl { get; set; }

        public DateTime UploadedUtc { get; set; }

        public DateTime? LastPlayedUtc { get; set; }

        public int PlayCount { get; set; }

        public Guid OwnerId { get; set; }
    }
}