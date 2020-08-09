using System.Collections.Generic;

namespace Rhyze.Core.Models
{
    /// <summary>
    /// The domain error entity
    /// </summary>
    public class Error
    {
        /// <summary>
        /// Set one or more messages for this error
        /// </summary>
        /// <param name="messages"></param>
        public Error(params string[] messages) => Messages = messages;

        /// <summary>
        /// All the error messages
        /// </summary>
        public IEnumerable<string> Messages { get; }

        /// <summary>
        /// Returns a comma delimited string of all the messages in this error
        /// </summary>
        /// <returns></returns>
        public override string ToString() => string.Join(", ", Messages);
    }
}