using System.Text.Json.Serialization;

namespace Rhyze.API.Models
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum UploadStatus
    {
        Unknown,
        Accepted,
        Rejected
    };

    public class UploadResult
    {
        public string Filename { get; set; }

        public UploadStatus Status { get; set; }

        public string StatusDetail { get; set; }
    }
}
