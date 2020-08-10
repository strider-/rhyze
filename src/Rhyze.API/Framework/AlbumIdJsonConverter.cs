using Rhyze.Core.Models;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Rhyze.API.Framework
{
    public class AlbumIdJsonConverter : JsonConverter<AlbumId>
    {
        public override AlbumId Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var id = reader.GetString();

            return new AlbumId(id);
        }

        public override void Write(Utf8JsonWriter writer, AlbumId value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.Value);
        }
    }

}
