namespace Rhyze.Core.Messages
{
    /// <summary>
    /// The message for enqueuing an uploaded track for further processing
    /// </summary>
    public class TrackUploadedMessage
    {
        /// <summary>
        /// The blob name of the track that was uploaded in the store, without the container name
        /// </summary>
        public string Name { get; set; }
    }
}
